using UnityEngine;

// Tights: often hits his classmates
public class ClingyBehavior : BaseBoidBehavior
{
    public ClingyBehavior(FlockManager flockManager, float dense, float loose, float elongated) : 
        base(flockManager, dense, loose, elongated) { }

    public override Color GetColor()
    {
        return Color.green;
    }
}
