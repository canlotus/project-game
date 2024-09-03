using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject cardPrefab; // Kart prefab'�
    public Sprite[] cardFrontSprites; // A��k y�zler i�in kullan�lacak sprite array (8 farkl� sprite)
    public Text scoreText; // Skoru g�sterecek text
    public Text resultText; // Hamle sonucunu g�sterecek text

    private List<Vector2> cardPositions = new List<Vector2>(); // Kartlar�n konumlar�n� tutan liste
    private List<GameObject> createdCards = new List<GameObject>(); // Olu�turulan kartlar� tutan liste
    private int score = 0; // Skor de�eri
    private Card firstCard, secondCard; // �evrilen kartlar

    void Start()
    {
        // Kartlar�n pozisyonlar�n� belirle
        InitializeCardPositions();

        // Kartlar� olu�tur
        GenerateCards();

        // Kartlar� da��t (Coroutine ba�lat)
        StartCoroutine(DistributeCards());
    }

    void InitializeCardPositions()
    {
        // 16 kart�n pozisyonlar�n� belirliyoruz
        // �st s�ra
        cardPositions.Add(new Vector2(-394, 345));
        cardPositions.Add(new Vector2(-130, 345));
        cardPositions.Add(new Vector2(129, 345));
        cardPositions.Add(new Vector2(392, 345));
        // �kinci s�ra
        cardPositions.Add(new Vector2(-394, 0));
        cardPositions.Add(new Vector2(-130, 0));
        cardPositions.Add(new Vector2(129, 0));
        cardPositions.Add(new Vector2(392, 0));
        // ���nc� s�ra
        cardPositions.Add(new Vector2(-394, -345));
        cardPositions.Add(new Vector2(-130, -345));
        cardPositions.Add(new Vector2(129, -345));
        cardPositions.Add(new Vector2(392, -345));
        // D�rd�nc� s�ra
        cardPositions.Add(new Vector2(-394, -690));
        cardPositions.Add(new Vector2(-130, -690));
        cardPositions.Add(new Vector2(129, -690));
        cardPositions.Add(new Vector2(392, -690));
    }

    void GenerateCards()
    {
        // 8 farkl� a��k y�z sprite'�n� iki kopya olarak listeye ekle
        List<Sprite> cardsToUse = new List<Sprite>();
        foreach (var sprite in cardFrontSprites)
        {
            cardsToUse.Add(sprite); // �lk kopya
            cardsToUse.Add(sprite); // �kinci kopya
        }

        // Kartlar� kar��t�r
        Shuffle(cardsToUse);

        // Kartlar� olu�tur ve 375, 765 noktas�na yerle�tir
        Vector2 startPosition = new Vector2(375, 765);
        for (int i = 0; i < 16; i++)
        {
            // Yeni kart prefab'� olu�tur
            GameObject newCard = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, transform);
            newCard.transform.localPosition = startPosition; // Ba�lang�� noktas� 375, 765
            Card cardScript = newCard.GetComponent<Card>();
            cardScript.SetCardFace(cardsToUse[i]); // Her karta rastgele atanm�� bir y�z ver
            cardScript.OnCardFlipped = OnCardFlipped; // Kart�n �evrilmesiyle tetiklenecek olay� ayarla
            createdCards.Add(newCard); // Olu�turulan kartlar� listeye ekle
        }
    }

    // Kartlar� s�rayla da��tacak Coroutine
    IEnumerator DistributeCards()
    {
        for (int i = 0; i < createdCards.Count; i++)
        {
            StartCoroutine(MoveCardToPosition(createdCards[i], cardPositions[i]));
            yield return new WaitForSeconds(0.5f); // Yar�m saniye bekle
        }

        // T�m kartlar da��t�ld�ktan sonra 2 saniye bekle ve kartlar� �evir
        yield return new WaitForSeconds(2f);

        // Kartlar�n �n y�zlerini g�ster
        ShowAllCards();

        // 2 saniye boyunca �n y�zleri g�ster
        yield return new WaitForSeconds(2f);

        // Kartlar� tekrar kapat
        HideAllCards();
    }

    // Bir kart� belirli bir konuma yava��a hareket ettir
    IEnumerator MoveCardToPosition(GameObject card, Vector2 targetPosition)
    {
        float duration = 0.5f; // Hareket s�resi
        float elapsed = 0f;
        Vector2 startingPosition = card.transform.localPosition;

        while (elapsed < duration)
        {
            card.transform.localPosition = Vector2.Lerp(startingPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        card.transform.localPosition = targetPosition; // Hedef konuma tam olarak yerle�tir
    }

    // Kart �evrildi�inde �a�r�l�r
    void OnCardFlipped(Card card)
    {
        if (firstCard == null)
        {
            firstCard = card; // �lk �evrilen kart
        }
        else if (secondCard == null)
        {
            secondCard = card; // �kinci �evrilen kart
            StartCoroutine(CheckCards());
        }
    }

    // Kartlar� kontrol et ve e�le�me durumunu kontrol et
    IEnumerator CheckCards()
    {
        yield return new WaitForSeconds(0.5f); // 0.5 saniye bekle

        if (firstCard.GetCardSprite() == secondCard.GetCardSprite())
        {
            // Kartlar e�le�iyor, yok et
            Destroy(firstCard.gameObject);
            Destroy(secondCard.gameObject);
            UpdateScore(1); // Skoru artt�r
            resultText.text = "E�le�me bulundu!";
        }
        else
        {
            // Kartlar e�le�miyor, geri �evir
            firstCard.Flip();
            secondCard.Flip();
            resultText.text = "E�le�me yok!";
        }

        // Kartlar� s�f�rla
        firstCard = null;
        secondCard = null;
    }

    // Skoru g�ncelle
    void UpdateScore(int amount)
    {
        score += amount;
        StartCoroutine(FlashScoreText());
        scoreText.text = "Score: " + score;
    }

    // Skor textini yan�p s�nd�r
    IEnumerator FlashScoreText()
    {
        Color originalColor = scoreText.color;
        scoreText.color = Color.green; // Ye�il yap
        yield return new WaitForSeconds(1f);
        scoreText.color = originalColor; // Orijinal renge geri d�n
    }

    // T�m kartlar�n �n y�zlerini g�ster
    void ShowAllCards()
    {
        foreach (var card in createdCards)
        {
            Card cardScript = card.GetComponent<Card>();
            cardScript.Flip(); // Kart� �evir (�n y�z� g�ster)
        }
    }

    // T�m kartlar� kapat
    void HideAllCards()
    {
        foreach (var card in createdCards)
        {
            Card cardScript = card.GetComponent<Card>();
            cardScript.Flip(); // Kart� tekrar �evir (kapal� y�z� g�ster)
        }
    }

    // Basit bir kar��t�rma algoritmas�
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
