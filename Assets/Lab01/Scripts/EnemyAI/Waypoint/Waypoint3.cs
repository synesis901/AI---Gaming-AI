using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Waypoint3 : MonoBehaviour 
{
	Transform target;
	GameObject[] otherWP;
	public List<Waypoint3> connections;
	
	int maxConnections = 8;
	
	// Use this for initialization
	void Start () 
	{
		BuildPaths();
	}
	
	void BuildPaths () 
	{
		connections = new List<Waypoint3>();
		
		otherWP = GameObject.FindGameObjectsWithTag("Waypoint");
		
		foreach(GameObject target in otherWP)
		{
			if (target != null && target.transform != this.transform) 
			{
	            if (connections.Count < maxConnections)
				{
					if (Vector3.Distance(transform.position, target.transform.position) <= 20.0f)
					{

                        Debug.DrawLine(transform.position, target.transform.position, Color.red, 200, false);
                        connections.Add(target.GetComponent<Waypoint3>());

					}
				}
				else
				{
					bool found = false;
					for (int i = 0;i < connections.Count && !found;i++)
					{
						if (Vector3.Distance(transform.position, target.transform.position) <
							Vector3.Distance(transform.position, connections[i].transform.position))
						{
							connections[i] = target.GetComponent<Waypoint3>();
							found = true;
						}
					}
				}
	        }
		}
	}
	
	void OnDrawGizmos() 
	{
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
	}
	
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, new Vector3(2,2,2));
		
		for (int i = 0;i < connections.Count;i++)
		{
			Gizmos.DrawCube(connections[i].transform.position, new Vector3(2,2,2));
		}
	}
}