using UnityEngine;

public class PacmanPlayerInput : PlayerInput
{

    protected override void Update()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE
        base.Update();
        #endif
        float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
        gameObject.transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    #if UNITY_EDITOR || UNITY_WEBGL
    public override void ButtonInput(string input)
    {
        base.ButtonInput(input);
    }
    #endif
}
