using UnityEngine;
using UnityEngine.UI;
using System;

public class Card : MonoBehaviour
{
    public Image backFace; // Kapal� y�z
    public Image frontFace; // A��k y�z
    private bool isFlipped = false; // Kart�n �evrilip �evrilmedi�ini takip eder
    public Action<Card> OnCardFlipped; // Kart �evrildi�inde tetiklenecek olay

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
            // Kart� kapat
            backFace.enabled = true;
            frontFace.enabled = false;
        }
        else
        {
            // Kart� a�
            backFace.enabled = false;
            frontFace.enabled = true;

            // Kart a��ld���nda olay� tetikle
            OnCardFlipped?.Invoke(this);
        }
        isFlipped = !isFlipped; // �evirme durumunu de�i�tir
    }
}
