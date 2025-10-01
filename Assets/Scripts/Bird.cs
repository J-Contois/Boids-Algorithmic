using System;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour, IEnthusiastic, ILatecomer, IClingy
{
    [Header("Materials")]
    [SerializeField] private Material _bodyMaterial;
    [SerializeField] private Material _featherMaterial;

    private bool leader;
    private FlockManager _manager;
    private BehaviorTypeEnum behaviorType;
    private float _fieldView;
    private float _speed;
    private Vector3 _direction;
    private float _maxVelocity;

    private List<Bird> _neighbourList;

    public void Init(FlockManager flockManager, float fieldView, float speed, float maxVelocity)
    {
        _manager = flockManager;
        _fieldView = fieldView;
        _speed = speed;
        _maxVelocity = maxVelocity;
        _neighbourList = new List<Bird>();

        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>(true);

        foreach (Renderer rend in renderers)
        {
            Material[] mats = rend.materials;

            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i] == _bodyMaterial || mats[i] == _featherMaterial)
                {
                    Color color = Color.white;

                    if (leader)
                    {
                        color = Color.yellow;
                    }
                    else
                    {
                        switch (behaviorType)
                        {
                            case BehaviorTypeEnum.enthusiastic:
                                color = Color.red;
                                break;
                            case BehaviorTypeEnum.latecomer:
                                color = Color.blue;
                                break;
                            case BehaviorTypeEnum.clingy:
                                color = Color.green;
                                break;
                        }
                    }

                    mats[i].color = color;
                }
            }

            rend.materials = mats;
        }
    }

    public void Tick(List<Bird> birdList, float deltaTime)
    {
        NeighbourDetector(birdList);
        switch (behaviorType)
        {
            case BehaviorTypeEnum.enthusiastic:
                EnthusiasticMovement(deltaTime);
                break;
            case BehaviorTypeEnum.latecomer:
                ClignyMovement(deltaTime);
                break;
            case BehaviorTypeEnum.clingy:
                LateComerMovement(deltaTime);
                break;
        }
    }

    private void LateComerMovement(float deltaTime)
    {

    }

    private void ClignyMovement(float deltaTime)
    {

    }

    private void EnthusiasticMovement(float deltaTime)
    {

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
