using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    [SerializeField] private Bird _birdPrefab;
    [SerializeField] private int limit;

    [Header("Bird Settings")]
    [Tooltip("Minimum speed used for a random speed range")]
    [SerializeField] private float _minSpeed;
    [Tooltip("Maximum speed used for a random speed range")]
    [SerializeField] private float _maxSpeed;
    [Tooltip("Maximum acceleration of agents calcul with the random range speed")]
    [SerializeField] private float _maxVelocity;
    [SerializeField] private float _fieldView;

    private List<Bird> _birdList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _birdList = new List<Bird>();

        if (_birdPrefab)
        {
            CreateBird();
        } 
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0;  i < _birdList.Count; i++)
        {
            _birdList[i].NeighbourDetector(_birdList);
            _birdList[i].Move(Time.deltaTime);
        }
    }

    public void CreateBird()
    {
        for (int i = 0; i < limit; i++)
        {
            Bird newBird = Instantiate(_birdPrefab, transform);

            newBird.Init(
                _fieldView,
                Random.Range(_minSpeed, _maxSpeed),
                _maxVelocity);

            _birdList.Add(newBird);
        }
    }
}
