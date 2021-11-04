using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundSource;
    public AudioSource effectsSource;

    public AudioClip gameStartClip;
    public AudioClip pacmanDeathClip;
    public AudioClip sirenClip;
    public AudioClip powerPelletClip;
    public AudioClip munchClip;
    public AudioClip eatGhostClip;
    public AudioClip ghostRetreatingClip;

    public AudioClip previousClip { get; private set; }

    public static AudioManager instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    public void PlaySoundEffect(AudioClip clip)
    {
        effectsSource.PlayOneShot(clip);
    }

    public void PlayBackgroundNoise(AudioClip clip)
    {
        if (backgroundSource.clip != clip)
        {
            previousClip = backgroundSource.clip;
        }        
        backgroundSource.clip = clip;
        backgroundSource.Play();
    }
}
