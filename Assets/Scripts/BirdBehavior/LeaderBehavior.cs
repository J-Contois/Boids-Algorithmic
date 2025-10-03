using UnityEngine;
using System.Collections.Generic;

// Leader: fly in random direction
public class LeaderBehavior : IBirdBehavior
{
    protected FlockManager manager;

    private float _explorationStrength = 1.5f;
    private Vector3 _currentExplorationDirection;

    public LeaderBehavior(FlockManager newManager)
    {
        manager = newManager;
        _currentExplorationDirection = Random.onUnitSphere;
    }
    
    public Color GetColor()
    {
        return Color.yellow;
    }

    public Vector3 CalculateMovement(Bird bird, float deltaTime)
    {
        Vector3 exploration = CalculateExploration();
        Vector3 boundary = CalculateBoundaryForce(bird);
        
        return exploration + boundary * 2f;
    }
    
    // Force required to remain within the constraint sphere
    public Vector3 CalculateBoundaryForce(Bird bird)
    {
        SphereCollider zone = manager.GetFlightZone();
        
        Vector3 offset = bird.transform.position - (manager.transform.position + zone.center);
        float distance = offset.magnitude;
        
        // If approaching the edge (80% of the radius), force returns to the center
        float threshold = zone.radius * 0.8f;
        
        if (distance > threshold)
        {
            // The closer you get to the edge, the stronger the force becomes.
            float strength = (distance - threshold) / (zone.radius - threshold) * 10f;
            return -offset.normalized * strength * 5f;
        }
        
        return Vector3.zero;
    }

    private Vector3 CalculateExploration()
    {
        float t = Time.time * 0.1f;
        float noiseX = Mathf.PerlinNoise(t, 0f) - 0.5f;
        float noiseY = Mathf.PerlinNoise(t, 100f) - 0.5f;
        float noiseZ = Mathf.PerlinNoise(t, 200f) - 0.5f;

        Vector3 targetDir = new Vector3(noiseX, noiseY, noiseZ).normalized;

        float dot = Vector3.Dot(_currentExplorationDirection, targetDir);
        if (dot < -0.3f) // -1 = completely opposite, 0 = perpendicular
        {
            // If too far off, slightly ‘turn’ targetDir back towards the current direction
            targetDir = Vector3.Slerp(targetDir, _currentExplorationDirection, 0.7f);
        }

        float maxTurnAngle = 30f; // max angle
        float maxRadiansDelta = maxTurnAngle * Mathf.Deg2Rad * Time.deltaTime;

        _currentExplorationDirection = Vector3.RotateTowards(
            _currentExplorationDirection,
            targetDir,
            maxRadiansDelta,
            0f
        );

        return _currentExplorationDirection * _explorationStrength;
    }

}
