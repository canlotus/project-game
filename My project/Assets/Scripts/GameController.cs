using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public GameObject cardPrefab; // Kart prefab'ý
    public Sprite[] cardFrontSprites; // Açýk yüzler için kullanýlacak sprite array (8 farklý sprite)

    private List<Vector2> cardPositions = new List<Vector2>(); // Kartlarýn konumlarýný tutan liste

    void Start()
    {
        // Kartlarýn pozisyonlarýný belirle
        InitializeCardPositions();

        // Kartlarý oluþtur ve konumlandýr
        GenerateCards();
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

        // Kartlarý oluþtur ve konumlandýr
        for (int i = 0; i < 16; i++)
        {
            // Yeni kart prefab'ý oluþtur
            GameObject newCard = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, transform);
            newCard.transform.localPosition = cardPositions[i];
            Card cardScript = newCard.GetComponent<Card>();
            cardScript.SetCardFace(cardsToUse[i]); // Her karta rastgele atanmýþ bir yüz ver
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
