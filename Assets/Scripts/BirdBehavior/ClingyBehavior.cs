using UnityEngine;

// Tights: often hits his classmates
public class ClingyBehavior : BaseBoidBehavior
{
    protected override float denseCoeff => 1.5f;
    protected override float looseCoeff => 0.5f;
    protected override float elongatedCoeff => 1.0f;

    public ClingyBehavior(FlockManager flockManager, float dense, float loose, float elongated) : 
        base(flockManager, dense, loose, elongated) { }

    public override Color GetColor()
    {
        return Color.green;
    }
}
