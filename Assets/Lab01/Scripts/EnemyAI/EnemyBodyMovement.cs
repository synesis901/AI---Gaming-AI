using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBodyMovement : MonoBehaviour {

    /*TODO list
     * 1)Add collision logic with walls, it seems to be very buggy and glitchy when I add wall collision detection, will have to figure something out
     * 2)Need to add waypoint logic similar to A* as well, have a limited history that ensures it doesn't go back, so that's half done
     * 3)Currently when it loses the player it doesn't take go towards the closest waypoint... need to find out how to do that but does go
     * back to random movement but will figure that out later.  ATM the function is called way too much, and a huuuuuge memory hog
     */

    private static float maxTimeDelay = 10.0f;
    private float rotationTimer = float.NaN;

    private bool seePlayer = false;
    private bool foundPlayer = false;

    public Waypoint3 startLocation;
    public Waypoint3 destination;
    public Waypoint3 currentLocation;
    public Waypoint3 target;

    GameObject[] findClosestWP;
    GameObject[] otherEnemyMovers;

    public float shortestDistance = -1;

    public bool arrived = false;
    public bool moving = false;
    public bool initDistance = false;
    bool found;

    float startTime;

    float speed = 20.0f;

    List<Waypoint3> waypointHistory;

    public GameObject tankLeader;

    void PlayerFound()
    {
        foundPlayer = true;
    }

    void PlayerLost()
    {
        foundPlayer = false;
    }

    void SeePlayer()
    {
        seePlayer = true;
    }

    void DoNotSeePlayer()
    {
        seePlayer = false;
    }

    void ResetTank()
    {
        findClosestWP = GameObject.FindGameObjectsWithTag("Waypoint");

        float tempDistance = 100;

        foreach (GameObject closestWP in findClosestWP)
        {
            if (Vector3.Distance(transform.position, closestWP.transform.position) <= tempDistance)
            {
                tempDistance = Vector3.Distance(transform.position, closestWP.transform.position);
                target = closestWP.GetComponent<Waypoint3>();
            }
        }
        waypointHistory = new List<Waypoint3>();

        waypointHistory.Add(target);

        seePlayer = false;
        foundPlayer = false;

    }

	// Use this for initialization
	void Start () {
        rotationTimer = Time.time;
		currentLocation = startLocation;
        transform.position = startLocation.transform.position;
        waypointHistory = new List<Waypoint3>();
        waypointHistory.Add(currentLocation);
        
	}
	
	// Update is called once per frame
	void Update () {

        if (tankLeader != null)
        {

        }
        else
        {
            RaycastHit hitTank;

            var ray1 = new Ray(transform.position, transform.forward);
            Debug.DrawLine(ray1.origin, ray1.origin + ray1.direction * 5, Color.red);
            var ray2 = new Ray(transform.position, Quaternion.Euler(0, -45, 0) * transform.forward);
            Debug.DrawLine(ray2.origin, ray2.origin + ray2.direction * 5, Color.green);
            var ray3 = new Ray(transform.position, Quaternion.Euler(0, 45, 0) * transform.forward);
            Debug.DrawLine(ray3.origin, ray3.origin + ray3.direction * 5, Color.green);

            //gets a little too close to the wall, moves backwards to reset it's orientation
            //doesn't avoid walls with pathfinding atm, doesn't seem to work well with it... will have to debug.  Adding it to the total else/if block makes
            //it unusually glitchy, will look into it later.  Pathfinding works enough as is
            if (Physics.Raycast(transform.position, transform.forward, out hitTank, 3.0f))
            {
                if (hitTank.collider.gameObject.tag.Equals("Wall"))
                {
                    transform.parent.GetComponent<EnemyTankMovement>().SendMessage("EnemyStop");
                    transform.parent.GetComponent<EnemyTankMovement>().SendMessage("EnemyMoveBackwards");
                }
            }

            if (!seePlayer && foundPlayer)
            {
                float degree = Mathf.Asin(Vector3.Cross(transform.forward, transform.Find("PlayerDetector").transform.forward).y) * Mathf.Rad2Deg;

                if (degree > 5)
                {
                    transform.parent.GetComponent<EnemyTankMovement>().SendMessage("EnemyRotateTank", 1.0f);
                }
                else if (degree < -5)
                {
                    transform.parent.GetComponent<EnemyTankMovement>().SendMessage("EnemyRotateTank", 0.0f);
                }
                else
                {
                    transform.parent.GetComponent<EnemyTankMovement>().SendMessage("EnemyMoveForward");
                    transform.parent.GetComponent<EnemyTankMovement>().SendMessage("EnemyResetRotation");
                }
            }
            else if (seePlayer)
            {

                //locating the player when it's close
                float degree = Mathf.Asin(Vector3.Cross(transform.forward, transform.Find("PlayerDetector").transform.forward).y) * Mathf.Rad2Deg;

                if (degree > 5)
                {
                    transform.parent.GetComponent<EnemyTankMovement>().SendMessage("EnemyRotateTank", 1.0f);
                }
                else if (degree < -5)
                {
                    transform.parent.GetComponent<EnemyTankMovement>().SendMessage("EnemyRotateTank", 0.0f);
                }
                else
                {
                    //Debug.Log(Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position));
                    if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 4.0)
                    {
                        transform.parent.GetComponent<EnemyTankMovement>().SendMessage("EnemyStop");
                        transform.parent.GetComponent<EnemyTankMovement>().SendMessage("EnemyResetRotation");
                    }
                    else
                    {
                        transform.parent.GetComponent<EnemyTankMovement>().SendMessage("EnemyMoveForward");
                        transform.parent.GetComponent<EnemyTankMovement>().SendMessage("EnemyResetRotation");
                    }
                }


            }
            else
            {
                PathfinderAI();

                //avoid wall logic, doesn't currently work well with pathfinding
                //if (Physics.Raycast(transform.position, transform.forward, out hitTank, 5.0f))
                //{

                //    if (hitTank.collider.gameObject.tag.Equals("Wall"))
                //    {

                //        SendMessageUpwards("EnemyRotateTank", Random.value);
                //        SendMessageUpwards("EnemyStop");

                //    }
                //}
                //else
                //{

                //    //ensures it can clear the obsticle otherwise, continue moving forward
                //    //if (Physics.Raycast(transform.position, Quaternion.Euler(0, -45, 0) * transform.forward, 6.0f))
                //    //{
                //    //    SendMessageUpwards("EnemyRotateTank", 1.0f);
                //    //}
                //    //else if (Physics.Raycast(transform.position, Quaternion.Euler(0, 45, 0) * transform.forward, 6.0f))
                //    //{
                //    //    SendMessageUpwards("EnemyRotateTank", 0.0f);
                //    //}
                //    //else
                //    //{

                //    //}
                //}
            }
        }
    }

    void PathfinderAI()
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


                //waypoint randomizer - ensures it doesn't go back to a previous waypoint via waypoint history
                bool doNotAdd = false;
                List<Waypoint3> possibleLocations = new List<Waypoint3>();

                for (int i = 0; i < currentLocation.connections.Count; i++)
                {
                    for (int b = 0; b < waypointHistory.Count && !doNotAdd; b++)
                    {
                        if (currentLocation.connections[i] == waypointHistory[b])
                        {
                            doNotAdd = true;
                        }
                    }

                    if (doNotAdd == true)
                        doNotAdd = false;
                    else
                        possibleLocations.Add(currentLocation.connections[i]);
                }
                otherEnemyMovers = GameObject.FindGameObjectsWithTag("EnemyMover");
                
                if (possibleLocations.Count == 0)
                {
                    target = currentLocation.connections[Random.Range(0, currentLocation.connections.Count - 1)];

                    //ensures no 2 tanks are going to the same location
                    foreach (GameObject enemyMover in otherEnemyMovers)
                    {
                        if (enemyMover.transform != this.transform)
                        {
                            while (enemyMover.GetComponent<EnemyBodyMovement>().target == target)
                            {
                                target = currentLocation.connections[Random.Range(0, currentLocation.connections.Count - 1)];
                            }
                        }
                    }
                }
                else
                {
                    target = possibleLocations[Random.Range(0, possibleLocations.Count - 1)];
                    
                    //ensures no 2 tanks are going to the same location
                    foreach (GameObject enemyMover in otherEnemyMovers)
                    {
                        if(enemyMover.transform != this.transform)
                        {
                            while (enemyMover.GetComponent<EnemyBodyMovement>().target == target)
                            {
                                target = possibleLocations[Random.Range(0, possibleLocations.Count - 1)];
                            }
                        }
                    }
                }

                moving = true;
                startTime = Time.time;
            }
            else
            {
                //going towards the waypoint that was determined above
                transform.Find("WaypointDetector").transform.LookAt(target.transform);
                if (Vector3.Distance(transform.parent.position, target.transform.position) > 0.5f)
                {

                    float degree = Mathf.Asin(Vector3.Cross(transform.forward, transform.Find("WaypointDetector").transform.forward).y) * Mathf.Rad2Deg;

                    if (degree > 1)
                    {
                        transform.parent.GetComponent<EnemyTankMovement>().SendMessage("EnemyStop");
                        transform.parent.GetComponent<EnemyTankMovement>().SendMessage("EnemyRotateTank", 1.0f);
                    }
                    else if (degree < -1)
                    {
                        transform.parent.GetComponent<EnemyTankMovement>().SendMessage("EnemyStop");
                        transform.parent.GetComponent<EnemyTankMovement>().SendMessage("EnemyRotateTank", 0.0f);
                    }
                    else
                    {
                        transform.parent.GetComponent<EnemyTankMovement>().SendMessage("EnemyMoveForward");
                        transform.parent.GetComponent<EnemyTankMovement>().SendMessage("EnemyResetRotation");
                    }
                    //float fracComplete = (Time.time - startTime) / 1.0f;
                    //transform.position = Vector3.Slerp(currentLocation.transform.position, target.transform.position, fracComplete);
                    //float distCovered = (Time.time - startTime) * speed;
                    //float fracJourney = distCovered / shortestDistance;
                    //transform.position = Vector3.Lerp(currentLocation.transform.position, target.transform.position, fracJourney);
                }
                else
                {
                    moving = false;
                    initDistance = false;
                    currentLocation = target;

                    transform.parent.position = currentLocation.transform.position;

                    //adding to the waypoint history objects, stores up to the previous 15 waypoints, ensures that it doesn't go to the same location twice too often
                    //capping amount at 15, seems to work pretty well at 15
                    if (waypointHistory.Count == 15)
                    {
                        waypointHistory.RemoveAt(0);
                        waypointHistory.Add(currentLocation);
                    }
                    else
                        waypointHistory.Add(currentLocation);

                    if (transform.parent.position == destination.transform.position)
                    {
                        arrived = true;
                        transform.parent.GetComponent<EnemyTankMovement>().SendMessage("EnemyStop");
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
