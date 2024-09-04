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
    private bool canFlip = false; // Kartlar�n �evrilebilece�ini kontrol eden bayrak

    private Vector2 startPosition = new Vector2(375, 765); // Kartlar�n ba�lang�� pozisyonu

    void Start()
    {
        InitializeCardPositions();
        GenerateCards();
        StartCoroutine(DistributeCards()); // Kartlar� da��tma coroutine'i ba�lat
    }

    void InitializeCardPositions()
    {
        // Kartlar�n konumlar�n� manuel olarak ayarl�yoruz
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
            GameObject newCard = Instantiate(cardPrefab, transform); // GameController'� parent olarak ayarla
            newCard.GetComponent<RectTransform>().anchoredPosition = startPosition; // Kartlar� ba�lang�� pozisyonuna yerle�tir
            Card cardScript = newCard.GetComponent<Card>();
            cardScript.SetCardFace(cardsToUse[i]);
            cardScript.OnCardFlipped = OnCardFlipped;
            createdCards.Add(newCard);
        }
    }

    IEnumerator DistributeCards()
    {
        for (int i = 0; i < createdCards.Count; i++)
        {
            StartCoroutine(MoveCardToPosition(createdCards[i], cardPositions[i]));
            yield return new WaitForSeconds(0.1f); // Her kart�n hareketi aras�nda bekleme s�resi
        }

        yield return new WaitForSeconds(3f); // Kartlar yerle�tikten sonra bekleme s�resi
        ShowAllCards();
        yield return new WaitForSeconds(3f); // Kartlar�n a��k kalma s�resi (3 saniye)
        HideAllCards();
        canFlip = true; // Kartlar�n �evrilebilece�ini belirt
    }

    IEnumerator MoveCardToPosition(GameObject card, Vector2 targetPosition)
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Vector2 startingPosition = card.GetComponent<RectTransform>().anchoredPosition;

        while (elapsed < duration)
        {
            card.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startingPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        card.GetComponent<RectTransform>().anchoredPosition = targetPosition;
    }

    void ShowAllCards()
    {
        foreach (var card in createdCards)
        {
            card.GetComponent<Card>().Flip();
        }
    }

    void HideAllCards()
    {
        foreach (var card in createdCards)
        {
            card.GetComponent<Card>().Close();
        }
    }

    void OnCardFlipped(Card card)
    {
        if (!canFlip) return; // Kartlar kapal�ysa veya iki kart zaten a��ksa ba�ka kart �evrilemez

        if (firstCard == null)
        {
            firstCard = card;
        }
        else if (secondCard == null)
        {
            secondCard = card;
            canFlip = false; // �ki kart a��ld�ktan sonra ba�ka kart a��lmas�n� engelle
            SetAllCardsInteractable(false); // T�m kartlar�n t�klanmas�n� engelle
            StartCoroutine(CheckCards());
        }
    }

    IEnumerator CheckCards()
    {
        yield return new WaitForSeconds(0.5f); // Biraz bekle

        if (firstCard.GetCardSprite() == secondCard.GetCardSprite())
        {
            // E�le�me bulundu
            firstCard.SetMatched();
            secondCard.SetMatched();

            // E�le�meden �nce kartlar� ge�ici olarak g�r�nmez yap
            firstCard.gameObject.SetActive(false);
            secondCard.gameObject.SetActive(false);

            matchCount++;
            resultText.text = "Match found!";
            resultText.color = Color.green;

            // E�le�en kartlar� yok et
            yield return new WaitForSeconds(0.5f); // Mesaj� g�sterdikten sonra bekle
            Destroy(firstCard.gameObject);
            Destroy(secondCard.gameObject);
        }
        else
        {
            // E�le�me yoksa kartlar� kapat
            yield return new WaitForSeconds(0.5f); // Mesaj� g�sterdikten sonra bekle
            firstCard.Close();
            secondCard.Close();
            resultText.text = "No match!";
            resultText.color = Color.red;
        }

        attemptsCount++;
        matchText.text = "Matches: " + matchCount;
        attemptsText.text = "Attempts: " + attemptsCount;

        yield return new WaitForSeconds(0.5f); // Sonucu g�sterdikten sonra bekle

        resultText.text = "";

        // E�le�me veya kapanmadan sonra t�m kartlar� tekrar t�klanabilir yap
        SetAllCardsInteractable(true);

        firstCard = null;
        secondCard = null;
        canFlip = true; // Yeni tur i�in tekrar kart �evirmeye izin ver
    }

    void SetAllCardsInteractable(bool state)
    {
        foreach (var card in createdCards)
        {
            if (card != null) // Kart�n hala var olup olmad���n� kontrol et
            {
                card.GetComponent<Card>().SetInteractable(state);
            }
        }
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
