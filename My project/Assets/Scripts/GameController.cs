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
    private bool canFlip = false; // Kartlarýn çevrilebileceðini kontrol eden bayrak

    private Vector2 startPosition = new Vector2(375, 765); // Kartlarýn baþlangýç pozisyonu

    void Start()
    {
        InitializeCardPositions();
        GenerateCards();
        StartCoroutine(DistributeCards()); // Kartlarý daðýtma coroutine'i baþlat
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
            GameObject newCard = Instantiate(cardPrefab, transform); // GameController'ý parent olarak ayarla
            newCard.GetComponent<RectTransform>().anchoredPosition = startPosition; // Kartlarý baþlangýç pozisyonuna yerleþtir
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
            yield return new WaitForSeconds(0.1f); // Her kartýn hareketi arasýnda bekleme süresi
        }

        yield return new WaitForSeconds(3f); // Kartlar yerleþtikten sonra bekleme süresi
        ShowAllCards();
        yield return new WaitForSeconds(3f); // Kartlarýn açýk kalma süresi (3 saniye)
        HideAllCards();
        canFlip = true; // Kartlarýn çevrilebileceðini belirt
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
        if (!canFlip) return; // Kartlar kapalýysa veya iki kart zaten açýksa baþka kart çevrilemez

        if (firstCard == null)
        {
            firstCard = card;
        }
        else if (secondCard == null)
        {
            secondCard = card;
            canFlip = false; // Ýki kart açýldýktan sonra baþka kart açýlmasýný engelle
            SetAllCardsInteractable(false); // Tüm kartlarýn týklanmasýný engelle
            StartCoroutine(CheckCards());
        }
    }

    IEnumerator CheckCards()
    {
        yield return new WaitForSeconds(0.5f); // Biraz bekle

        if (firstCard.GetCardSprite() == secondCard.GetCardSprite())
        {
            // Eþleþme bulundu
            firstCard.SetMatched();
            secondCard.SetMatched();

            // Eþleþmeden önce kartlarý geçici olarak görünmez yap
            firstCard.gameObject.SetActive(false);
            secondCard.gameObject.SetActive(false);

            matchCount++;
            resultText.text = "Match found!";
            resultText.color = Color.green;

            // Eþleþen kartlarý yok et
            yield return new WaitForSeconds(0.5f); // Mesajý gösterdikten sonra bekle
            Destroy(firstCard.gameObject);
            Destroy(secondCard.gameObject);
        }
        else
        {
            // Eþleþme yoksa kartlarý kapat
            yield return new WaitForSeconds(0.5f); // Mesajý gösterdikten sonra bekle
            firstCard.Close();
            secondCard.Close();
            resultText.text = "No match!";
            resultText.color = Color.red;
        }

        attemptsCount++;
        matchText.text = "Matches: " + matchCount;
        attemptsText.text = "Attempts: " + attemptsCount;

        yield return new WaitForSeconds(0.5f); // Sonucu gösterdikten sonra bekle

        resultText.text = "";

        // Eþleþme veya kapanmadan sonra tüm kartlarý tekrar týklanabilir yap
        SetAllCardsInteractable(true);

        firstCard = null;
        secondCard = null;
        canFlip = true; // Yeni tur için tekrar kart çevirmeye izin ver
    }

    void SetAllCardsInteractable(bool state)
    {
        foreach (var card in createdCards)
        {
            if (card != null) // Kartýn hala var olup olmadýðýný kontrol et
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
