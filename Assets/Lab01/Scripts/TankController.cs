using UnityEngine;
using System.Collections;
 
public class TankController : MonoBehaviour 
{
	#region Variables
    private bool endGame = false;
    GameObject[] EnemyData;

    private int enemySeesPlayer = 0;

	public float speed = 10.0f;
	public float maxVelocityChange = 10.0f;
	
	public float tankRotationSpeed = 100.0f; // Multiplier for the tank rotation.
	public float hInput;

    public GUIStyle GUIStyle;
    float endGameTime = 0.0f;

    private int playerLives = 0;
	#endregion
	
	void Awake () 
	{
        playerLives = 3;
	    // Freeze the rigidbody rotation on all axis if you don't want the tank to be able to roll.
		// This means that it will look fine as long as you are moving on a flat surface.
		rigidbody.freezeRotation = true;
	}

    void PlayerEndGame()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        endGame = true;
        endGameTime = Time.time;
        Application.Quit();
        
    }

    void OnGUI()
    {
        if(!endGame)
            GUI.Box(new Rect(10, 10, 200, 10), Time.realtimeSinceStartup.ToString(), GUIStyle);
        else
            GUI.Box(new Rect(10, 10, 200, 10), endGameTime.ToString() + " : Game Over.  Player has been captured.", GUIStyle);
            
    }

    void EnemyInsideSphereOfInfluence()
    {
        enemySeesPlayer++;
    }

    void EnemyOutsideSphereOfInfluence()
    {
        enemySeesPlayer--;
    }

	void FixedUpdate () 
	{
        if (enemySeesPlayer >= 2 && !endGame)
            PlayerEndGame();

        //Debug.Log(enemySeesPlayer);


	    hInput = Input.GetAxis("Horizontal"); // Input value for the horizontal axis (-1 to 1)
		
        // Calculate how fast we should be moving
		// Sets targetVelocity to be forward(1) or back(-1) according to the world.
        Vector3 targetVelocity = new Vector3(0, 0, Input.GetAxis("Vertical"));
		
		// Changes the velocity to be relative to the direction the tank is pointing.
        targetVelocity = transform.TransformDirection(targetVelocity);
		
        // Multiplies the targetVelocity by the speed of the tank.
		targetVelocity *= speed;

        // Apply a force that attempts to reach our target velocity
		// Subtract the current velocity from the targetVelocity to determine the velocityChange.
        Vector3 velocityChange = (targetVelocity - rigidbody.velocity);
		
        // Clamp the x and z velocityChange so it does not exceed the maximum.
		velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
		
		// Set the y velocityChange to 0 to make sure the tank stays on the ground.
        velocityChange.y = 0;
        
		// Add the force to the rigidbody to make it move.
		rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
		
		transform.Rotate(Vector3.up, hInput * tankRotationSpeed * Time.deltaTime); // Rotate the tank
 	}
}