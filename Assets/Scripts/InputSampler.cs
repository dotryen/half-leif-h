using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amnesia.Player;
using Amnesia.Settings;

public static class InputSampler {

    public static float viewAngle;
    public static float orient;

    public static Inputs Sample() {
        Inputs input = new Inputs();

        input.movement = new Vector2((Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0), (Input.GetKey(KeyCode.S) ? -1 : 0) + (Input.GetKey(KeyCode.W) ? 1 : 0));
        if (input.movement.magnitude > 1) input.movement /= input.movement.magnitude;
        
        input.sprint = Input.GetKey(KeyCode.LeftShift);
        input.jump = Input.GetKey(KeyCode.Space);
        
        orient += Input.GetAxis("Mouse X") * Settings.MouseSensitivity;
        viewAngle += Input.GetAxis("Mouse Y") * Settings.MouseSensitivity;
        orient = RepeatOrient(orient);
        viewAngle = Mathf.Clamp(viewAngle, -90, 90);
        input.orientation = orient;
        input.viewAngle = viewAngle;

        return input;
    }

    public static float RepeatOrient(float t) {
        return Mathf.Repeat(t + 180, 360) - 180;
    }
}
