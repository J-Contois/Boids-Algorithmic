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
        Vector3 exploration = CalculateExploration(bird);
        
        return Vector3.one * 0.3f + exploration * 0.7f;
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

