using UnityEngine;
using System.Collections;

public class DetectPlayer : MonoBehaviour {

    GameObject[] otherTanks;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);

        var ray = new Ray(transform.position, transform.forward);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 15, Color.white);

        RaycastHit seePlayerRay;

        if (Physics.Raycast(transform.position, transform.forward, out seePlayerRay, 14.0f))
        {
            if(seePlayerRay.transform.gameObject.tag.Equals("Player"))
            {
                transform.parent.GetComponent<EnemyBodyMovement>().SendMessage("SeePlayer");

                otherTanks = GameObject.FindGameObjectsWithTag("EnemyMover");
                foreach (GameObject tank in otherTanks)
                {
                    tank.GetComponent<EnemyBodyMovement>().SendMessage("PlayerFound");
                }
            }
            else
            {
                transform.parent.GetComponent<EnemyBodyMovement>().SendMessage("DoNotSeePlayer");
                otherTanks = GameObject.FindGameObjectsWithTag("EnemyMover");
                foreach (GameObject tank in otherTanks)
                {
                    tank.GetComponent<EnemyBodyMovement>().SendMessage("PlayerLost");
                }
            }
        }

	}
}
