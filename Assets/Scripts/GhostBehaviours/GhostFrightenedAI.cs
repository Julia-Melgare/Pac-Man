using System.Collections;
using UnityEngine;

public class GhostFrightenedAI : GhostFrightened
{
    private void Update()
    {
        if (eaten)
        {
            float distanceHome = Vector3.Distance(ghost.transform.position, ghost.home.outside.position);
            Debug.Log(gameObject.name + " - distance to home: " + distanceHome);
            if(distanceHome <= 1.05f)
            {
                Debug.Log(gameObject.name + " arrived home");
                StartCoroutine(EnterTransition());
                Disable();                
            }
        }
    }

    public override void Enable(float duration)
    {
        base.Enable(duration);
        ghost.gameObject.layer = LayerMask.NameToLayer("Ghost");
    }

    public override void Disable()
    {
        ghost.gameObject.layer = LayerMask.NameToLayer("Ghost");
        base.Disable();
    }

    protected override void Eaten()
    {
        base.Eaten();
        ghost.movement.speedMultiplier = 2f;
        ghost.gameObject.layer = LayerMask.NameToLayer("GhostEaten");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled)
        {
            if (!eaten)
            {
                int index = Random.Range(0, node.availableDirections.Count);

                if (node.availableDirections[index] == -ghost.movement.direction && node.availableDirections.Count > 1)
                {
                    index++;
                    if (index >= node.availableDirections.Count)
                    {
                        index = 0;
                    }
                }

                ghost.movement.SetDirection(node.availableDirections[index]);
            }
            else
            {
                Vector2 direction = Vector2.zero;
                float minDistance = float.MaxValue;

                foreach (Vector2 availableDirection in node.availableDirections)
                {
                    Vector3 newPosition = gameObject.transform.position + new Vector3(availableDirection.x, availableDirection.y);
                    float distance = (ghost.home.outside.position - newPosition).sqrMagnitude;

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

    public IEnumerator EnterTransition()
    {
        ghost.movement.SetDirection(Vector2.down, true);
        ghost.movement.enabled = false;

        Vector3 position = transform.position;

        float duration = 0.5f;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            Vector3 newPosition = Vector3.Lerp(position, ghost.home.inside.position, elapsed / duration);
            newPosition.z = position.z;
            ghost.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }
        ghost.home.Enable(1f);
    }

}