using UnityEngine;

// Enthusiastic: erratic behaviour
public class EnthusiasticBehavior : BaseBoidBehavior
{
    protected override float denseCoeff => 0.2f;
    protected override float looseCoeff => 0.75f;
    protected override float elongatedCoeff => 2.5f;

    public EnthusiasticBehavior(FlockManager flockManager, float dense, float loose, float elongated) : 
        base(flockManager, dense, loose, elongated) { }

    public override Color GetColor()
    {
        return Color.red;
    }
    
    public override Vector3 CalculateMovement(Bird bird, float deltaTime)
    {
        Vector3 baseMovement = base.CalculateMovement(bird, deltaTime);

        // Adds Perlin noise for erratism (more fluid than random)
        float noise = Mathf.PerlinNoise(Time.time * 2f, bird.GetInstanceID()) - 0.5f;
        Vector3 randomDir = new Vector3(noise, noise * 0.5f, noise);
        
        return baseMovement + randomDir * 0.5f;
    }
}
