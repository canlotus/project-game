using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    public Button playButton;

    private string selectedMode = "Easy";

    void Start()
    {
        easyButton.onClick.AddListener(() => SelectMode("Easy"));
        mediumButton.onClick.AddListener(() => SelectMode("Medium"));
        hardButton.onClick.AddListener(() => SelectMode("Hard"));
        playButton.onClick.AddListener(StartGame);
    }

    void SelectMode(string mode)
    {
        selectedMode = mode;

        easyButton.image.color = mode == "Easy" ? Color.green : Color.white;
        mediumButton.image.color = mode == "Medium" ? Color.green : Color.white;
        hardButton.image.color = mode == "Hard" ? Color.green : Color.white;
    }

    void StartGame()
    {
        PlayerPrefs.SetString("SelectedMode", selectedMode);
        SceneManager.LoadScene("GameScene");
    }
}
