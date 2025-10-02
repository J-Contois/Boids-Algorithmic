using UnityEngine;
using System.Collections.Generic;

// Leader: follows invisible points in space
public class LeaderBehavior : IBirdBehavior
{
    public override float CohesionWeight => 0.2f;
    public override float SeparationWeight => 0.8f;
    public override float AlignmentWeight => 0.1f;

    private float _explorationStrength = 1.5f;
    private float _directionChangeInterval = 3f;
    private Vector3 _currentExplorationDirection;
    private float _lastDirectionChangeTime;

    public LeaderBehavior(List<Vector3> waypoints)
    {
        _currentExplorationDirection = Random.onUnitSphere;
        _lastDirectionChangeTime = Time.time;
    }
    
    public Color GetColor()
    {
        return Color.yellow;
    }

    public override Vector3 CalculateMovement(Bird bird, FlockManager manager, float deltaTime)
    {
        Vector3 boidForces = base.CalculateMovement(bird, manager, deltaTime);
        Vector3 exploration = CalculateExploration(bird);
        
        return boidForces * 0.3f + exploration * 0.7f;
    }
    private Vector3 CalculateExploration(Bird bird)
    {
        if (Time.time - _lastDirectionChangeTime > _directionChangeInterval)
        {
            float noiseX = Mathf.PerlinNoise(Time.time * 0.5f, 0f) - 0.5f;
            float noiseY = Mathf.PerlinNoise(Time.time * 0.5f, 100f) - 0.5f;
            float noiseZ = Mathf.PerlinNoise(Time.time * 0.5f, 200f) - 0.5f;
            
            _currentExplorationDirection = new Vector3(noiseX, noiseY, noiseZ).normalized;
            _lastDirectionChangeTime = Time.time;
            _directionChangeInterval = Random.Range(2f, 5f);
        }

        return _currentExplorationDirection * _explorationStrength;
    }
}

