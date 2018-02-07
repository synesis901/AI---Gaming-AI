using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour 
{
	#region Variables
	public Rigidbody projectile;
	
	public float projectileVelocity = 50.0f;
	
	public float fireRate = 1.0f;
	float lastFire = -1.0f;
	#endregion
	
	// Update is called once per frame
	void Update () 
	{
		// When the spacebar is pressed check the fireRate to determine whether to fire a projectile.
		if (Input.GetAxis("Fire1") > 0 && lastFire + fireRate < Time.time)
		{
			lastFire = Time.time;

			// Instantiate a copy of the projectile
			Rigidbody newProjectile = (Rigidbody)Instantiate(projectile, transform.position, transform.rotation);
			
			// Give the projectile a forward velocity
			newProjectile.velocity = transform.TransformDirection(0, 0, projectileVelocity);
		}
	}
}
