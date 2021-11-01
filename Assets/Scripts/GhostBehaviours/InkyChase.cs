using UnityEngine;

public class InkyChase : GhostChase
{
    public Transform blinky;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 pacmanDirection = pacman.movement.direction;
        Vector3 targetOffset = new Vector3(pacmanDirection.x, pacmanDirection.y) * 2; //Inky looks two tiles ahead of Pacman's direction
        if (pacmanDirection == Vector2.up)
        {
            targetOffset += Vector3.left * 2; //If Pacman is going up, Inky looks two tiles up and two tiles left of Pacman
        }
        Vector3 lookAhead = pacman.gameObject.transform.position + targetOffset;
        chaseTarget.position = lookAhead + (lookAhead - blinky.position); //Inky's target is the extended vector between Blinky's position and two tiles ahead of Pacman's direction
        base.OnTriggerEnter2D(collision);
    }
}
