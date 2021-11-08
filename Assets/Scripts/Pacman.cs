using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman : MonoBehaviour
{
    public AnimatedSprite deathAnimation;
    public SpriteRenderer spriteRenderer { get; private set; }
    public new Collider2D collider { get; private set; }
    public Movement movement { get; private set; }
    public AudioClip pacmanDeathClip;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        movement = gameObject.GetComponent<Movement>();
    }

    public void ResetState()
    {
        enabled = true;
        spriteRenderer.enabled = true;
        collider.enabled = true;
        deathAnimation.enabled = false;
        deathAnimation.spriteRenderer.enabled = false;
        gameObject.SetActive(true);
        movement.ResetState();
    }

    public void DeathAnimation()
    {
        enabled = false;
        spriteRenderer.enabled = false;
        collider.enabled = false;
        movement.enabled = false;
        deathAnimation.enabled = true;
        deathAnimation.spriteRenderer.enabled = true;
        deathAnimation.Restart();
    }
}
