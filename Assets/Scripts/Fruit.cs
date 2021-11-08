using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int points = 100;
    public AudioClip eatFruitClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            AudioManager.instance.PlaySoundEffect(eatFruitClip);
            FindObjectOfType<GameManager>().FruitEaten(this);
        }
    }
}
