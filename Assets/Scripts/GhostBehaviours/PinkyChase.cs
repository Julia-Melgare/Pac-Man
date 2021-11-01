using UnityEngine;

public class PinkyChase : GhostChase
{

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 pacmanDirection = pacman.movement.direction;
        Vector3 targetOffset = new Vector3(pacmanDirection.x, pacmanDirection.y) * 4; //Pinky's target is four tiles ahead of Pacman's direction
        if(pacmanDirection == Vector2.up)
        {
            targetOffset += Vector3.left * 4; //If Pacman is going up, Pinky's target is four tiles up and four tiles left of Pacman
        }
        chaseTarget.position = pacman.gameObject.transform.position + targetOffset;
        base.OnTriggerEnter2D(collision);
    }
}
