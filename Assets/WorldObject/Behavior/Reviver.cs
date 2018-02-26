using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class Reviver : MonoBehaviour {


    //*******************************************
    //BEGIN INTERFACE 
    //*******************************************

	public GameObject GOfocus; 
	public bool performingRevive;

	public bool IssueReviveCommand(GameObject hitObject){
		if (AbleToPayCost()){
			Corpse corpse = hitObject.GetComponentInParent<Corpse> ();
			if (corpse && !corpse.IsFinishedBeingCollected()) {
				corpseBeingRevived = corpse;
				GOfocus = corpse.gameObject;
				performingRevive = false; 
				return true;
			}
		}
		return false; 
	}

	public bool CheckForRepeatCommand(GameObject hitObject){
		WorldObject wo = hitObject.GetComponentInParent<WorldObject>(); 
		if (wo){
			Corpse hitCorpse = wo.GetComponentInChildren<Corpse>();
			if (hitCorpse && corpseBeingRevived && hitCorpse == corpseBeingRevived){
				return true; 
			}
		}
		return false; 
	}

	private bool AbleToPayCost(){
		if (unit.paramManager.ManaPoints > 33){
			return true; 
		}
		return false; 
	}

	public void BeginRevive(){
		performingRevive = true; 
	}

    public void ReviveExecution(){
		unit.ChangeMana(-33); 
        FinishCorpseRevival(corpseBeingRevived); 
        corpseBeingRevived = null; 
		performingRevive = false;
    }

	public void ResetReviver(){
		isTryingToCollectCorpse = false;
		corpseBeingRevived = null; 
		performingRevive = false; 
	}


    //*******************************************
    //END INTERFACE 
    //*******************************************

	private List<GameObject> deadGameObjectsCurrentlyCarried = new List<GameObject> (); 
	private Corpse corpseBeingRevived;

	private bool isTryingToCollectCorpse= false;
	private bool isTryingToAccessElevator = false;
	private PlayerElevator elevator; 
	private Unit unit; 


	private void Awake(){
		unit = GetComponent<Unit>(); 
	}
	private float corpseCollectionContributionFloat = 0.0f;

		


	private void FinishCorpseRevival (Corpse collectedCorpse){
        WorldObject wo = collectedCorpse.GetComponent<WorldObject>();
        if (wo){
            wo.ReviveWO();
        }
	}

	//protected virtual void KeepUpWithAnyCorpseCollectionYouAreDoing(){
 //       if(actionManager.collecting) {
	//		if(corpseBeingCollected != null && corpseBeingCollected.IsFinishedBeingCollected() == false) {
	//			float incrementalContribution = corpseCollectionSpeed * Time.deltaTime;
	//			corpseCollectionContributionFloat += incrementalContribution;
	//			if(corpseCollectionContributionFloat > 1) {
	//				int constructionContributionInt = Mathf.FloorToInt (corpseCollectionContributionFloat); 
	//				corpseBeingCollected.BeCollected(constructionContributionInt);
	//				corpseCollectionContributionFloat = 0.0f;
	//				if (corpseBeingCollected.IsFinishedBeingCollected () == true) {
	//					FinishCorpseCollection (corpseBeingCollected);
 //                       actionManager.RequestActionChangeApproval(Action.Collect, false);
	//					corpseBeingCollected = null; 
	//					//movement.ResetInteractions (); 
	//				}
	//			}
	//		}
	//	}
	//}

	//protected virtual void MonitorConditionsForStartingCorpseCollection(){
	//	if (isTryingToCollectCorpse) {
	//		if (movement.IsEngagedWithTarget(corpseBeingCollected.gameObject)) {
 //               if (actionManager.RequestActionChangeApproval(Action.Collect, true)) {
 //                   isTryingToCollectCorpse = false;

 //               }
	//		}
	//		else {
 //               actionManager.RequestActionChangeApproval(Action.Collect, false);
	//		}
	//	}
	//}


    // private void MonitorConditionsForStartingCorpseCollection(){
    //         if (corpseBeingRevived && actionManager.IsEngagedWithGO(corpseBeingRevived.gameObject)){
    //             actionManager.RequestActionChangeApproval(State.Revive, true);
    //             }
    // }



	// public void MonitorConditionsForRegisteringBluePrint(){
	// 	if (deadGameObjectsCurrentlyCarried.Count > 0 && elevator && movement.IsInRangeOfPrimaryTarget (elevator.gameObject)) {
	// 		elevator.RegisterBlueprintAssignment (deadGameObjectsCurrentlyCarried [0]);
	// 		deadGameObjectsCurrentlyCarried.RemoveAt (0);  
	// 	}
	// }



}

