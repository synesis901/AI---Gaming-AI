using UnityEngine;
using System.Collections;

public class TankAI : MonoBehaviour 
{
	bool selected;
	Collider target;
	
	public GameObject selectionCircle;
	
	// Use this for initialization
	void Start () 
	{
		selected = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public void SelectTank(bool newSelection)
	{
		selected = newSelection;
		
		// If selected then activate my selection circle.
		if (selected)
		{
			selectionCircle.SetActive(true);
			// If I have a target then activate my target's selection circle.
			if (target != null)
			{
				target.SendMessage("SelectTank", true);
			}
		}
		else // If I am deselected turn off my selelction circle.
		{
			// If I have a target deselect my target's selection circle.
			if (target != null)
			{
				target.SendMessage("SelectTank", false);
			}
			selectionCircle.SetActive(false);
		}
	}
	
	public bool IsSelected()
	{
		return selected;
	}
	
	public void AssignTarget(Collider newTarget)
	{
		// Assign me a target and activate my target's selection circle.
		target = newTarget;	
		target.SendMessage("SelectTank", true);
	}
	
	public void ClearTarget()
	{
		// Turn off my target's selection circle and set my target to null.
		if (target != null)
		{
			target.SendMessage("SelectTank", false);
			target = null;
		}
	}
}
