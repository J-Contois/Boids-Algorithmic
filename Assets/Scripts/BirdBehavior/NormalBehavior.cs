using UnityEngine;

// Normal: act without any changed behavior
public class NormalBehavior : BaseBoidBehavior
{
    public NormalBehavior(FlockManager flockManager, float dense, float loose, float elongated) :
        base(flockManager, dense, loose, elongated)
    { }
}
