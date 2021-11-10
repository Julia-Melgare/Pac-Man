using UnityEngine;

public class PassageEntry : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ghost"))
        {
            Ghost ghost = collision.gameObject.GetComponent<Ghost>();
            if (ghost.frightened.enabled)
            {
                ghost.movement.speedMultiplier = (LevelManager.instance.frightGhostsSpeed / LevelManager.instance.ghostsSpeed) / 2;
            }
            else
            {
                ghost.movement.speedMultiplier = LevelManager.instance.ghostsTunnelSpeed / LevelManager.instance.ghostsSpeed;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ghost"))
        {
            Ghost ghost = collision.gameObject.GetComponent<Ghost>();
            if (ghost.frightened.enabled)
            {
                ghost.movement.speedMultiplier = LevelManager.instance.frightGhostsSpeed / LevelManager.instance.ghostsSpeed;
            }
            else
            {
                ghost.movement.speedMultiplier = 1f;
            }
        }
    }
}
