using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // SceneManager i�in gerekli

public class GameController : MonoBehaviour
{
    public GameObject cardPrefab;
    public Sprite[] cardFrontSprites;
    public Text matchText;
    public Text attemptsText;
    public Text resultText;
    public ParticleSystem[] particleEffects; // 16 tane particle effect referans�

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

        // Ba�lang��ta t�m particle efektleri kapat
        foreach (ParticleSystem ps in particleEffects)
        {
            ps.gameObject.SetActive(false);
        }
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
        int numberOfCards = GameSettings.numberOfCards; // Oyun zorlu�una g�re kart say�s�n� al
        List<Sprite> cardsToUse = new List<Sprite>();

        for (int i = 0; i < numberOfCards / 2; i++) // Her bir e�lemeden 2 adet
        {
            cardsToUse.Add(cardFrontSprites[i]);
            cardsToUse.Add(cardFrontSprites[i]);
        }
        Shuffle(cardsToUse);

        for (int i = 0; i < numberOfCards; i++)
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

        yield return new WaitForSeconds(1f); // Kartlar yerle�tikten sonra bekleme s�resi
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

            // E�le�me sesi �al
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

            // E�le�en kartlar� yok et
            yield return new WaitForSeconds(0.5f); // Mesaj� g�sterdikten sonra bekle
            Destroy(firstCard.gameObject);
            Destroy(secondCard.gameObject);

            // T�m kartlar e�le�tiyse oyunu sonland�r
            if (matchCount * 2 == createdCards.Count) // T�m kartlar e�le�mi�se
            {
                yield return new WaitForSeconds(0.5f); // K�sa bir bekleme
                AudioManager.instance.PlayGameEndSound(); // Oyun biti� sesini �al
                yield return new WaitForSeconds(AudioManager.instance.gameEndSound.length); // Sesin s�resi kadar bekle
                SceneManager.LoadScene("MainScene"); // Ana men�ye d�n
            }
        }
        else
        {
            // E�le�me yoksa kartlar� kapat
            yield return new WaitForSeconds(0.5f); // Mesaj� g�sterdikten sonra bekle
            firstCard.Close();
            secondCard.Close();

            // E�le�meme sesi �al
            AudioManager.instance.PlayNoMatchSound();

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

    IEnumerator DeactivateParticleEffect(ParticleSystem ps, float delay)
    {
        yield return new WaitForSeconds(delay);
        ps.Stop();
        ps.gameObject.SetActive(false);
    }
}
