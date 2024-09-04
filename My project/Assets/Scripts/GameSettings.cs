using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public static int numberOfCards = 8; // Varsay�lan easy modda 8 kart
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    public Button playButton;

    private void Start()
    {
        // Ba�lang��ta easy se�ili olacak
        SelectEasy();

        // Butonlara click event'leri ekle
        easyButton.onClick.AddListener(SelectEasy);
        mediumButton.onClick.AddListener(SelectMedium);
        hardButton.onClick.AddListener(SelectHard);
        playButton.onClick.AddListener(StartGame);
    }

    // Easy modu se�
    public void SelectEasy()
    {
        numberOfCards = 8;
        SetButtonColors(easyButton, mediumButton, hardButton);
    }

    // Medium modu se�
    public void SelectMedium()
    {
        numberOfCards = 12;
        SetButtonColors(mediumButton, easyButton, hardButton);
    }

    // Hard modu se�
    public void SelectHard()
    {
        numberOfCards = 16;
        SetButtonColors(hardButton, easyButton, mediumButton);
    }

    // Oyunu ba�lat
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); // GameScene sahnesine ge�
    }

    // Se�ilen butonun rengini de�i�tir
    private void SetButtonColors(Button selected, Button unselected1, Button unselected2)
    {
        Color selectedColor = new Color(0.5f, 0.8f, 0.5f); // Ye�il
        Color unselectedColor = new Color(0.8f, 0.8f, 0.8f); // Gri

        selected.GetComponent<Image>().color = selectedColor;
        unselected1.GetComponent<Image>().color = unselectedColor;
        unselected2.GetComponent<Image>().color = unselectedColor;
    }
}
