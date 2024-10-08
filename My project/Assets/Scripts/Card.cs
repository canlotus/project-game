using UnityEngine;
using UnityEngine.UI;
using System;

public class Card : MonoBehaviour
{
    public Image backFace; // Kapal� y�z
    public Image frontFace; // A��k y�z
    public Action<Card> OnCardFlipped; // Kart �evrildi�inde tetiklenecek olay

    private bool isFlipped = false; // Kart�n �evrilip �evrilmedi�ini takip eder
    private bool isMatched = false; // Kart�n e�le�ip e�le�medi�ini takip eder

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
        if (isMatched || isFlipped) return; // E�le�mi� veya zaten a��k olan kartlar� �evirme

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

    // T�klanabilirli�i kontrol eden metod
    public void SetInteractable(bool state)
    {
        GetComponent<Button>().interactable = state;
    }
}
