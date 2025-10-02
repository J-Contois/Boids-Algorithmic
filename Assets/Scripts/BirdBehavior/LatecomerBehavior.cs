using UnityEngine;

// Laggard: often isolated and falling behind
// Low cohesion, high separation
public class LatecomerBehavior : BaseBoidBehavior
{
    public override float CohesionWeight => 0.3f;      // Low attraction to the group
    public override float SeparationWeight => 1.5f;    // Strong tendency to deviate
    public override float AlignmentWeight => 0.4f;     // Poor alignment

    public override Color GetColor()
    {
        return Color.blue;
    }
}

