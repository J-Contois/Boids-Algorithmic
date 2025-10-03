using UnityEngine;

// Laggard: often isolated and falling behind
public class LatecomerBehavior : BaseBoidBehavior
{
    protected override float denseCoeff => 0.25f;
    protected override float looseCoeff => 2.5f;
    protected override float elongatedCoeff => 0.5f;

    public LatecomerBehavior(FlockManager flockManager, float dense, float loose, float elongated) : 
        base(flockManager, dense, loose, elongated) { }

    public override Color GetColor()
    {
        return Color.blue;
    }

    public override Vector3 CalculateMovement(Bird bird, float deltaTime)
    {
        Vector3 baseMovement = base.CalculateMovement(bird, deltaTime);

        return baseMovement * 0.3f;
    }
}

