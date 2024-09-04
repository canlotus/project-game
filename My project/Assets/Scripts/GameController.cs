using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject cardPrefab;
    public Sprite[] cardFrontSprites;
    public Text matchText;
    public Text attemptsText;
    public Text resultText;

    private List<Vector2> cardPositions = new List<Vector2>();
    private List<GameObject> createdCards = new List<GameObject>();
    private int matchCount = 0;
    private int attemptsCount = 0;
    private Card firstCard, secondCard;
    private bool canFlip = false;

    void Start()
    {
        InitializeCardPositions();
        GenerateCards();
        StartCoroutine(ShowAndHideAllCards());
    }

    void InitializeCardPositions()
    {
        // Kartlarýn konumlarýný manuel olarak ayarlýyoruz
        cardPositions.Add(new Vector2(-394, 345));
        cardPositions.Add(new Vector2(-130, 345));
        cardPositions.Add(new Vector2(129, 345));
        cardPositions.Add(new Vector2(392, 345));
        cardPositions.Add(new Vector2(-394, 0));
        cardPositions.Add(new Vector2(-130, 0));
        cardPositions.Add(new Vector2(129, 0));
        cardPositions.Add(new Vector2(392, 0));
        cardPositions.Add(new Vector2(-394, -345));
        cardPositions.Add(new Vector2(-130, -345));
        cardPositions.Add(new Vector2(129, -345));
        cardPositions.Add(new Vector2(392, -345));
        cardPositions.Add(new Vector2(-394, -690));
        cardPositions.Add(new Vector2(-130, -690));
        cardPositions.Add(new Vector2(129, -690));
        cardPositions.Add(new Vector2(392, -690));
    }

    void GenerateCards()
    {
        List<Sprite> cardsToUse = new List<Sprite>();
        foreach (var sprite in cardFrontSprites)
        {
            cardsToUse.Add(sprite);
            cardsToUse.Add(sprite);
        }
        Shuffle(cardsToUse);

        for (int i = 0; i < cardPositions.Count; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, cardPositions[i], Quaternion.identity, transform);
            Card cardScript = newCard.GetComponent<Card>();
            cardScript.SetCardFace(cardsToUse[i]);
            cardScript.OnCardFlipped = OnCardFlipped;
            createdCards.Add(newCard);
        }
    }

    IEnumerator ShowAndHideAllCards()
    {
        foreach (var card in createdCards)
        {
            card.GetComponent<Card>().Flip();
        }

        yield return new WaitForSeconds(3f);

        foreach (var card in createdCards)
        {
            card.GetComponent<Card>().Close();
        }

        canFlip = true;
    }

    void OnCardFlipped(Card card)
    {
        if (!canFlip) return;

        if (firstCard == null)
        {
            firstCard = card;
        }
        else if (secondCard == null)
        {
            secondCard = card;
            canFlip = false;
            StartCoroutine(CheckCards());
        }
    }

    IEnumerator CheckCards()
    {
        yield return new WaitForSeconds(0.5f);

        if (firstCard.GetCardSprite() == secondCard.GetCardSprite())
        {
            firstCard.SetMatched();
            secondCard.SetMatched();
            Destroy(firstCard.gameObject);
            Destroy(secondCard.gameObject);
            matchCount++;
            resultText.text = "Eþleþme bulundu!";
            resultText.color = Color.green;
        }
        else
        {
            firstCard.Close();
            secondCard.Close();
            resultText.text = "Eþleþme yok!";
            resultText.color = Color.red;
        }

        attemptsCount++;
        matchText.text = "Eþleþmeler: " + matchCount;
        attemptsText.text = "Denemeler: " + attemptsCount;

        yield return new WaitForSeconds(1f);
        resultText.text = "";

        firstCard = null;
        secondCard = null;
        canFlip = true;
    }

    void Shuffle(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
