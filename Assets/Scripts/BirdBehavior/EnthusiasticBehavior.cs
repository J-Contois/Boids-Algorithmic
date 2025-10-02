using UnityEngine;

// Enthousiaste : comportement erratique
// Cohésion et alignement variables/faibles
public class EnthusiasticBehavior : BaseBoidBehavior
{
    public override float CohesionWeight => 0.8f;      // Average cohesion
    public override float SeparationWeight => 0.5f;    // Weak separation (hence erratism)
    public override float AlignmentWeight => 0.3f;     // Poor alignment (often changes direction)

    public override Color GetColor()
    {
        return Color.red;
    }
    
    public override Vector3 CalculateMovement(Bird bird, float deltaTime)
    {
        Vector3 baseMovement = base.CalculateMovement(bird, deltaTime);
        
        // Adds Perlin noise for erratism
        float noise = Mathf.PerlinNoise(Time.time * 2f, bird.GetInstanceID()) - 0.5f;
        Vector3 randomDir = new Vector3(noise, noise * 0.5f, noise);
        
        return baseMovement + randomDir * 0.5f;
    }
}

