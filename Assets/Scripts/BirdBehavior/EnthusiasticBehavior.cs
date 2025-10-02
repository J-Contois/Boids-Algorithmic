using UnityEngine;

// Enthousiaste : comportement erratique
public class EnthusiasticBehavior : BaseBoidBehavior
{
    public EnthusiasticBehavior(FlockManager flockManager, float dense, float loose, float elongated) : 
        base(flockManager, dense, loose, elongated) { }

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

