using UnityEngine;

public class Obstacle : MonoBehaviour {
    private readonly float _strength = 20f;                                               // Repulsion force
    private readonly float _bonusRadius = 2f;
    private float _radius;

    void Start() {
        _radius = GetComponent<SphereCollider>().radius;                         // Get radius from collider
        _radius *= transform.localScale.x;
        _radius += _bonusRadius;                                                  // Make effect zone larger than collider
    }

    public Vector3 GetRepulsionForce(Bird bird) {
        if (!bird) return Vector3.zero;

        Vector3 offset = bird.transform.position - transform.position;
        float distance = Vector3.Distance(transform.position, bird.transform.position);

        if (distance <= _radius && distance > 0.01f) {
            //float factor = 1f - (distance / radius);                        // Force increase with proximity
            //Vector3 repulse = offset.normalized * strength * factor;
            Vector3 repulse = offset.normalized * _strength;
            return repulse;
        }

        return Vector3.zero;
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
