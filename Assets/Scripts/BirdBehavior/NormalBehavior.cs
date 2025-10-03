using UnityEngine;

// Tights: often hits his classmates
public class NormalBehavior : BaseBoidBehavior
{
    public NormalBehavior(FlockManager flockManager, float dense, float loose, float elongated) :
        base(flockManager, dense, loose, elongated)
    { }
}
