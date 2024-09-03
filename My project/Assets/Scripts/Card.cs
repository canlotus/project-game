using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Image cardImage; // Kartýn yüzünü gösterecek olan Image komponenti
    public Sprite backFaceSprite; // Kapalý yüz için kullanýlacak sprite
    public Sprite frontFaceSprite; // Açýk yüz için kullanýlacak sprite

    private bool isFlipped = false; // Kartýn çevrilip çevrilmediðini takip eder

    void Start()
    {
        // Baþlangýçta kapalý yüzü göster
        cardImage.sprite = backFaceSprite;
    }

    public void SetCardFace(Sprite frontSprite)
    {
        // Açýk yüz sprite'ýný ayarla
        frontFaceSprite = frontSprite;
    }

    public void Flip()
    {
        if (isFlipped)
        {
            // Kartý kapat
            cardImage.sprite = backFaceSprite;
        }
        else
        {
            // Kartý aç
            cardImage.sprite = frontFaceSprite;
        }
        isFlipped = !isFlipped; // Çevirme durumunu deðiþtir
    }
}
