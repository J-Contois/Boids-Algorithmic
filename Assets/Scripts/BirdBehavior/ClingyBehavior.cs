using UnityEngine;

// Collant : percute souvent ses camarades
// Forte cohésion, faible séparation
public class ClingyBehavior : BaseBoidBehavior
{
    public ClingyBehavior(FlockManager flockManager, int dense, int loose, int elongated) : base(flockManager, dense, loose, elongated) { }

    public override Color GetColor()
    {
        return Color.green;
    }
}

