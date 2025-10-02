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
    private SphereCollider _zone;

    void Start() {
        CreateFlightZone();

        CreateFlock();
    }

    void Update() {
        if (_Agents == null) return;

        foreach (var agent in _Agents) agent.Tick(_Agents, Time.deltaTime);
    }

    void CreateFlightZone() {
        _zone = gameObject.GetComponent<SphereCollider>();                      // Add flight zone component
        if (_zone == null) _zone = gameObject.AddComponent<SphereCollider>();
        _zone.radius = _flightRadius;
        Gizmos.color = new Color(0f, 0.5f, 1f, 0.2f);
        Gizmos.DrawSphere(transform.position, _zone.radius);
    }

    void CreateFlock() {
        _agentParent = new GameObject("AllAgents");                             // Create empty GameObject to hold all agents
        _agentParent.transform.parent = transform;                              // Inside self

        if (_agentPrefab == null) return;

        _Agents = new List<Bird>();                                             // Create agents

        // Create leader
        Vector3 randomPos = Random.insideUnitSphere * _spawnRadius;             // Random position in spawn area
        Bird leader = Instantiate(_agentPrefab, randomPos, Quaternion.identity, _agentParent.transform);
        IBirdBehavior behavior = new LeaderBehavior(this);
        float _leaderSpeed = Random.Range(_agentMinSpeed, _agentMaxSpeed);
        leader.Init(behavior, _agentSight, _leaderSpeed, _agentMaxVelocity);
        _Agents.Add(leader);

        for (int i = 1; i < _numberOfAgents; i++) {
            randomPos = Random.insideUnitSphere * _spawnRadius;                 // Random position in spawn area
            Bird newBoid = Instantiate(_agentPrefab, randomPos, Quaternion.identity, _agentParent.transform);
            float _agentSpeed = Random.Range(_agentMinSpeed, _agentMaxSpeed);
            behavior = GetRandomBehaviour();
            newBoid.Init(behavior, _agentSight, _agentSpeed, _agentMaxVelocity);
            _Agents.Add(newBoid);
        }
    }

    private IBirdBehavior GetRandomBehaviour() {
        int index = Random.Range(0, 3);

        switch (index) {
            case 0:
                return new LatecomerBehavior(this, _denseWeight, _looseWeight, _elongatedWeight);
            case 1:
                return new EnthusiasticBehavior(this, _denseWeight, _looseWeight, _elongatedWeight);
            case 2:
                return new ClingyBehavior(this, _denseWeight, _looseWeight, _elongatedWeight);
            default:
                return new EnthusiasticBehavior(this, _denseWeight, _looseWeight, _elongatedWeight);
        }
    }

    public SphereCollider getFlightZone() { return _zone; }
}
