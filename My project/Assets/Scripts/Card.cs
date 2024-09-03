using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Image cardImage; // Kart�n y�z�n� g�sterecek olan Image komponenti
    public Sprite backFaceSprite; // Kapal� y�z i�in kullan�lacak sprite
    public Sprite frontFaceSprite; // A��k y�z i�in kullan�lacak sprite

    private bool isFlipped = false; // Kart�n �evrilip �evrilmedi�ini takip eder

    void Start()
    {
        // Ba�lang��ta kapal� y�z� g�ster
        cardImage.sprite = backFaceSprite;
    }

    public void SetCardFace(Sprite frontSprite)
    {
        // A��k y�z sprite'�n� ayarla
        frontFaceSprite = frontSprite;
    }

    public void Flip()
    {
        if (isFlipped)
        {
            // Kart� kapat
            cardImage.sprite = backFaceSprite;
        }
        else
        {
            // Kart� a�
            cardImage.sprite = frontFaceSprite;
        }
        isFlipped = !isFlipped; // �evirme durumunu de�i�tir
    }
}
