using UnityEngine;

public class PacmanPlayerInput : PlayerInput
{
    protected override void Update()
    {
        base.Update();
        float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
        gameObject.transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }
}
