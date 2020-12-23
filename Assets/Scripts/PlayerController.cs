using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This code contains movement for both local players and non local players
namespace Amnesia.Player {
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour {
        public Rigidbody rb { get; protected set; }
        protected CapsuleCollider col;

        [Header("Movement")]
        public Transform orientation;
        public bool grounded = true;

        [Header("Camera")]
        public new Camera camera;

        // Values needed accross frames
        protected Vector3 floorNormal;

        protected virtual void Awake() {
            rb = GetComponent<Rigidbody>();
            col = GetComponentInChildren<CapsuleCollider>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void FixedUpdate() {
            var input = InputSampler.Sample();
            UpdateCamera(input);
            HandleMovement(input);
        }

        public void HandleMovement(Inputs input) {
            rb.AddForce(Vector3.down * Time.deltaTime * 10);

            // calculate direction
            var delta = Vector3.zero;
            var target = input.movement * GetCurrentSpeed(input);

            // delta calculation
            if (input.movement.x != 0 || grounded) delta.x = (target.x - RelativeVelocity.x) * Acceleration;
            if (input.movement.y != 0 || grounded) delta.z = (target.y - RelativeVelocity.z) * Acceleration;

            rb.AddForce(orientation.TransformDirection(delta), ForceMode.Acceleration);
            
            // Jump
            if (input.jump && grounded) {
                rb.AddForce(Vector3.up * PlayerSettings.JumpSpeed * 0.50f, ForceMode.VelocityChange);
                rb.AddForce(floorNormal.normalized * PlayerSettings.JumpSpeed * 0.50f, ForceMode.VelocityChange);
                grounded = false;
            }
            Debug.DrawRay(transform.position, rb.velocity, Color.blue, Time.deltaTime);
        }

        public void UpdateCamera(Inputs input) {
            orientation.localEulerAngles = new Vector3(0, input.orientation);
            camera.transform.localEulerAngles = new Vector3(-input.viewAngle, 0);

            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit)) {
                Debug.DrawLine(camera.transform.position, hit.point, Color.red, Time.deltaTime);
            }
        }

        #region Ground Check

        // Thanks Dani.

        protected bool IsFloor(Vector3 v, float slope) {
            float angle = Vector3.Angle(Vector3.up, v);
            return angle < slope;
        }

        bool cancellingGrounded;

        private void OnCollisionStay(Collision other) {
            int layer = other.gameObject.layer;
            if (PlayerSettings.Ground != (PlayerSettings.Ground | (1 << layer))) return;

            for (int i = 0; i < other.contactCount; i++) {
                Vector3 normal = other.GetContact(i).normal;
                if (IsFloor(normal, PlayerSettings.SlopeLimit)) {
                    floorNormal = normal;
                    rb.useGravity = false;
                    grounded = true;

                    cancellingGrounded = false;
                    CancelInvoke(nameof(StopGrounded));
                }
            }

            float delay = 3f;
            if (!cancellingGrounded) {
                cancellingGrounded = true;
                Invoke(nameof(StopGrounded), Time.deltaTime * delay);
            }
        }

        private void StopGrounded() {
            grounded = false;
            rb.useGravity = true;
        }

        #endregion

        #region Utilities

        // all of these are relative to view
        // z: how much you are moving forward
        // x: how much you are moving to the right
        public Vector3 RelativeVelocity { get { return orientation.InverseTransformDirection(rb.velocity); } }

        public float Acceleration { get { return grounded ? PlayerSettings.AccelerateSpeed : PlayerSettings.AirAccelerate; } }

        public float GetCurrentSpeed(Inputs input) {
            return input.sprint ? PlayerSettings.SprintSpeed : PlayerSettings.RunningSpeed;
        }

        public float ViewAngleEasing(float normalizedTime) {
            var x = Mathf.InverseLerp(0, 90, Mathf.Abs(normalizedTime));
            var value = 1 - Mathf.Pow(1 - x, 4);

            return value * normalizedTime;
        }

        #endregion
    }
}
