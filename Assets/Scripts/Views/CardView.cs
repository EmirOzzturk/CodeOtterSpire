using System;
using Action_System;
using TMPro; 
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CardView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text cardType;
    [SerializeField] private TMP_Text mana;
    [SerializeField] private SpriteRenderer cardImageSr;

    [Header("Background")]
    [SerializeField] private SpriteRenderer backgroundSr;            // <-- arka planı çizen SR
    [SerializeField] private Sprite redBg;    // 4 farklı sprite
    [SerializeField] private Sprite yellowBg;
    [SerializeField] private Sprite whiteBg;
    [SerializeField] private Sprite greenBg;

    [Header("Misc")]
    [SerializeField] private GameObject wrapper;
    [SerializeField] private LayerMask dropLayer;
    public Card Card {get; private set;}
    private Vector3 dragStartPos;
    private Quaternion dragStartRot;

    public void Setup(Card card)
    {
        if (card == null)
        {
            return;
        }

        Card = card;
        title.text = card.Title;
        description.text = card.Description;
        cardType.text = card.CardTypeString;
        mana.text = card.Mana.ToString();
        cardImageSr.sprite = card.Image;
        backgroundSr.sprite = GetBackgroundSprite(card);
    }
    
    // Mouse Actions
    void OnMouseEnter()
    {
        if (!InteractionSystem.Instance.PlayerCanHover()) return;
        wrapper.SetActive(false);
        Vector3 pos = new(transform.position.x, -2.5f, 0);
        CardViewHoverSystem.Instance.Show(Card, pos);
    }
    void OnMouseExit()
    {
        if (!InteractionSystem.Instance.PlayerCanHover()) return;
        CardViewHoverSystem.Instance.Hide();
        wrapper.SetActive(true);
    }
    void OnMouseDown()
    {
        if (!InteractionSystem.Instance.PlayerCanInteract()) return;
        if (Card.ManualTargetEffect != null)
        {
            ManualTargetSystem.Instance.StartTargeting(transform.position);
        }
        else
        {
            InteractionSystem.Instance.PlayerIsDragging = true;
            wrapper.SetActive(true);
            CardViewHoverSystem.Instance.Hide();
            dragStartPos = transform.position;
            dragStartRot = transform.rotation;
            transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    void OnMouseDrag()
    {
        if (!InteractionSystem.Instance.PlayerCanInteract()) return;
        if (Card.ManualTargetEffect != null) return;
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
    }
    private void OnMouseUp()
    {
        if (!InteractionSystem.Instance.PlayerCanInteract()) return;
        if (Card.ManualTargetEffect != null)
        {
            EnemyView target = ManualTargetSystem.Instance.EndTargeting(MouseUtil.GetMousePositionInWorldSpace(-1));
            if (target != null && ManaSystem.Instance.HasEnoughMana(Card.Mana))
            {
                PlayCardGA playCardGa = new(Card, target);
                ActionSystem.Instance.Perform(playCardGa);
            }
        }
        else
        {
            if (ManaSystem.Instance.HasEnoughMana(Card.Mana)
                && Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f, dropLayer))
            {
                PlayCardGA playCardGa = new(Card);
                ActionSystem.Instance.Perform(playCardGa);
            }
            else
            {
                transform.position = dragStartPos;
                transform.rotation = dragStartRot;
            }
            InteractionSystem.Instance.PlayerIsDragging = false;
        }
    }
    
    private Sprite GetBackgroundSprite(Card card)
    {
        return card.cardData.CardColor switch
        {
            CardColor.RED    => redBg,
            CardColor.GREEN  => greenBg,
            CardColor.WHITE  => whiteBg,
            CardColor.YELLOW => yellowBg,
            _                => whiteBg,
        };
    }
}
