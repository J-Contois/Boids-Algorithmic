using UnityEngine;

// Collant : percute souvent ses camarades
// Forte cohésion, faible séparation
public class ClingyBehavior : BaseBoidBehavior
{
    public override float CohesionWeight => 1.8f;      // Très forte attraction vers le groupe
    public override float SeparationWeight => 0.2f;    // Très faible séparation (d'où les collisions)
    public override float AlignmentWeight => 1.0f;     // Fort alignement

    public ClingyBehavior(FlockManager flockManager, int dense, int loose, int elongated) : base(flockManager, dense, loose, elongated) { }

    public override Color GetColor()
    {
        return Color.green;
    }
}

