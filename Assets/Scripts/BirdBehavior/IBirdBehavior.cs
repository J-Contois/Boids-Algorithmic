using UnityEngine;

namespace BirdBehavior {
    public interface IBirdBehavior {
        public Vector3 CalculateMovement(Bird bird);

        public Color GetColor();
    }
}
