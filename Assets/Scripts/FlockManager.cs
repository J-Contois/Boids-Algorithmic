using System.Collections.Generic;
using UnityEngine;

// Leader : yellow
// Latecomer : blue
// Enthusiastic : red
// Clingy : green

// ! Keep agents inside flight area
// ! Add range field for flock behaviour
// I Each flock will have one leader
// L Add flocks ability to exchange agents
// L Add agents ability to flee predators and seek food
// L Add agents ability to avoid obstacles
public class FlockManager : MonoBehaviour {
    [Header("Flock Settings")]
    [Tooltip("Number of agents in the flock")]
    [SerializeField] private int _numberOfAgents = 20;
    [Tooltip("Size of the area in which agents can move")]
    [SerializeField] private float _flightRadius = 100f;
    [Tooltip("Radius within which agents are spawned")]
    [SerializeField] private float _spawnRadius = 20f;
    //[Tooltip("Distance within which an agent will be considered separated from his flock")]
    //[SerializeField] private float separationRadius = 10f;                    // [later] When'll use several flock script
    [SerializeField, Range(0, 100)] private int _denseWeight = 50;
    [SerializeField, Range(0, 100)] private int _looseWeight = 50;
    [SerializeField, Range(0, 100)] private int _elongatedWeight = 50;

    [Header("Agent Settings")]
    [SerializeField] private Bird _agentPrefab;
    [Tooltip("Distance within which an agent can see other agents")]
    [SerializeField] private float _agentSight = 10f;
    [SerializeField] private float _agentMinSpeed = 1f;
    [SerializeField] private float _agentMaxSpeed = 10f;
    [SerializeField] private float _agentMaxVelocity = 100f;

    private List<Bird> _Agents;
    private GameObject _agentParent;

    void Start() {
        CreateFlightZone();

        CreateFlock();
    }

    void Update() {
        if (_Agents == null) return;

        foreach (var agent in _Agents) agent.Tick(_Agents, Time.deltaTime);
    }

    void CreateFlightZone() {
        SphereCollider zone = gameObject.GetComponent<SphereCollider>();        // Add flight zone component
        if (zone == null) zone = gameObject.AddComponent<SphereCollider>();
        zone.radius = _flightRadius;
        Gizmos.color = new Color(0f, 0.5f, 1f, 0.2f);
        Gizmos.DrawSphere(transform.position, zone.radius);
    }

    void CreateFlock() {
        _agentParent = new GameObject("AllAgents");                             // Create empty GameObject to hold all agents
        _agentParent.transform.parent = transform;                              // Inside self

        if (_agentPrefab == null) return;

        _Agents = new List<Bird>();                                             // Create agents
        _Agents.Add(_agentPrefab);                                              // ! Make it the leader

        for (int i = 1; i < _numberOfAgents; i++) {
            Vector3 randomPos = Random.insideUnitSphere * _spawnRadius;         // Random position in spawn area
            Bird newBoid = Instantiate(_agentPrefab, randomPos, Quaternion.identity, _agentParent.transform);
            float _agentSpeed = Random.Range(_agentMinSpeed, _agentMaxSpeed);
            newBoid.Init(_agentSight, _agentSpeed, _agentMaxVelocity, _denseWeight, _looseWeight, _elongatedWeight);
            _Agents.Add(newBoid);
        }
    }
}
