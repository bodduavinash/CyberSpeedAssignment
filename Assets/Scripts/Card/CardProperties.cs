using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardProperties : MonoBehaviour
{
    [SerializeField] private Image cardImage;

    private int _cardIndex = 0;

    public Image GetCardImage()
    {
        return cardImage;
    }

    public void SetCardIndex(int index)
    {
        _cardIndex = index;
    }

    private void SetCardSprite()
    {
        cardImage.sprite = CardController.instance.GetSelectedCardSpriteFromList(_cardIndex);
    }

    public void OnCardButtonClicked()
    {
        SetCardSprite();
        gameObject.GetComponent<Button>().enabled = false;

        SelectedCardInfo cardInfo = new SelectedCardInfo();
        cardInfo.sprite = CardController.instance.GetSelectedCardSpriteFromList(_cardIndex);
        cardInfo.gameObject = gameObject;

        CardController.instance.CompareClickedCards(cardInfo);
    }
}