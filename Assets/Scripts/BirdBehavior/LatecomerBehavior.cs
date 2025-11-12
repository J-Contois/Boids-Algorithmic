using UnityEngine;

// Laggard: often isolated and falling behind
namespace BirdBehavior {
    public class LatecomerBehavior : BaseBoidBehavior {
        protected override float DenseCoeff => 0.25f;
        protected override float LooseCoeff => 2.5f;
        protected override float ElongatedCoeff => 0.5f;

        public LatecomerBehavior(FlockManager flockManager, float dense, float loose, float elongated) : 
            base(flockManager, dense, loose, elongated) { }

        public override Color GetColor() {
            return Color.blue;
        }

        public override Vector3 CalculateMovement(Bird bird) {
            Vector3 baseMovement = base.CalculateMovement(bird);
            return baseMovement * 0.3f;
        }
    }
}

