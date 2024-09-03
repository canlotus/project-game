using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject cardPrefab;
    public Sprite[] cardFrontSprites; // A��k y�zler i�in kullan�lacak sprite array

    void Start()
    {
        // �rnek olarak 4 kart olu�turup y�zlerini ayarlama
        for (int i = 0; i < 4; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
            Card cardScript = newCard.GetComponent<Card>();
            cardScript.SetCardFace(cardFrontSprites[i]); // Her karta farkl� bir y�z atar
        }
    }
}