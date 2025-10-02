using UnityEngine;

// Laggard: often isolated and falling behind
// Low cohesion, high separation
public class LatecomerBehavior : BaseBoidBehavior
{
    public LatecomerBehavior(FlockManager flockManager, int dense, int loose, int elongated) : base(flockManager, dense, loose, elongated) { }

    public override Color GetColor()
    {
        return Color.blue;
    }
}

