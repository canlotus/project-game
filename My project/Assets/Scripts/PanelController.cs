using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PanelController : MonoBehaviour
{
    public GameObject pausePanel; // Panel referans�
    public Button continueButton; // Devam butonu
    public Button mainMenuButton; // Ana men� butonu

    void Start()
    {
        // Ba�lang��ta paneli kapal� tut
        pausePanel.SetActive(false);

        // Butonlara click event'leri ekle
        continueButton.onClick.AddListener(HidePanel);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    // Paneli g�ster
    public void ShowPanel()
    {
        pausePanel.SetActive(true);
    }

    // Paneli gizle ve oyuna devam et
    public void HidePanel()
    {
        pausePanel.SetActive(false);
    }

    // Ana men�ye d�n
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }
}
