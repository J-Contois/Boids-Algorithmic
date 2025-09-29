using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    [SerializeField] private Bird _birdPrefab;
    /*    [SerializeField] private int limitBird;*/
    [SerializeField] private Array<Bird> _typeList;

    [Header("Bird Settings")]
    [SerializeField] private float _fieldView;
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _typeList = new List<BehaviorTypeEnum>();
        CreateBird();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0;  i < _typeList.Count; i++)
    }

    public void CreateBird()
    {
        for (int i = 0;  i < _typeList.Count; i++)
        {
            _birdPrefab = new Bird(
                _typeList[i],
                _fieldView,
                Random.Range(_minSpeed, _maxSpeed),
                Random.Range(0f, 1f));

            Instantiate(_birdPrefab, transform);
        }
}
