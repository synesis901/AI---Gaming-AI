using UnityEngine;
using System.Collections;

public class EnemyCannon : MonoBehaviour {

    #region Variables
    public Rigidbody projectile;

    public float projectileVelocity = 50.0f;

    public float fireRate = 1.0f;
    float lastFire = -1.0f;
    #endregion

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //RaycastHit hit;

        // Checks to see what the raycast hit
        //if (Physics.Raycast(transform.position, transform.forward, out hit, 15.0f))
        //{
        //    //checks to see if the raycast hit the player which is tagged in Unity
        //    if(hit.transform.gameObject.tag.Equals("Player") && lastFire + fireRate < Time.time)
        //    {
        //        lastFire = Time.time;

        //        // Instantiate a copy of the projectile
        //        Rigidbody newProjectile = (Rigidbody)Instantiate(projectile, transform.position, transform.rotation);

        //        // Give the projectile a forward velocity
        //        newProjectile.velocity = transform.TransformDirection(0, 0, projectileVelocity);
        //    }
        //}

	}
}
