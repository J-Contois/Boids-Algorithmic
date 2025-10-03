using UnityEngine;

public interface IBirdBehavior
{
    public Vector3 CalculateMovement(Bird bird, float deltaTime);
    
    public Vector3 CalculateBoundaryForce(Bird bird);

    public Color GetColor();
}
