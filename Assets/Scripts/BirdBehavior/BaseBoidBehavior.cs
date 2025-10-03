using UnityEngine;

public abstract class BaseBoidBehavior : IBirdBehavior
{
    protected FlockManager manager;

    protected virtual float denseCoeff { get; } = 1f;
    protected virtual float looseCoeff { get; } = 1f;
    protected virtual float elongatedCoeff { get; } = 1f;

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
        Vector3 followLeader = FollowLeader(bird) * 1.5f;
        
        return cohesion + separation + alignment + bounds + followLeader;
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

        return (center - bird.transform.position);
    }

    private Vector3 CalculateSeparation(Bird bird)
    {
        if (bird.NeighbourList.Count == 0) return Vector3.zero;

        Vector3 separationForce = Vector3.zero;
        float separationRadius = 200f;
        
        foreach (Bird neighbour in bird.NeighbourList)
        {
            Vector3 diff = bird.transform.position - neighbour.transform.position;
            float distance = diff.magnitude;
            
            if (distance > 0)
            {
                float strength = (separationRadius - distance) / separationRadius;
                separationForce += diff.normalized * (strength * strength);
            }
        }

        return separationForce;
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

        return averageVelocity - bird.Velocity;
    }

    // Force required to remain within the constraint sphere
    public Vector3 CalculateBoundaryForce(Bird bird)
    {
        SphereCollider zone = manager.GetFlightZone();

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

    private Vector3 FollowLeader(Bird bird)
    {
        Bird leader = manager.GetLeader();
        
        if (leader == bird) return Vector3.zero;
        
        // Direction towards the leader
        Vector3 directionToLeader = leader.transform.position - bird.transform.position;
        float distance = directionToLeader.magnitude;
        
        // Ideal distance behind the leader (comfort zone)
        float idealDistance = 8f;
        
        if (distance < idealDistance * 0.5f)
        {
            // Too close: repulsive force
            return -directionToLeader.normalized * (idealDistance - distance);
        }
        else if (distance > idealDistance * 2f)
        {
            // Too far: strong gravitational pull
            return directionToLeader.normalized * Mathf.Min(distance - idealDistance, 5f);
        }
        else if (distance > idealDistance)
        {
            // A bit far: moderate gravitational pull
            return directionToLeader.normalized * (distance - idealDistance) * 0.5f;
        }
        
        // Dans la zone de confort : pas de force
        return Vector3.zero;
        
    }
}
