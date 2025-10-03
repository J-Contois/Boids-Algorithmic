using UnityEngine;
using System.Collections.Generic;

// Leader: follows invisible points in space
public class LeaderBehavior : IBirdBehavior
{
    protected FlockManager manager;

    private float _explorationStrength = 1.5f;
    private float _directionChangeInterval = 3f;
    private Vector3 _currentExplorationDirection;
    private float _lastDirectionChangeTime;

    public LeaderBehavior(FlockManager newManager)
    {
        manager = newManager;
        _currentExplorationDirection = Random.onUnitSphere;
        _lastDirectionChangeTime = Time.time;
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

    private Vector3 CalculateExploration() {
        if (Time.time - _lastDirectionChangeTime > _directionChangeInterval) {
            _currentExplorationDirection = Random.onUnitSphere;
            _lastDirectionChangeTime = Time.time;
            _directionChangeInterval = Random.Range(2f, 5f);
        }
        return _currentExplorationDirection * _explorationStrength;
    }
}

