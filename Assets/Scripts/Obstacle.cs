using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    private float strength = 10f;                                               // Repulsion force
    private float radius;

    private List<Bird> birds;

    [System.Obsolete]
    void Start() {
        radius = GetComponent<SphereCollider>().radius;                         // Get radius from collider
        if (radius <= 0) radius = 10f;                                          // Default radius if none

        FlockManager manager = FindObjectOfType<FlockManager>();                // Assumes there's only one FlockManager
        if (manager != null ) birds = manager.Agents;
    }

    void Update() {
        if (birds == null) return;

        foreach (Bird bird in birds) {
            if (bird == null) continue;

            Vector3 offset = bird.transform.position - transform.position;
            float distance = offset.magnitude;

            if (distance < radius && distance > 0.01f) {
                float factor = 1f - (distance / radius);                        // Force increase with proximity
                Vector3 repulse = offset.normalized * strength * factor;

                bird.AddForce(repulse * Time.deltaTime);                      // ? Should use time
                //bird.AddForce(repulse);                                         // Apply force to bird
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
