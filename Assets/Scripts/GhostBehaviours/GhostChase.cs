using UnityEngine;

public class GhostChase : GhostBehaviour
{
    public Pacman pacman;
    public Transform chaseTarget;

    private void OnDisable()
    {
        ghost.scatter.Enable();
    }

    public override void Enable(float duration)
    {
        ghost.target = chaseTarget;
        base.Enable(duration);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Node node = collision.GetComponent<Node>();

        if (node != null && enabled && !ghost.frightened.enabled)
        {
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            foreach (Vector2 availableDirection in node.availableDirections)
            {
                Vector3 newPosition = gameObject.transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (ghost.target.position - newPosition).sqrMagnitude;

                if (distance < minDistance && availableDirection != -ghost.movement.direction && node.availableDirections.Count > 1)
                {
                    direction = availableDirection;
                    minDistance = distance;
                }
            }

            ghost.movement.SetDirection(direction);
        }
    }
}
