using UnityEngine;

public abstract class BaseBoidBehavior : IBirdBehavior
{
    protected FlockManager manager;
    protected Bird leader;

    protected float denseCoeff = 1f;
    protected float looseCoeff = 1f;
    protected float elongatedCoeff = 1f;

    protected float flockDensity = 0.5f;
    protected float flockLooseness = 0.5f;
    protected float flockElongating = 0.5f;

    public virtual Color GetColor()
    {
        return Color.wheat;
    }

    public BaseBoidBehavior(FlockManager flockManager, float dense, float loose, float elongated)
    {
        manager = flockManager;
        leader = manager.getLeader();
        flockDensity = dense;
        flockLooseness = loose;
        flockElongating = elongated;
    }

    public void SetManager(FlockManager newManager)
    {
        manager = newManager;
    }

    public virtual Vector3 CalculateMovement(Bird bird, float deltaTime)
    {
        Vector3 cohesion = CalculateCohesion(bird) * flockDensity * denseCoeff;
        Vector3 separation = CalculateSeparation(bird) * flockLooseness * looseCoeff;
        Vector3 alignment = CalculateAlignment(bird) * flockElongating * elongatedCoeff;
        Vector3 bounds = CalculateBoundaryForce(bird) * 2f;                     // Strong force to remain in the sphere

        return cohesion + separation + alignment + bounds;
    }

    private Vector3 CalculateCohesion(Bird bird)
    {
        if (bird.NeighbourList.Count == 0) return Vector3.zero;

        Vector3 center = Vector3.zero;
        foreach (Bird neighbour in bird.NeighbourList)
        {
            center += neighbour.transform.position;
        }
        center /= bird.NeighbourList.Count;

        return (center - bird.transform.position).normalized;
    }

    private Vector3 CalculateSeparation(Bird bird)
    {
        if (bird.NeighbourList.Count == 0) return Vector3.zero;

        Vector3 separationForce = Vector3.zero;
        foreach (Bird neighbour in bird.NeighbourList)
        {
            Vector3 diff = bird.transform.position - neighbour.transform.position;
            float distance = diff.magnitude;
            if (distance > 0)
            {
                separationForce += diff.normalized / distance;
            }
        }

        return separationForce.normalized;
    }

    private Vector3 CalculateAlignment(Bird bird)
    {
        if (bird.NeighbourList.Count == 0) return Vector3.zero;

        Vector3 averageVelocity = Vector3.zero;
        foreach (Bird neighbour in bird.NeighbourList)
        {
            averageVelocity += neighbour.Velocity;
        }
        averageVelocity /= bird.NeighbourList.Count;

        return averageVelocity.normalized;
    }

    // Force required to remain within the constraint sphere
    private Vector3 CalculateBoundaryForce(Bird bird)
    {
        SphereCollider zone = manager.getFlightZone();

        Vector3 center = zone.transform.position + zone.center;
        float radius = zone.radius * zone.transform.lossyScale.x;
        Vector3 offset = bird.transform.position - center;
        float distance = offset.magnitude;
        
        // If approaching the edge (80% of the radius), force returns to the centre
        float threshold = radius * 0.8f;
        
        if (distance > threshold)
        {
            // The closer you get to the edge, the stronger the force becomes.
            float strength = (distance - threshold) / (radius - threshold);
            return -offset.normalized * strength;
        }
        
        return Vector3.zero;
    }
}
