using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour {
    private GameObject original;
    public new Camera camera;

    public WheelCollider fl;
    public WheelCollider fr;
    public WheelCollider bl;
    public WheelCollider br;

    public float maxTorque;
    public float maxAngle;

    public void FixedUpdate() {
        if (!original) return;

        if (Input.GetKeyDown(KeyCode.E)) {
            Deactivate();
            return;
        }

        float motor = 0f;
        float angle = 0f;

        if (Input.GetKey(KeyCode.W)) motor += -1;
        if (Input.GetKey(KeyCode.S)) motor += 1;
        if (Input.GetKey(KeyCode.A)) angle += -1;
        if (Input.GetKey(KeyCode.D)) angle += 1;

        motor *= maxTorque;
        angle *= maxAngle;

        fl.steerAngle = angle;
        fr.steerAngle = angle;

        bl.motorTorque = motor;
        br.motorTorque = motor;
    }

    public void Activate(GameObject org) {
        original = org;
        org.SetActive(false);
        camera.gameObject.SetActive(true);
    }

    public void Deactivate() {
        camera.gameObject.SetActive(false);
        original.SetActive(true);
        original = null;
    }
}
