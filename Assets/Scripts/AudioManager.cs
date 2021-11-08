using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundSource;
    public AudioSource effectsSource;

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
        backgroundSource.clip = clip;
        backgroundSource.Play();
    }
}
