using UnityEngine;

// Laggard: often isolated and falling behind
public class LatecomerBehavior : BaseBoidBehavior
{
    protected new float denseCoeff = 0.5f;
    protected new float looseCoeff = 1.5f;
    protected new float elongatedCoeff = 0.8f;

    public LatecomerBehavior(FlockManager flockManager, float dense, float loose, float elongated) : 
        base(flockManager, dense, loose, elongated) { }

    public override Color GetColor()
    {
        return Color.blue;
    }
}

