using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject cardPrefab;
    public Sprite[] cardFrontSprites; // Açýk yüzler için kullanýlacak sprite array

    void Start()
    {
        // Örnek olarak 4 kart oluþturup yüzlerini ayarlama
        for (int i = 0; i < 4; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
            Card cardScript = newCard.GetComponent<Card>();
            cardScript.SetCardFace(cardFrontSprites[i]); // Her karta farklý bir yüz atar
        }
    }
}