using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private BehaviorTypeEnum _type;
    private float _fieldView;
    private float _speed;
    private float _force;

    private Vector3 _direction;
    private List<Bird> _neighbourList;

    public Bird(BehaviorTypeEnum type, float fieldView, float speed, float force)
    {
        _type = type;
        _fieldView = fieldView;
        _speed = speed;
        _force = force;
    }

    private void NeighbourDetector()
    {
        
    }

}
