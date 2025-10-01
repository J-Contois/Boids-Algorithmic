using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material _bodyMaterial;
    [SerializeField] private Material _featherMaterial;

    private bool leader;
    //private FlockManager manager;                                             // L Use for interaction between flocks
    private BehaviorTypeEnum behaviorType;
    private float _fieldView;
    private float _speed;
    private Vector3 _direction;
    private float _maxVelocity;

    private List<Bird> _neighbourList;

    public void Init(float fieldView, float speed, float maxVelocity)
    {
        _fieldView = fieldView;
        _speed = speed;
        _maxVelocity = maxVelocity;
        _neighbourList = new List<Bird>();
    }

    public void Tick(List<Bird> birdList, float deltaTime)
    {
        NeighbourDetector(birdList);
        Move(deltaTime);
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

        _direction = Barycentre();
    }

    private Vector3 Barycentre()
    {
        if (_neighbourList.Count == 0) return transform.position;

        Vector3 sum = Vector3.zero;

        for (int i = 0; i < _neighbourList.Count; i++)
        {
            sum += _neighbourList[i].transform.position;
        }

        Vector3 barycentre = sum / _neighbourList.Count;

        return barycentre;
    }

    public virtual void Move(float deltaTime)
    {
        Vector3 velocity = _direction * _speed;

        if (velocity.magnitude > _maxVelocity)
        {
            velocity = velocity.normalized * _maxVelocity;
        }

        transform.position += velocity * deltaTime;
    }
}
