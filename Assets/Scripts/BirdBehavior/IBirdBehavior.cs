using UnityEngine;

public interface IBirdBehavior
{
    public Vector3 CalculateMovement(Bird bird, float deltaTime);
    public Color GetColor();
    public void SetManager(FlockManager newManager);
}
