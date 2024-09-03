using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public GameObject cardPrefab; // Kart prefab'�
    public Sprite[] cardFrontSprites; // A��k y�zler i�in kullan�lacak sprite array (8 farkl� sprite)

    private List<Vector2> cardPositions = new List<Vector2>(); // Kartlar�n konumlar�n� tutan liste

    void Start()
    {
        // Kartlar�n pozisyonlar�n� belirle
        InitializeCardPositions();

        // Kartlar� olu�tur ve konumland�r
        GenerateCards();
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

        // Kartlar� olu�tur ve konumland�r
        for (int i = 0; i < 16; i++)
        {
            // Yeni kart prefab'� olu�tur
            GameObject newCard = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, transform);
            newCard.transform.localPosition = cardPositions[i];
            Card cardScript = newCard.GetComponent<Card>();
            cardScript.SetCardFace(cardsToUse[i]); // Her karta rastgele atanm�� bir y�z ver
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
