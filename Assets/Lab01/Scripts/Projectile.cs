using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	public GameObject explosion;
	
	void OnCollisionEnter(Collision collision)
	{


		ContactPoint contact = collision.contacts[0];
	
		Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
		Instantiate(explosion, contact.point, rotation);
		
		// Destroy the projectile when it hits something
        if(collision.gameObject.tag.Equals("Enemy"))
        {
            Destroy(gameObject);
            collision.gameObject.SendMessage("EnemyDied");
        }

        if(collision.gameObject.tag.Equals("Player"))
        {
            Destroy(gameObject);
            collision.gameObject.SendMessage("PlayerEndGame");
            
        }
		Destroy(gameObject);
	}
}
