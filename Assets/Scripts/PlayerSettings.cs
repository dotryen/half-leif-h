using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Amnesia.Player {
    public static class PlayerSettings {
        public static float SlopeLimit = 35;
        public static float JumpSlopeLimit = 50;

        public static float RunningSpeed = 10;
        public static float SprintSpeed = 16;
        public static float CrouchSpeed = 4;

        public static float AccelerateSpeed = 10f;
        public static float AirAccelerate = 2f;

        public static float JumpSpeed = 5;
        public static float CrouchLerpSpeed = 10;

        public static readonly LayerMask Ground = LayerMask.GetMask("Default");
    }
}
