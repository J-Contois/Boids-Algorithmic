using UnityEngine;

public interface IBirdBehavior
{
    public Vector3 CalculateMovement(Bird bird, float deltaTime);
    public Color GetColor();
    
    // Weight of Boids forces (vary according to profile)
    public float CohesionWeight { get; }
    public float SeparationWeight { get; }
    public float AlignmentWeight { get; }

}
