using UnityEngine;

public class Passage : MonoBehaviour
{
    public Transform connection;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 position = collision.transform.position;
        position.x = connection.position.x;
        position.y = connection.position.y;
        collision.transform.position = position;
    }
}
