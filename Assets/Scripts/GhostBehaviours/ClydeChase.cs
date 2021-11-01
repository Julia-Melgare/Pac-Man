using UnityEngine;

public class ClydeChase : GhostChase
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Node node = collision.GetComponent<Node>();

        if (node != null && enabled && !ghost.frightened.enabled)
        {
            float distancePacman = Vector3.Distance(ghost.transform.position, pacman.transform.position);
            Vector2 direction = Vector2.zero;
            if (distancePacman > 8)
            {                
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
            }
            else
            {
                float maxDistance = float.MinValue;

                foreach (Vector2 availableDirection in node.availableDirections)
                {
                    Vector3 newPosition = gameObject.transform.position + new Vector3(availableDirection.x, availableDirection.y);
                    float distance = (ghost.target.position - newPosition).sqrMagnitude;

                    if (distance > maxDistance && availableDirection != -ghost.movement.direction && node.availableDirections.Count > 1)
                    {
                        direction = availableDirection;
                        maxDistance = distance;
                    }
                }
            }   
            ghost.movement.SetDirection(direction);
        }
    }

}
