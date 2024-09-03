using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Image backFace; // Kapalý yüzü gösterecek Image
    public Image frontFace; // Açýk yüzü gösterecek Image
    private bool isFlipped = false; // Kartýn çevrilip çevrilmediðini takip eder

    void Start()
    {
        // Kartýn baþlangýçta kapalý yüzünü göster
        backFace.enabled = true;
        frontFace.enabled = false;
    }

    // Kartýn açýk yüzünü ayarlayan fonksiyon
    public void SetCardFace(Sprite frontSprite)
    {
        frontFace.sprite = frontSprite;
    }

    public void Flip()
    {
        if (isFlipped)
        {
            // Kartý kapat
            backFace.enabled = true;
            frontFace.enabled = false;
        }
        else
        {
            // Kartý aç
            backFace.enabled = false;
            frontFace.enabled = true;
        }
        isFlipped = !isFlipped; // Çevirme durumunu deðiþtir
    }
}
