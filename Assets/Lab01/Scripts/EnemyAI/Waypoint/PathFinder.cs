using UnityEngine;
using System.Collections;

public class PathFinder : MonoBehaviour 
{
	public Waypoint3 startLocation;
	public Waypoint3 destination;
	public Waypoint3 currentLocation;
	public Waypoint3 target;
	
	public float shortestDistance = -1;
	
	public bool arrived = false;
	public bool moving = false;
	public bool initDistance = false;
	bool found;
	
	float startTime;
	
	float speed = 20.0f;
	
	
	// Use this for initialization
	void Start () 
	{
		transform.position = startLocation.transform.position;
		currentLocation = startLocation;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!arrived)
		{
			if (!moving)
			{
				if (!initDistance)
				{
					shortestDistance = Vector3.Distance(currentLocation.transform.position, destination.transform.position);
					initDistance = true;
					startTime = Time.time;
					found = false;
				}
					
				for (int i = 0; i < currentLocation.connections.Count && !found; i++)
				{
					float tempDistance = Vector3.Distance(currentLocation.connections[i].transform.position, destination.transform.position);
					
					if (currentLocation.connections[i] == destination)
					{
						target = currentLocation.connections[i];
						found = true;
						
					}
					else if (tempDistance < shortestDistance)
					{
						shortestDistance = tempDistance;
						target = currentLocation.connections[i];
					}
				}
				
				moving = true;
				startTime = Time.time;
			}
			else
			{
				if (Vector3.Distance(transform.position, target.transform.position) > 0.5f)
				{
					//float fracComplete = (Time.time - startTime) / 1.0f;
					//transform.position = Vector3.Slerp(currentLocation.transform.position, target.transform.position, fracComplete);
					float distCovered = (Time.time - startTime) * speed;
        			float fracJourney = distCovered / shortestDistance;
        			transform.position = Vector3.Lerp(currentLocation.transform.position, target.transform.position, fracJourney);
				}
				else
				{
					moving = false;
					initDistance = false;
					currentLocation = target;
					transform.position = currentLocation.transform.position;
					
					if (transform.position == destination.transform.position)
					{
						arrived = true;	
					}
				}
			}
		}
		else
		{
			currentLocation = destination;
			destination = startLocation;
			startLocation = currentLocation;
			arrived = false;
		}		
	}
}
