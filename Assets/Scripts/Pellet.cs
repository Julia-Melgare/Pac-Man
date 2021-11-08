using UnityEngine;

public class Pellet : MonoBehaviour
{
    public int points = 10;
    public AudioClip eatPelletClip;

    protected virtual void Eat()
    {
        if (!AudioManager.instance.effectsSource.isPlaying)
        {
            AudioManager.instance.PlaySoundEffect(eatPelletClip);
        }
        FindObjectOfType<GameManager>().PelletEaten(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            Eat();
        }
    }
}
