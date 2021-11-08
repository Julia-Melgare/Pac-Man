using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration = 8f;

    protected override void Eat()
    {
        AudioManager.instance.PlayBackgroundNoise(eatPelletClip);
        FindObjectOfType<GameManager>().PowerPelletEaten(this);
    }
}
