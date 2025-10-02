using UnityEngine;
using System.Collections.Generic;

public class BaseBoidBehavior : IBirdBehavior
{
    public abstract float CohesionWeight { get; }
    public abstract float SeparationWeight { get; }
    public abstract float AlignmentWeight { get; }
    public abstract Color GetColor();

    public Vector3 CalculateMovement(Bird bird, float deltaTime)
    {
        Vector3 cohesion = CalculateCohesion(bird) * CohesionWeight;
        Vector3 separation = CalculateSeparation(bird) * SeparationWeight;
        Vector3 alignment = CalculateAlignment(bird) * AlignmentWeight;
        Vector3 bounds = CalculateBoundaryForce(bird, manager) * 2f; // Strong force to remain in the sphere

        return cohesion + separation + alignment;
    }

    protected Vector3 CalculateCohesion(Bird bird)
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

    protected Vector3 CalculateSeparation(Bird bird)
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

    protected Vector3 CalculateAlignment(Bird bird)
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
    protected Vector3 CalculateBoundaryForce(Bird bird, FlockManager manager)
    {
        GlobalFlockManager globalManager = manager.GetGlobalManager();
        if (globalManager == null) return Vector3.zero;
        
        Vector3 center = globalManager.GlobalCenter;
        float radius = globalManager.GlobalBoundaryRadius;
        
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
