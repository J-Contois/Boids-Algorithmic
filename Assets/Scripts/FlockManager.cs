using UnityEngine;

// ! Can have multiples leaders
public class NewMonoBehaviourScript : MonoBehaviour {
    [SerializeField] private int _numberOfAgents = 20;
    [SerializeField] private float _spawnRadius = 5f;
    [SerializeField] private float _agentSight = 10f;

    [SerializeField] private enum FlockBehaviour { Dense, Loose, Elongated };

    private GameObject[] _Agents;

    void Start() {
        // ! Create parent object to hold all agents
        // ! Create agents at random positions within the spawn radius
        // ! First one is the leader : follow its own path
        _Agents = new GameObject[_numberOfAgents];
        _Agents[0] = gameObject;
        for (int i = 1; i < _numberOfAgents; i++) {
            Vector3 randomPos = Random.insideUnitSphere * _spawnRadius;
            _Agents[i] = Instantiate(gameObject, randomPos, Quaternion.identity);
        }
    }

    void Update() {
        foreach (var agent in _Agents) {
            //agent.Tick(_Agents);
        }
    }
}
