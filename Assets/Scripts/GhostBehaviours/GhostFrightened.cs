using UnityEngine;

public abstract class GhostFrightened : GhostBehaviour
{
    public SpriteRenderer body;
    public SpriteRenderer eyes;
    public SpriteRenderer blue;
    public SpriteRenderer flash;

    public bool eaten { get; protected set; }

    public override void Enable(float duration)
    {
        base.Enable(duration);

        body.enabled = false;
        eyes.enabled = false;
        blue.enabled = true;
        flash.enabled = false;
        eaten = false;

        Invoke(nameof(Flash), duration / 2.0f);
    }

    public override void Disable()
    {
        eaten = false;
        body.enabled = true;
        eyes.enabled = true;
        blue.enabled = false;
        flash.enabled = false;
        base.Disable();
    }

    protected void Flash()
    {
        if (!eaten)
        {
            blue.enabled = false;
            flash.enabled = true;
            flash.GetComponent<AnimatedSprite>().Restart();
        }
    }

    protected virtual void Eaten()
    {
        CancelInvoke();
        eaten = true;
        body.enabled = false;
        eyes.enabled = true;
        blue.enabled = false;
        flash.enabled = false;
    }

    protected void OnEnable()
    {
        blue.GetComponent<AnimatedSprite>().Restart();
        ghost.movement.speedMultiplier = LevelManager.instance.frightGhostsSpeed / LevelManager.instance.ghostsSpeed;
        eaten = false;
    }

    protected void OnDisable()
    {
        ghost.movement.speedMultiplier = 1.0f;
        eaten = false;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (enabled && !eaten)
            {
                Eaten();
            }
        }
    }


}
