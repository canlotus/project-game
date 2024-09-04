using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // SceneManager için gerekli

public class GameController : MonoBehaviour
{
    public GameObject cardPrefab;
    public Sprite[] cardFrontSprites;
    public Text matchText;
    public Text attemptsText;
    public Text resultText;
    public ParticleSystem[] particleEffects; // 16 tane particle effect referansý

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

        // Baþlangýçta tüm particle efektleri kapat
        foreach (ParticleSystem ps in particleEffects)
        {
            ps.gameObject.SetActive(false);
        }
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
        int numberOfCards = GameSettings.numberOfCards; // Oyun zorluðuna göre kart sayýsýný al
        List<Sprite> cardsToUse = new List<Sprite>();

        for (int i = 0; i < numberOfCards / 2; i++) // Her bir eþlemeden 2 adet
        {
            cardsToUse.Add(cardFrontSprites[i]);
            cardsToUse.Add(cardFrontSprites[i]);
        }
        Shuffle(cardsToUse);

        for (int i = 0; i < numberOfCards; i++)
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

        yield return new WaitForSeconds(1f); // Kartlar yerleþtikten sonra bekleme süresi
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

            // Eþleþme sesi çal
            AudioManager.instance.PlayMatchSound();

            // Particle effect tetikle
            int firstCardIndex = createdCards.IndexOf(firstCard.gameObject);
            int secondCardIndex = createdCards.IndexOf(secondCard.gameObject);

            if (firstCardIndex >= 0 && firstCardIndex < particleEffects.Length)
            {
                particleEffects[firstCardIndex].gameObject.SetActive(true);
                particleEffects[firstCardIndex].Play();
                StartCoroutine(DeactivateParticleEffect(particleEffects[firstCardIndex], 1f));
            }

            if (secondCardIndex >= 0 && secondCardIndex < particleEffects.Length)
            {
                particleEffects[secondCardIndex].gameObject.SetActive(true);
                particleEffects[secondCardIndex].Play();
                StartCoroutine(DeactivateParticleEffect(particleEffects[secondCardIndex], 1f));
            }

            matchCount++;
            resultText.text = "Match found!";
            resultText.color = Color.green;

            // Eþleþen kartlarý yok et
            yield return new WaitForSeconds(0.5f); // Mesajý gösterdikten sonra bekle
            Destroy(firstCard.gameObject);
            Destroy(secondCard.gameObject);

            // Tüm kartlar eþleþtiyse oyunu sonlandýr
            if (matchCount * 2 == createdCards.Count) // Tüm kartlar eþleþmiþse
            {
                yield return new WaitForSeconds(0.5f); // Kýsa bir bekleme
                AudioManager.instance.PlayGameEndSound(); // Oyun bitiþ sesini çal
                yield return new WaitForSeconds(AudioManager.instance.gameEndSound.length); // Sesin süresi kadar bekle
                SceneManager.LoadScene("MainScene"); // Ana menüye dön
            }
        }
        else
        {
            // Eþleþme yoksa kartlarý kapat
            yield return new WaitForSeconds(0.5f); // Mesajý gösterdikten sonra bekle
            firstCard.Close();
            secondCard.Close();

            // Eþleþmeme sesi çal
            AudioManager.instance.PlayNoMatchSound();

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

    IEnumerator DeactivateParticleEffect(ParticleSystem ps, float delay)
    {
        yield return new WaitForSeconds(delay);
        ps.Stop();
        ps.gameObject.SetActive(false);
    }
}
