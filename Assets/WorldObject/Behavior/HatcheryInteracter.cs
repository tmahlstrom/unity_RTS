using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class HatcheryInteracter : MonoBehaviour {


	public GameObject GOfocus; 
	public bool performingInteract;
	private Hatchery hatcheryBeingTargeted;


	public bool IssueInteractCommand(WorldObject worldObject){
		if (worldObject){
			Hatchery hatchery = worldObject.gameObject.GetComponentInParent<Hatchery> ();
			if (hatchery && hatchery.active == true) {
				hatcheryBeingTargeted = hatchery;
				GOfocus = hatchery.gameObject;
				performingInteract = false; 
				return true;
			}
		}
		return false; 
	}

	public bool CheckForRepeatCommand(GameObject hitObject){
		WorldObject wo = hitObject.GetComponentInParent<WorldObject>(); 
		if (wo){
			Hatchery hitHatchery = hitObject.GetComponentInParent<Hatchery> ();
			if (hitHatchery && hatcheryBeingTargeted && hitHatchery == hatcheryBeingTargeted){
				return true; 
			}
		}
		return false; 
	}


	public void BeginInteract(){
		performingInteract = true; 
	}

    	public void InteractExecution(){
 		hatcheryBeingTargeted.ToggleHatcheryActivity(false); 
        	hatcheryBeingTargeted = null; 
		performingInteract = false;
    	}

	public void ResetHatcheryInteracter(){
		hatcheryBeingTargeted = null; 
		performingInteract = false; 
	}


}

