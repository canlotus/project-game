using UnityEngine;
using UnityEngine.UI;
using System;

public class Card : MonoBehaviour
{
    public Image backFace; // Kapalý yüz
    public Image frontFace; // Açýk yüz
    public Action<Card> OnCardFlipped; // Kart çevrildiðinde tetiklenecek olay

    private bool isFlipped = false; // Kartýn çevrilip çevrilmediðini takip eder
    private bool isMatched = false; // Kartýn eþleþip eþleþmediðini takip eder

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
        if (isMatched || isFlipped) return; // Eþleþmiþ veya zaten açýk olan kartlarý çevirme

        isFlipped = true;
        backFace.enabled = false;
        frontFace.enabled = true;

        if (OnCardFlipped != null)
        {
            OnCardFlipped(this); // Kart çevrildiðinde GameController'da bu olay tetiklenir
        }
    }

    public void SetMatched()
    {
        isMatched = true; // Kart eþleþti olarak iþaretlenir
    }

    public void Close()
    {
        isFlipped = false;
        backFace.enabled = true;
        frontFace.enabled = false;
    }
}
