using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Singleton referans�

    public AudioSource audioSource; // Ses �almak i�in kullan�lan AudioSource
    public AudioClip buttonClickSound; // Butona t�klama sesi
    public AudioClip matchSound; // E�le�me sesi
    public AudioClip noMatchSound; // E�le�meme sesi
    public AudioClip gameEndSound; // Oyun biti� sesi

    void Awake()
    {
        // Singleton pattern, sadece bir tane AudioManager olmas�n� sa�lar
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Scene de�i�se bile yok olmas�n
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Belirtilen sesi �al
    public void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // Buton t�klama sesini �al
    public void PlayButtonClickSound()
    {
        PlaySound(buttonClickSound);
    }

    // E�le�me sesini �al
    public void PlayMatchSound()
    {
        PlaySound(matchSound);
    }

    // E�le�meme sesini �al
    public void PlayNoMatchSound()
    {
        PlaySound(noMatchSound);
    }

    // Oyun biti� sesini �al
    public void PlayGameEndSound()
    {
        PlaySound(gameEndSound);
    }
}
