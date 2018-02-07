using UnityEngine;
using System.Collections;

public class RaycastMouseCursor : MonoBehaviour 
{
    RaycastHit hit; // This will be used to store the location of where the raycast hits.
    Collider selected; // This is the object used to show where the raycast hit.
    float distance;
    Vector3 outputPosition;
	
	NavMeshAgent agent;
	
    // Update is called once per frame
    void Update()
    {
        // Generate a plane that intersects the transform's position with an upwards normal.
        // Plane playerPlane = new Plane(Vector3.up, transform.position);
        Plane zeroPlane = new Plane(Vector3.up, Vector3.zero);

        // Generate a ray from the cursor position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Store the location on the plane where the raycast intersects.
		if (zeroPlane.Raycast(ray, out distance))
        {
            outputPosition = ray.origin + ray.direction * distance;
        }

        // Mouse click
		if (Input.GetButtonDown("Fire1"))
        {
            // If the raycast hit anything
			if (Physics.Raycast(transform.position, ray.direction, out hit, 1000))
            {
                // If the raycast hit a friendly unit
				if (hit.collider.tag == "Friendly")
                {
					// Store the friendly unity in the selected variable
					selected = hit.collider;
					// Store the NavMeshAgent of the selected unit
					agent = hit.transform.GetComponent<NavMeshAgent>();
                }
				// If the raycast hit the floor
				else if(hit.collider.tag == "Floor")
				{
					// If a friendly unit is selected
					if (selected != null)
					{
						// Set the destination of the selected units NavMeshAgent
						agent.destination = hit.point;
					}
				}
            }
        }
    }
}
