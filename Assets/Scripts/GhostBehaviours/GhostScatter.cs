using UnityEngine;

public class GhostScatter : GhostBehaviour
{
    private void OnDisable()
    {
        ghost.chase.Enable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Node node = collision.GetComponent<Node>();

        if(node != null && enabled && !ghost.frightened.enabled)
        {
            //TODO: change ghost target to respective scatter target
            //TODO: change to calculate direction based on target
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
    }
}
