using UnityEngine;

// Laggard: often isolated and falling behind
public class LatecomerBehavior : BaseBoidBehavior
{
    public LatecomerBehavior(FlockManager flockManager, float dense, float loose, float elongated) : 
        base(flockManager, dense, loose, elongated) { }

    public override Color GetColor()
    {
        return Color.blue;
    }
}

