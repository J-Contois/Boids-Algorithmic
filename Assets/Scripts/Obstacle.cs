using UnityEngine;

public class Obstacle : MonoBehaviour {
    private float strength = 100f;                                               // Repulsion force
    private float radius;

    void Start() {
        radius = GetComponent<SphereCollider>().radius;                         // Get radius from collider
        radius += 10f;                                                          // Make effect zone larger than collider
    }

    public Vector3 GetRepulsionForce(Bird bird) {
        if (bird == null) return Vector3.zero;

        Vector3 offset = bird.transform.position - transform.position;
        //float distance = offset.magnitude;
        float distance = Vector3.Distance(transform.position, bird.transform.position);

        if (distance <= radius && distance > 0.01f) {
            //float factor = 1f - (distance / radius);                        // Force increase with proximity
            float factor = 1f;
            Vector3 repulse = offset.normalized * strength * factor;
            return repulse;
        }

        return Vector3.zero;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
