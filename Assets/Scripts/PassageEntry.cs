using UnityEngine;

public class PassageEntry : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ghost") && !collision.gameObject.GetComponent<Ghost>().frightened.enabled)
        {
            collision.gameObject.GetComponent<Movement>().speedMultiplier = LevelManager.instance.ghostsTunnelSpeed / LevelManager.instance.ghostsSpeed;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ghost") && !collision.gameObject.GetComponent<Ghost>().frightened.enabled)
        {
            collision.gameObject.GetComponent<Movement>().speedMultiplier = 1f;
        }
    }
}
