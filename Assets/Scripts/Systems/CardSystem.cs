using System;
using System.Collections;
using System.Collections.Generic;
using Action_System;
using DG.Tweening;
using UnityEngine;

namespace Systems.Card_System
{
    public class CardSystem : Singleton<CardSystem>
    {
        [SerializeField] private HandView handView;
        [SerializeField] private Transform drawPilePoint;
        [SerializeField] private Transform discardPilePoint;
        
        private readonly List<Card> drawPile = new();
        private readonly List<Card> discardPile = new();
        private readonly List<Card> hand = new();
        

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
        
        // Publics
        public void Setup(List<CardData> deckData)
        {
            foreach (var cardData in deckData)
            {
                Card card = new(cardData);
                drawPile.Add(card);
            }
        }

        // Performers
        private IEnumerator DrawCardsPerformer(DrawCardsGA drawCardsGa)
        {
            int actualAmount = Mathf.Min(drawCardsGa.Amount, drawPile.Count);
            int notDrawnAmount = drawCardsGa.Amount - actualAmount;
            for (int i = 0; i < actualAmount; i++)
            {
                yield return DrawCard();
            }

            if (notDrawnAmount > 0)
            {
                RefillDeck();
                for (int i = 0; i < notDrawnAmount; i++)
                {
                    yield return DrawCard();
                }
            }
        }
        private IEnumerator DiscardAllCardPerformer(DiscardAllCardsGA discardAllCardsGa)
        {
            foreach (var card in hand)
            {
                CardView cardView = handView.RemoveCard(card);
                yield return DiscardCard(cardView);
            }
        }

        private IEnumerator PlayCardPerformer(PlayCardGA playCardGa)
        {
            discardPile.Add(playCardGa.Card);
            hand.Remove(playCardGa.Card);
            CardView cardView = handView.RemoveCard(playCardGa.Card);
            yield return DiscardCard(cardView);

            foreach (var perkData in playCardGa.Card.Perks)
            {
                PerkSystem.Instance.AddPerk(new Perk(perkData));
            }
            
            SpendManaGA spendManaGa = new SpendManaGA(playCardGa.Card.Mana);
            ActionSystem.Instance.AddReaction(spendManaGa);

            if (playCardGa.Card.ManualTargetEffect != null)
            {
                PerformEffectGA performEffectGa =
                    new(playCardGa.Card.ManualTargetEffect, new() { playCardGa.ManualTarget }, HeroSystem.Instance.HeroView);
                ActionSystem.Instance.AddReaction(performEffectGa);
            }
            foreach (var effectWrapper in playCardGa.Card.OtherEffects)
            {
                List<CombatantView> targets = effectWrapper.TargetMode.GetTargets();
                PerformEffectGA performEffectGa = new(effectWrapper.Effect, targets, HeroSystem.Instance.HeroView);
                ActionSystem.Instance.AddReaction(performEffectGa);
            }
        }
        
        // Helpers
        private IEnumerator DiscardCard(CardView cardView)
        {
            if (cardView == null) yield break;
            discardPile.Add(cardView.Card);
            cardView.transform.DOScale(Vector3.zero, 0.2f);
            Tween tween = cardView.transform.DOMove(discardPilePoint.position, 0.2f);
            yield return tween.WaitForCompletion();
            Destroy(cardView.gameObject);
        }
        private void RefillDeck()
        {
            drawPile.AddRange(discardPile);
            discardPile.Clear();
        }
        private IEnumerator DrawCard()
        {
            if (drawPile.Count == 0) yield break;
            
            Card card = drawPile.Draw();
            hand.Add(card);
            CardView cardView = CardViewCreator.Instance.CreateCardView(card, drawPilePoint.position, drawPilePoint.rotation);
            yield return handView.AddCard(cardView);
        }

    }
}