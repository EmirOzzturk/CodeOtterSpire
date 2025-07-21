using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Action_System;
using Random = UnityEngine.Random;

/// <summary>
/// Döngüsel çekme / atma mantığı ve kart maliyeti–etki zincirini yöneten ana sistem.
/// Tek yere odaklı sorumluluk: kart lifecycle (çekme–elle tutma–oynama–discard)
/// </summary>
public class CardSystem : Singleton<CardSystem>
{
    [Header("References")]
    [SerializeField] private HandView handView;
    [SerializeField] private Transform drawPilePoint;
    [SerializeField] private Transform discardPilePoint;

    [Header("Config")] 
    [SerializeField, Min(1)] private int maxHandSize = 10;
    [SerializeField] private float discardAnimDuration = 0.2f;

    // -------------- Runtime State --------------
    // Queue: O(1) deque + basit random shuffle
    private readonly Queue<Card> drawPile   = new();
    private readonly List<Card>  discardPile = new();
    private readonly List<Card>  hand        = new();
    
    // -------------- Unity Messages --------------
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<DrawCardsGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardsGA>(DiscardAllCardPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardsGA>();
        ActionSystem.DetachPerformer<DiscardAllCardsGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
    }

    // -------------- Public API --------------
    public void Setup(IEnumerable<CardData> deckData)
    {
        drawPile.Clear();
        foreach (var data in deckData)
        {
            drawPile.Enqueue(new Card(data));
        }
        Shuffle(drawPile);
    }
    
    public IReadOnlyList<Card> GetRemainingDrawPile() => drawPile.ToList();
    public IReadOnlyList<Card> GetRemainingDiscardPile() => discardPile;
    public IReadOnlyList<Card> GetRemainingDrawPileForDeckShow()
    {
        Queue<Card> copy = new Queue<Card>(drawPile);
        var sortedCopy = copy.OrderBy(card => card.Title).ToList();
        return sortedCopy;
    }

    // -------------- Performer Implementations --------------
    private IEnumerator DrawCardsPerformer(DrawCardsGA ga)
    {
        int toDraw = ga.Amount;

        while (toDraw-- > 0)
        {
            if (hand.Count >= maxHandSize) break; // taşma engeli

            if (!TryDrawCard(out var card))
            {
                if (discardPile.Count == 0) break; // tamamen boş
                RefillDeck();
                if (!TryDrawCard(out card)) break;
            }

            hand.Add(card);
            var cardView = CardViewCreator.Instance.CreateCardView(card, drawPilePoint.position, drawPilePoint.rotation);
            yield return handView.AddCard(cardView);
        }
    }

    private IEnumerator DiscardAllCardPerformer(DiscardAllCardsGA ga)
    {
        // Kopyayla iterate, hand Remove sırasında bozulmaz
        foreach (var card in hand.ToArray())
        {
            hand.Remove(card);
            yield return DiscardCard(handView.RemoveCard(card));
        }
    }

    private IEnumerator PlayCardPerformer(PlayCardGA ga)
    {
        if (!hand.Remove(ga.Card)) yield break; // Elde yoksa

        yield return DiscardCard(handView.RemoveCard(ga.Card));

        ApplyCardCostAndEffects(ga);
    }

    // -------------- Helper Methods --------------
    private bool TryDrawCard(out Card card)
    {
        if (drawPile.Count > 0)
        {
            card = drawPile.Dequeue();
            return true;
        }
        card = null;
        return false;
    }

    private IEnumerator DiscardCard(CardView cardView)
    {
        if (cardView == null) yield break;

        discardPile.Add(cardView.Card);

        cardView.transform.DOScale(Vector3.zero, discardAnimDuration);
        Tween t = cardView.transform.DOMove(discardPilePoint.position, discardAnimDuration);
        yield return t.WaitForCompletion();

        // TODO: Object Pool ile optimize edilebilir
        Destroy(cardView.gameObject);
    }

    private void RefillDeck()
    {
        foreach (var card in discardPile) drawPile.Enqueue(card);
        discardPile.Clear();
        Shuffle(drawPile);
    }

    private static void Shuffle<T>(Queue<T> queue)
    {
        // Fisher–Yates (queue -> list -> queue) - deck boyutu küçük, gc maliyeti ihmal.
        var list = new List<T>(queue);
        queue.Clear();
        for (int i = list.Count - 1; i >= 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
            queue.Enqueue(list[i]);
        }
    }

    private void ApplyCardCostAndEffects(PlayCardGA ga)
    {
        ActionSystem.Instance.AddReaction(new SpendManaGA(ga.Card.Mana));

        if (ga.Card.ManualTargetEffect != null)
        {
            ActionSystem.Instance.AddReaction(new PerformEffectGA(
                ga.Card.ManualTargetEffect,
                new() { ga.ManualTarget },
                HeroSystem.Instance.HeroView));
        }

        foreach (var perkData in ga.Card.Perks)
            PerkSystem.Instance.AddPerk(new Perk(perkData));

        foreach (var wrapper in ga.Card.OtherEffects)
            ActionSystem.Instance.AddReaction(new PerformEffectGA(
                wrapper.Effect,
                wrapper.TargetMode.GetTargets(),
                HeroSystem.Instance.HeroView));
    }
}
