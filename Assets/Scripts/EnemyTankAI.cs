using UnityEngine;
using System.Collections;

public class EnemyTankAI : MonoBehaviour 
{
	bool selected;
	
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
		
		if (selected)
		{
			selectionCircle.SetActive(true);
		}
		else
		{
			selectionCircle.SetActive(false);
		}
	}
	
	public bool IsSelected()
	{
		return selected;
	}		
}
