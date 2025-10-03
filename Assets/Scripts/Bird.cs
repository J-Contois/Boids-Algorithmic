using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{  
    private IBirdBehavior _behavior;

    private float _fieldView;
    private float _speed;
    private Vector3 _velocity;
    private float _maxVelocity;
    private Vector3 _oldVelocity;

    private List<Bird> _neighbourList;

    public float FieldView => _fieldView;
    public List<Bird> NeighbourList => _neighbourList;
    public Vector3 Velocity => _velocity;

    public void Init(IBirdBehavior behavior, float fieldView, float speed, float maxVelocity)
    {
        _behavior = behavior;

        _fieldView = fieldView;
        _speed = speed;
        _maxVelocity = maxVelocity;

        _neighbourList = new List<Bird>();
        _velocity = Random.onUnitSphere * _speed;
        _oldVelocity = Vector3.zero;

        SetColor(_behavior.GetColor());
    }


    public void Tick(List<Bird> birdList, float agentMinSpeed,  float deltaTime)
    {
        NeighbourDetector(birdList);
        
        // Behaviour calculates direction
        Vector3 steeringForce = _behavior.CalculateMovement(this, deltaTime);
        
        // Velocity update
        _velocity += steeringForce;
        
        // Application of drag (air friction) to stabilise movement
        _velocity *= 0.98f;
        
        // Limits maximum velocity
        if (_velocity.magnitude > _maxVelocity)
        {
            _velocity = _velocity.normalized * _maxVelocity;
        }
        
        float currentSpeed = _velocity.magnitude;
        // Limits minimum velocity
        _velocity = Vector3.ClampMagnitude(_velocity, _maxVelocity);

        if (_velocity.magnitude < agentMinSpeed)
            _velocity = _velocity.normalized * agentMinSpeed;

        float maxRadiansDelta = 90f * Mathf.Deg2Rad * deltaTime;
        _velocity = Vector3.RotateTowards(_oldVelocity, _velocity, maxRadiansDelta, float.MaxValue);
        _oldVelocity = _velocity;
        
        // Move the bird
        transform.position += _velocity * deltaTime;
        
        // Directs the bird in the direction of movement
        if (_velocity.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(_velocity);
        }
    }

    private void SetColor(Color color)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>(true);

        foreach (Renderer rend in renderers)
        {
            Material[] mats = rend.materials;

            for (int i = 0; i < mats.Length; i++)
            {
                mats[i].color = color;
            }

            rend.materials = mats;
        }
    }

    // Look if other bird are in the radius of bird and adding them in neighbour list
    public void NeighbourDetector(List<Bird> birdList)
    {
        if (birdList.Count == 0) return;

        _neighbourList.Clear();
        for (int i = 0; i < birdList.Count; i++)
        {
            if (birdList[i] != this)
            {
                if (Vector3.Distance(transform.position, birdList[i].transform.position) <= _fieldView) {
                    _neighbourList.Add(birdList[i]);
                }
            }
        }
    }
}
