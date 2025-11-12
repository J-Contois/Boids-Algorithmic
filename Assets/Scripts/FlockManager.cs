using System.Collections.Generic;
using BirdBehavior;
using UnityEngine;

// !! Keep leader inside flight zone

// ! Keep agents inside flight area (does it work ?)
// I Each flock will have one leader
// L Add flocks ability to exchange agents
// L Add agents ability to flee predators and seek food
// L Add agents ability to avoid obstacles
public class FlockManager : MonoBehaviour {
    [Header("Flock Settings")]
    [Tooltip("Number of agents in the flock")]
    [SerializeField] private int numberOfAgents = 20;
    [Tooltip("Size of the area in which agents can move")]
    [SerializeField] private float flightRadius = 80f;
    [Tooltip("Radius within which agents are spawned")]
    [SerializeField] private float spawnRadius = 20f;
    //[Tooltip("Distance within which an agent will be considered separated from his flock")]
    //[SerializeField] private float separationRadius = 10f;                    // [later] When will use several flock script

    [Tooltip("How dense the flock while be (close to each other")]
    [SerializeField, Range(0, 100)] private int denseWeight = 50;
    [Tooltip("How loose the flock will be (more space between agents)")]
    [SerializeField, Range(0, 100)] private int looseWeight = 50;
    [Tooltip("How elongated the flock will be (more eparse than in cluster)")]
    [SerializeField, Range(0, 100)] private int elongatedWeight = 50;

    [Header("Agent Settings")]
    [SerializeField] private Bird agentPrefab;
    [Tooltip("Distance within which an agent can see other agents")]
    [SerializeField] private float agentSight = 30f;
    [SerializeField] private float agentMinSpeed = 1f;
    [SerializeField] private float agentMaxSpeed = 10f;
    [SerializeField] private float agentMaxVelocity = 20f;

    private List<Bird> agents;
    private Obstacle[] obstacles;
    private GameObject _agentParent;
    private Bird _leader;
    private SphereCollider _zone;

    void Start() {
        obstacles = FindObjectsByType<Obstacle>(FindObjectsSortMode.None);

        CreateFlightZone();

        CreateFlock();
    }

    void Update() {
        if (agents == null) return;

        foreach (var agent in agents) {
            Vector3 force = GetObstaclesForce(agent);
            agent.Tick(agents, force, agentMinSpeed, Time.deltaTime);
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = new Color(0f, 0.5f, 1f, 0.2f);
        Gizmos.DrawSphere(transform.position, flightRadius);
    }

    void CreateFlightZone() {
        _zone = gameObject.GetComponent<SphereCollider>();                      // Add flight zone component
        if (_zone == null) _zone = gameObject.AddComponent<SphereCollider>();
        _zone.radius = flightRadius;
    }

    void CreateFlock() {
        _agentParent = new GameObject("AllAgents") {
            transform = { parent = transform }};                                // Create parent inside self to hold all agents

        if (!agentPrefab) return;

        agents = new List<Bird>();

        // Create leader
        Vector3 randomPos = Random.insideUnitSphere * spawnRadius;             // Random position in spawn area
        _leader = Instantiate(agentPrefab, randomPos, Quaternion.identity, _agentParent.transform);
        IBirdBehavior behavior = new LeaderBehavior(this);
        float leaderSpeed = Random.Range(agentMinSpeed, agentMaxSpeed);
        _leader.Init(behavior, agentSight, leaderSpeed, agentMaxVelocity);
        agents.Add(_leader);

        if (Camera.main) {
            CameraManager cameraManager = Camera.main.GetComponent<CameraManager>();
            if (cameraManager) cameraManager.SetTarget(_leader.transform);      // Make camera follow leader
        }

        for (int i = 1; i < numberOfAgents; i++) {
            randomPos = Random.insideUnitSphere * spawnRadius;                 // Random position in spawn area
            Bird newBoid = Instantiate(agentPrefab, randomPos, Quaternion.identity, _agentParent.transform);
            float agentSpeed = Random.Range(agentMinSpeed, agentMaxSpeed);
            behavior = GetRandomBehaviour();
            newBoid.Init(behavior, agentSight, agentSpeed, agentMaxVelocity);
            agents.Add(newBoid);
        }
    }

    private IBirdBehavior GetRandomBehaviour() {
        int index = Random.Range(0, 4);
        float dense = denseWeight * 0.01f;
        float loose = looseWeight * 0.01f;
        float elongated = elongatedWeight * 0.01f;

        switch (index) {
            case 0: return new LatecomerBehavior(this, dense, loose, elongated);
            case 1: return new EnthusiasticBehavior(this, dense, loose, elongated);
            case 2: return new ClingyBehavior(this, dense, loose, elongated);
            default: return new NormalBehavior(this, dense, loose, elongated);
        }
    }

    private Vector3 GetObstaclesForce(Bird bird) {
        Vector3 force = Vector3.zero;
        foreach (var obstacle in obstacles) force += obstacle.GetRepulsionForce(bird);
        return force;
    }

    public SphereCollider GetFlightZone() => _zone;

    public Bird GetLeader() => _leader;
}
