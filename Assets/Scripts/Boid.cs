using UnityEngine;

// Leader : yellow
// Latecomer : blue
// Enthusiastic : red
// Clingy : green
public class Boid : MonoBehaviour {
    private FlockManager _manager;
    private Vector3 _velocity;
    private float _sight;
    private Boid[] _Neighbors;

    public void Init(FlockManager manager, float sight) {
        _manager = manager;
        _sight = sight;
        _velocity = Random.insideUnitSphere;                                    // Random initial velocity
    }

    public void Tick(Boid[] Flock) {
        
    }
}
