using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntidadesJogo
{
    public class Ball : MonoBehaviour
    {
        public enum BallState { Prepare, Playing, Died, Finish }
        public BallState ballState;
    }
}

