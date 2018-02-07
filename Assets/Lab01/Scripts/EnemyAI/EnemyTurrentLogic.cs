using UnityEngine;
using System.Collections;

public class EnemyTurrentLogic : MonoBehaviour {

    #region Variables
    public float turretRotationSpeed = 100.0f;
    bool setForward = false; // This bool is used to determine if the turret is being reset to the forward position.

    bool seePlayer = false;
    #endregion

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //float hInput2 = Input.GetAxis("Horizontal2");

        //// If there is some horizontal input from the keyboard stop the turret from resetting to the forward position.
        //if (hInput2 != 0)
        //{
        //    setForward = false;
        //}

        //transform.Rotate(Vector3.up, hInput2 * turretRotationSpeed * Time.deltaTime); // Rotate the turret

        //// If the 'I' key is pressed reset the turret position to forward.
        //if (Input.GetKey(KeyCode.I))
        //{
        //    setForward = true;
        //}
        //if (setForward)
        //{
        //    // Set the turret rotation to that of the tank body
        //    transform.rotation = Quaternion.Slerp(transform.rotation, transform.root.rotation, Time.deltaTime);
            
        //    if (transform.rotation == transform.root.rotation)
        //    {
        //        setForward = false;
        //    }
        //}
	}
}
