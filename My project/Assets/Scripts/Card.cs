using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Image backFace; // Kapal� y�z� g�sterecek Image
    public Image frontFace; // A��k y�z� g�sterecek Image
    private bool isFlipped = false; // Kart�n �evrilip �evrilmedi�ini takip eder

    void Start()
    {
        // Kart�n ba�lang��ta kapal� y�z�n� g�ster
        backFace.enabled = true;
        frontFace.enabled = false;
    }

    // Kart�n a��k y�z�n� ayarlayan fonksiyon
    public void SetCardFace(Sprite frontSprite)
    {
        frontFace.sprite = frontSprite;
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
        }
        isFlipped = !isFlipped; // �evirme durumunu de�i�tir
    }
}
