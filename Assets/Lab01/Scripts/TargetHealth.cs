using UnityEngine;
using System.Collections;

public class TargetHealth : MonoBehaviour 
{
	public float health = 100;
	public GameObject explosion;
	public float verticalForce = 5000.0f;
	
	public float deathTime = 2.0f;
	float timeStamp = 0;
	bool destruction = false;
	
	void ApplyDamage(float damage)
	{
		health -= damage;
		
		if (health <= 0)
		{
			timeStamp = Time.time;
			destruction = true;
			rigidbody.AddForce(0,verticalForce,0);
		}		
	}
	
	void Update()
	{
		if (destruction && Time.time > timeStamp + deathTime)
		{
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy(gameObject);
		}
	}
}
