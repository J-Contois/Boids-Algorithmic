using System.Collections.Generic;
using System.Net;
using Unity.Mathematics;
using UnityEngine;

public class Bird : MonoBehaviour
{
    //[Header("Materials")]
    //[SerializeField] private Material _bodyMaterial;
    //[SerializeField] private Material _featherMaterial;
    
    private IBirdBehavior _behavior;

    private float _fieldView;
    private float _speed;
    private Vector3 _direction;
    private Vector3 _velocity;
    private float _maxVelocity;

    private int _denseWeight;
    private int _looseWeight;
    private int _elongatedWeight;

    private List<Bird> _neighbourList;
    
    public List<Bird> NeighbourList => _neighbourList;
    public Vector3 Velocity => _velocity;
    public float Speed => _speed;


    public void Init(IBirdBehavior behavior, float fieldView, 
        float speed, float maxVelocity, int dense, int loose, int elongated)
    {
        _behavior = behavior;

        _fieldView = fieldView;
        _speed = speed;
        _maxVelocity = maxVelocity;

        _denseWeight = dense;
        _looseWeight = loose;
        _elongatedWeight = elongated;

        _neighbourList = new List<Bird>();
        _velocity = Vector3.zero;

        SetColor(_behavior.GetColor());
    }


    public void Tick(List<Bird> birdList, float deltaTime)
    {
        NeighbourDetector(birdList);
        
        // Behaviour calculates direction
        Vector3 direction = _behavior.CalculateMovement(this, deltaTime);
        
        // Applies the speed
        _velocity = direction.normalized * _speed;
        
        // Limits maximum velocity
        if (_velocity.magnitude > _maxVelocity)
        {
            _velocity = _velocity.normalized * _maxVelocity;
        }

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
                /*if (mats[i] == _bodyMaterial || mats[i] == _featherMaterial)
                {
                    mats[i].color = color;
                }*/
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
