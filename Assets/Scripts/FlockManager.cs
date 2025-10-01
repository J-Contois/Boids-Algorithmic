using System.Collections.Generic;
using UnityEngine;

// Leader : yellow
// Latecomer : blue
// Enthusiastic : red
// Clingy : green

// I Each flock will have one leader
// ! Add flocks ability to exchange agents
// ! Add agents ability to flee predators and seek food
// ! Add agents ability to avoid obstacles
public class FlockManager : MonoBehaviour {
    [Header("Flock Settings")]
    [Tooltip("Number of agents in the flock")]
    [SerializeField] private int _numberOfAgents = 20;
    [Tooltip("Radius within which agents are spawned")]
    [SerializeField] private float _spawnRadius = 5f;
    //[Tooltip("Distance within which an agent will be considered separated from his flock")]
    //[SerializeField] private float separationRadius = 10f;                    // [later] When'll use several flock script

    [Header("Agent Settings")]
    [SerializeField] private Bird _agentPrefab;
    [Tooltip("Distance within which an agent can see other agents")]
    [SerializeField] private float _agentSight = 10f;
    [SerializeField] private float _agentMinSpeed = 1f;
    [SerializeField] private float _agentMaxSpeed = 10f;
    [SerializeField] private float _agentMaxVelocity = 100f;

    [HideInInspector] public enum FlockBehaviour { Dense, Loose, Elongated };
    [SerializeField] private FlockBehaviour flockBehaviour;

    private List<Bird> _Agents;
    private GameObject _agentParent;
    private float cohesion = 1f;
    private float separation = 1f;
    private float alignment = 1f;

    void Start() {
        _agentParent = new GameObject("AllAgents");                             // Create empty GameObject to hold all agents
        _agentParent.transform.parent = transform;                              // Inside self

        if (flockBehaviour == FlockBehaviour.Dense) {                           // Flock parameters based on behaviour
            separation = 0.6f;
            alignment = 0.3f;
        } else if (flockBehaviour == FlockBehaviour.Loose) {
            cohesion = 0.6f;
            alignment = 0.3f;
        } else if (flockBehaviour == FlockBehaviour.Elongated) {
            cohesion = 0.6f;
            separation = 0.3f;
        }

        if (_agentPrefab == null) return;
        _Agents = new List<Bird>();                                             // Create agents
        _Agents[0] = _agentPrefab;                                              // ! Make it the leader
        for (int i = 1; i < _numberOfAgents; i++) {
            Vector3 randomPos = Random.insideUnitSphere * _spawnRadius;         // Random position in spawn area
            Bird newBoid = Instantiate(_agentPrefab, randomPos, Quaternion.identity, _agentParent.transform);
            float _agentSpeed = Random.Range(_agentMinSpeed, _agentMaxSpeed);
            newBoid.Init(this, _agentSight, _agentSpeed, _agentMaxVelocity);
            _Agents.Add(newBoid);
        }
    }

    void Update() {
        if (_Agents == null) return;

        foreach (var agent in _Agents) agent.Tick(_Agents, Time.deltaTime);
    }

    public float GetCohesion() { return cohesion; }

    public float GetSeparation() { return separation; }

    public float GetAlignment() { return alignment; }
}
