using UnityEngine;

// Tights: often hits his classmates
namespace BirdBehavior {
    public class ClingyBehavior : BaseBoidBehavior {
        protected override float DenseCoeff => 2.5f;
        protected override float LooseCoeff => 0.5f;
        protected override float ElongatedCoeff => 0.6f;

        public ClingyBehavior(FlockManager flockManager, float dense, float loose, float elongated) : 
            base(flockManager, dense, loose, elongated) { }

        public override Color GetColor() {
            return Color.green;
        }
    }
}
