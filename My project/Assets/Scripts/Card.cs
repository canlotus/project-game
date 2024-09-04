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

    public bool IsFlipped()
    {
        return isFlipped;
    }

    public void Flip()
    {
        if (isMatched || isFlipped) return; // Eþleþmiþ veya zaten açýk olan kartlarý çevirme

        isFlipped = true;
        backFace.enabled = false;
        frontFace.enabled = true;

        if (OnCardFlipped != null)
        {
            OnCardFlipped(this);
        }
    }

    public void SetMatched()
    {
        isMatched = true;
    }

    public void Close()
    {
        isFlipped = false;
        backFace.enabled = true;
        frontFace.enabled = false;
    }

    // Týklanabilirliði kontrol eden metod
    public void SetInteractable(bool state)
    {
        GetComponent<Button>().interactable = state;
    }
}
