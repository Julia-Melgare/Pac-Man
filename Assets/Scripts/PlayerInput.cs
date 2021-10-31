using UnityEngine;

[RequireComponent(typeof(Movement))]
public class PlayerInput : MonoBehaviour
{

    public Movement movement { get; private set; }

    private void Awake()
    {
        movement = gameObject.GetComponent<Movement>();
    }
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            movement.SetDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            movement.SetDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movement.SetDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            movement.SetDirection(Vector2.right);
        }
    }

    public virtual void ButtonInput(string input)
    {
        switch (input)
        {
            case "up":
                movement.SetDirection(Vector2.up);
                break;
            case "down":
                movement.SetDirection(Vector2.down);
                break;
            case "left":
                movement.SetDirection(Vector2.left);
                break;
            case "right":
                movement.SetDirection(Vector2.right);
                break;
        }
    }
}
