using UnityEngine;
using System.Collections;

public class EnemyTankMovement : MonoBehaviour {

    #region Variables

    GameObject[] findAllWP;
    GameObject[] otherTanks;

    public float speed = 10.0f;
    public float maxVelocityChange = 10.0f;

    public float tankRotationSpeed = 100.0f; // Multiplier for the tank rotation.

    private float enemyForwardMovement = 0.0f;
    private float enemyRotationMovement = 0.0f;

    public enum TankStatus
    {
        Leader = 1,
        Follower = 2
    }

    public TankStatus tankStatus;
    #endregion

	// Use this for initialization
	void Start () {
	}

    void EnemyDied()
    {
        findAllWP = GameObject.FindGameObjectsWithTag("Waypoint");
        otherTanks = GameObject.FindGameObjectsWithTag("EnemyMover");

        GameObject respawnLocation = GameObject.FindGameObjectWithTag("EnemyRespawn");


        //this procedure is -suppose- to create another tank, but for w/e reason it's not cloneing properly
        //if (otherTanks.Length < (findAllWP.Length - 5))
        //{
        //    respawnLocation = findAllWP[Random.Range(0, findAllWP.Length - 1)];
        //    foreach (GameObject tank in otherTanks)
        //    {
        //        while (respawnLocation.GetComponent<Waypoint3>() == tank.GetComponent<EnemyBodyMovement>().target || respawnLocation.GetComponent<Waypoint3>() == tank.GetComponent<EnemyBodyMovement>().currentLocation)
        //        {
        //            respawnLocation = findAllWP[Random.Range(0, findAllWP.Length - 1)];
        //        }
        //    }
        //    Instantiate(this, respawnLocation.transform.position, Quaternion.identity);

        //    bool resetTankLogic = false;
        //    GameObject[] otherTanksAfterCloning = GameObject.FindGameObjectsWithTag("EnemyMover");

        //    //this is just to reset the new clone
        //    foreach (GameObject tankPostClone in otherTanksAfterCloning)
        //    {
        //        foreach (GameObject tank in otherTanks)
        //        {
        //            if (tank == tankPostClone)
        //                resetTankLogic = true;
        //        }
                
        //        if (resetTankLogic == false)
        //            tankPostClone.GetComponent<EnemyBodyMovement>().SendMessage("ResetTank");

        //        resetTankLogic = false;
        //    }

        //}

        respawnLocation = findAllWP[Random.Range(0, findAllWP.Length - 1)];
        foreach (GameObject tank in otherTanks)
        {
            while (respawnLocation.GetComponent<Waypoint3>() == tank.GetComponent<EnemyBodyMovement>().target || respawnLocation.GetComponent<Waypoint3>() == tank.GetComponent<EnemyBodyMovement>().currentLocation)
            {
                respawnLocation = findAllWP[Random.Range(0, findAllWP.Length - 1)];
            }
        }
        transform.position = respawnLocation.transform.position;
        transform.Find("BodyMover").GetComponent<EnemyBodyMovement>().SendMessage("ResetTank");
    }

    void EnemyMoveBackwards()
    {
        enemyForwardMovement = -1.0f;
    }

    void EnemyMoveForward()
    {
        enemyForwardMovement = 1.0f;
    }

    void EnemyRotateTank(float rotationLogic)
    {
        if (rotationLogic > .5f)
            enemyRotationMovement = 1.0f;
        else
            enemyRotationMovement = -1.0f;
    }

    void EnemyResetRotation()
    {
        enemyRotationMovement = 0.0f;
    }

    void EnemyStop()
    {
        enemyForwardMovement = 0.0f;
    }
	// Update is called once per frame
	void Update () {

        // Calculate how fast we should be moving
        // Sets targetVelocity to be forward(1) or back(-1) according to the world.
        Vector3 enemyVelocity = new Vector3(0, 0, enemyForwardMovement);

        // Changes the velocity to be relative to the direction the tank is pointing.
        enemyVelocity = transform.TransformDirection(enemyVelocity);

        // Multiplies the targetVelocity by the speed of the tank.
        enemyVelocity *= speed;

        // Apply a force that attempts to reach our target velocity
        // Subtract the current velocity from the targetVelocity to determine the velocityChange.
        Vector3 velocityChange = (enemyVelocity - rigidbody.velocity);

        // Clamp the x and z velocityChange so it does not exceed the maximum.
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);

        // Set the y velocityChange to 0 to make sure the tank stays on the ground.
        velocityChange.y = 0;

        // Add the force to the rigidbody to make it move.
        rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

        transform.Rotate(Vector3.up, enemyRotationMovement * tankRotationSpeed * Time.deltaTime); // Rotate the tank
	}
}
