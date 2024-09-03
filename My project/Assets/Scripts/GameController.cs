using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject cardPrefab; // Kart prefab'ý
    public Sprite[] cardFrontSprites; // Açýk yüzler için kullanýlacak sprite array (8 farklý sprite)
    public Text scoreText; // Skoru gösterecek text
    public Text resultText; // Hamle sonucunu gösterecek text

    private List<Vector2> cardPositions = new List<Vector2>(); // Kartlarýn konumlarýný tutan liste
    private List<GameObject> createdCards = new List<GameObject>(); // Oluþturulan kartlarý tutan liste
    private int score = 0; // Skor deðeri
    private Card firstCard, secondCard; // Çevrilen kartlar

    void Start()
    {
        // Kartlarýn pozisyonlarýný belirle
        InitializeCardPositions();

        // Kartlarý oluþtur
        GenerateCards();

        // Kartlarý daðýt (Coroutine baþlat)
        StartCoroutine(DistributeCards());
    }

    void InitializeCardPositions()
    {
        // 16 kartýn pozisyonlarýný belirliyoruz
        // Üst sýra
        cardPositions.Add(new Vector2(-394, 345));
        cardPositions.Add(new Vector2(-130, 345));
        cardPositions.Add(new Vector2(129, 345));
        cardPositions.Add(new Vector2(392, 345));
        // Ýkinci sýra
        cardPositions.Add(new Vector2(-394, 0));
        cardPositions.Add(new Vector2(-130, 0));
        cardPositions.Add(new Vector2(129, 0));
        cardPositions.Add(new Vector2(392, 0));
        // Üçüncü sýra
        cardPositions.Add(new Vector2(-394, -345));
        cardPositions.Add(new Vector2(-130, -345));
        cardPositions.Add(new Vector2(129, -345));
        cardPositions.Add(new Vector2(392, -345));
        // Dördüncü sýra
        cardPositions.Add(new Vector2(-394, -690));
        cardPositions.Add(new Vector2(-130, -690));
        cardPositions.Add(new Vector2(129, -690));
        cardPositions.Add(new Vector2(392, -690));
    }

    void GenerateCards()
    {
        // 8 farklý açýk yüz sprite'ýný iki kopya olarak listeye ekle
        List<Sprite> cardsToUse = new List<Sprite>();
        foreach (var sprite in cardFrontSprites)
        {
            cardsToUse.Add(sprite); // Ýlk kopya
            cardsToUse.Add(sprite); // Ýkinci kopya
        }

        // Kartlarý karýþtýr
        Shuffle(cardsToUse);

        // Kartlarý oluþtur ve 375, 765 noktasýna yerleþtir
        Vector2 startPosition = new Vector2(375, 765);
        for (int i = 0; i < 16; i++)
        {
            // Yeni kart prefab'ý oluþtur
            GameObject newCard = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, transform);
            newCard.transform.localPosition = startPosition; // Baþlangýç noktasý 375, 765
            Card cardScript = newCard.GetComponent<Card>();
            cardScript.SetCardFace(cardsToUse[i]); // Her karta rastgele atanmýþ bir yüz ver
            cardScript.OnCardFlipped = OnCardFlipped; // Kartýn çevrilmesiyle tetiklenecek olayý ayarla
            createdCards.Add(newCard); // Oluþturulan kartlarý listeye ekle
        }
    }

    // Kartlarý sýrayla daðýtacak Coroutine
    IEnumerator DistributeCards()
    {
        for (int i = 0; i < createdCards.Count; i++)
        {
            StartCoroutine(MoveCardToPosition(createdCards[i], cardPositions[i]));
            yield return new WaitForSeconds(0.5f); // Yarým saniye bekle
        }

        // Tüm kartlar daðýtýldýktan sonra 2 saniye bekle ve kartlarý çevir
        yield return new WaitForSeconds(2f);

        // Kartlarýn ön yüzlerini göster
        ShowAllCards();

        // 2 saniye boyunca ön yüzleri göster
        yield return new WaitForSeconds(2f);

        // Kartlarý tekrar kapat
        HideAllCards();
    }

    // Bir kartý belirli bir konuma yavaþça hareket ettir
    IEnumerator MoveCardToPosition(GameObject card, Vector2 targetPosition)
    {
        float duration = 0.5f; // Hareket süresi
        float elapsed = 0f;
        Vector2 startingPosition = card.transform.localPosition;

        while (elapsed < duration)
        {
            card.transform.localPosition = Vector2.Lerp(startingPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        card.transform.localPosition = targetPosition; // Hedef konuma tam olarak yerleþtir
    }

    // Kart çevrildiðinde çaðrýlýr
    void OnCardFlipped(Card card)
    {
        if (firstCard == null)
        {
            firstCard = card; // Ýlk çevrilen kart
        }
        else if (secondCard == null)
        {
            secondCard = card; // Ýkinci çevrilen kart
            StartCoroutine(CheckCards());
        }
    }

    // Kartlarý kontrol et ve eþleþme durumunu kontrol et
    IEnumerator CheckCards()
    {
        yield return new WaitForSeconds(0.5f); // 0.5 saniye bekle

        if (firstCard.GetCardSprite() == secondCard.GetCardSprite())
        {
            // Kartlar eþleþiyor, yok et
            Destroy(firstCard.gameObject);
            Destroy(secondCard.gameObject);
            UpdateScore(1); // Skoru arttýr
            resultText.text = "Eþleþme bulundu!";
        }
        else
        {
            // Kartlar eþleþmiyor, geri çevir
            firstCard.Flip();
            secondCard.Flip();
            resultText.text = "Eþleþme yok!";
        }

        // Kartlarý sýfýrla
        firstCard = null;
        secondCard = null;
    }

    // Skoru güncelle
    void UpdateScore(int amount)
    {
        score += amount;
        StartCoroutine(FlashScoreText());
        scoreText.text = "Score: " + score;
    }

    // Skor textini yanýp söndür
    IEnumerator FlashScoreText()
    {
        Color originalColor = scoreText.color;
        scoreText.color = Color.green; // Yeþil yap
        yield return new WaitForSeconds(1f);
        scoreText.color = originalColor; // Orijinal renge geri dön
    }

    // Tüm kartlarýn ön yüzlerini göster
    void ShowAllCards()
    {
        foreach (var card in createdCards)
        {
            Card cardScript = card.GetComponent<Card>();
            cardScript.Flip(); // Kartý çevir (ön yüzü göster)
        }
    }

    // Tüm kartlarý kapat
    void HideAllCards()
    {
        foreach (var card in createdCards)
        {
            Card cardScript = card.GetComponent<Card>();
            cardScript.Flip(); // Kartý tekrar çevir (kapalý yüzü göster)
        }
    }

    // Basit bir karýþtýrma algoritmasý
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
