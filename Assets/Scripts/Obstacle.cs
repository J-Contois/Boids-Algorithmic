using UnityEngine;

public class Obstacle : MonoBehaviour {
    private readonly float strength = 20f;                                               // Repulsion force
    private float radius;
    private readonly float bonusRadius = 2f;

    void Start() {
        radius = GetComponent<SphereCollider>().radius;                         // Get radius from collider
        radius *= transform.localScale.x;
        radius += bonusRadius;                                                  // Make effect zone larger than collider
    }

    public Vector3 GetRepulsionForce(Bird bird) {
        if (!bird) return Vector3.zero;

        Vector3 offset = bird.transform.position - transform.position;
        float distance = Vector3.Distance(transform.position, bird.transform.position);

        if (distance <= radius && distance > 0.01f) {
            //float factor = 1f - (distance / radius);                        // Force increase with proximity
            //Vector3 repulse = offset.normalized * strength * factor;
            Vector3 repulse = offset.normalized * strength;
            return repulse;
        }

        return Vector3.zero;
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
