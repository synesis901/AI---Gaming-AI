using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {

    GameObject player;

    //may impliment, can use spehere of influence for sound detection

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(other.transform.tag.Equals("Enemy"))
        {
            player.GetComponent<TankController>().SendMessage("EnemyInsideSphereOfInfluence");

        }
    }

    void OnTriggerExit(Collider other)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (other.transform.tag.Equals("Enemy"))
        {
            player.GetComponent<TankController>().SendMessage("EnemyOutsideSphereOfInfluence");

        }
    }
}
