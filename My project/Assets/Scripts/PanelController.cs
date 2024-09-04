using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PanelController : MonoBehaviour
{
    public GameObject pausePanel; // Panel referansý
    public Button continueButton; // Devam butonu
    public Button mainMenuButton; // Ana menü butonu

    void Start()
    {
        // Baþlangýçta paneli kapalý tut
        pausePanel.SetActive(false);

        // Butonlara click event'leri ekle
        continueButton.onClick.AddListener(HidePanel);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    // Paneli göster
    public void ShowPanel()
    {
        pausePanel.SetActive(true);
    }

    // Paneli gizle ve oyuna devam et
    public void HidePanel()
    {
        pausePanel.SetActive(false);
    }

    // Ana menüye dön
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }
}
