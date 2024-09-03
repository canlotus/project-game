using UnityEngine;
using UnityEngine.UI;
using System;

public class Card : MonoBehaviour
{
    public Image backFace; // Kapalý yüz
    public Image frontFace; // Açýk yüz
    private bool isFlipped = false; // Kartýn çevrilip çevrilmediðini takip eder
    public Action<Card> OnCardFlipped; // Kart çevrildiðinde tetiklenecek olay

    public void SetCardFace(Sprite frontSprite)
    {
        frontFace.sprite = frontSprite;
    }

    public Sprite GetCardSprite()
    {
        return frontFace.sprite;
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

            // Kart açýldýðýnda olayý tetikle
            OnCardFlipped?.Invoke(this);
        }
        isFlipped = !isFlipped; // Çevirme durumunu deðiþtir
    }
}
