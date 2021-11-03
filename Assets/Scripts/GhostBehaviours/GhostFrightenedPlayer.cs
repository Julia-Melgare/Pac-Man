using UnityEngine;

public class GhostFrightenedPlayer : GhostFrightened
{
    public Transform homeSpawn;

    protected override void Eaten()
    {
        base.Eaten();
        Vector3 newPosition = homeSpawn.position;
        newPosition.z = ghost.gameObject.transform.position.z;
        ghost.gameObject.transform.position = newPosition;
        Disable();
    }

}
