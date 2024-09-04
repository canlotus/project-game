using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Singleton referansý

    public AudioSource audioSource; // Ses çalmak için kullanýlan AudioSource
    public AudioClip buttonClickSound; // Butona týklama sesi
    public AudioClip matchSound; // Eþleþme sesi
    public AudioClip noMatchSound; // Eþleþmeme sesi
    public AudioClip gameEndSound; // Oyun bitiþ sesi

    void Awake()
    {
        // Singleton pattern, sadece bir tane AudioManager olmasýný saðlar
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Scene deðiþse bile yok olmasýn
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Belirtilen sesi çal
    public void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // Buton týklama sesini çal
    public void PlayButtonClickSound()
    {
        PlaySound(buttonClickSound);
    }

    // Eþleþme sesini çal
    public void PlayMatchSound()
    {
        PlaySound(matchSound);
    }

    // Eþleþmeme sesini çal
    public void PlayNoMatchSound()
    {
        PlaySound(noMatchSound);
    }

    // Oyun bitiþ sesini çal
    public void PlayGameEndSound()
    {
        PlaySound(gameEndSound);
    }
}
