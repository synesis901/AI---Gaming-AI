using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour 
{
	public float explosionTime = 1.0f; // Length of time that the particle system should emit.
    public float explosionRadius = 5.0f; // The damage radius of the explosion.
    public float explosionPower = 10.0f; // The power of the explosion for adding force.
    public float explosionDamage = 100.0f; // The damage value of the explosion.
	
	float timeStamp; // A time stamp to determine when to destroy the explosion object.

    void Start () 
    {
        Vector3 explosionPosition = transform.position;
        Vector3 closestPoint;
        float distance;
        double adjustedDamage;
        
		// Get a list of colliders within the explosion radius.
		Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
		
		// Calculate damage and force for each object in the explosion radius.
        foreach (Collider hit in colliders)
        {
            // If the current collider has a rigidbody.
			if (hit.rigidbody)
            {
                // Apply a force to the rigidbody.
				hit.rigidbody.AddExplosionForce(explosionPower, explosionPosition, explosionRadius, 3.0f);

                // Find the closest point of the rigidbody to the center of the explosion.
				closestPoint = hit.rigidbody.ClosestPointOnBounds(explosionPosition);
                // Calculate the distance from the closest point to the center of the explosion.
				distance = Vector3.Distance(closestPoint, explosionPosition);

                // Calculate the percentage of the damage to apply based on the distance.
				adjustedDamage = 1.0 - Mathf.Clamp01(distance / explosionRadius);
                adjustedDamage *= explosionDamage;

                // Send the message ApplyDamage to the script on the object with the amount of damage.
				hit.rigidbody.SendMessageUpwards("ApplyDamage", adjustedDamage, SendMessageOptions.DontRequireReceiver);
            }
        }
		
        // Start the particle system if one exists and set the timeStamp.
		if (particleSystem)
        {
			timeStamp = Time.time;
			particleSystem.Play();
        }
		
		// Destroy the object when the particles have finished emitting.
		Destroy(gameObject, explosionTime + particleSystem.startLifetime + 1); // Add one extra second to make sure.
    }
	
	void Update ()
	{
		// If the current time is greater than the timeStamp + explosionTime stop the particle system.
		if (Time.time > timeStamp + explosionTime)
		{
			particleSystem.Stop();
		}
	}
}
