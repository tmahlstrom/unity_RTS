using System.Collections;
using UnityEngine;
using RTS; 

public class UnitBaseState {

	protected Unit unit;
	protected bool manuallyInitatedState = false;
	public bool ManuallyInitatedState { get {return manuallyInitatedState; }}

   	public UnitBaseState(Unit unit, bool manualInit){
		this.unit = unit;
        	this.manuallyInitatedState = manualInit;
	}


	public virtual void UpdateState(){

	}



	public virtual void SpaceBar(){
		if (unit.builder){
			unit.builder.InitiateBuildingPlacement();
			return; 
		}
		if (unit.spawner){
			if (unit.spawner.ReadyToSpawnerSpecial()){
				unit.SetUnitState(new SpecialState(unit, true));
			}
		}
	}

	public virtual void MouseClickRight (GameObject hitObject, Vector3 hitPoint) {
		unit.mover.EnableMoveInput();
       		WorldObject hitWorldObject = hitObject.GetComponentInParent<WorldObject> ();
		if (unit.hatcheryInteracter){
			if (unit.hatcheryInteracter.IssueInteractCommand(hitWorldObject)){
				AudioManager.Instance.Play("AllyInteractCommand");
				unit.SetUnitState(new HatcheryInteractState(unit, true));
				if (hitWorldObject){
					hitWorldObject.TargetedByPlayer(); 
				}
				return; 
			}
        	}

		if (unit.builder) { 
			if (unit.builder.CheckToCancelBuildingPlacement()){
				return; 
			}
			if (unit.builder.CheckToGiveResumeBuildOrder(hitObject)){
				unit.SetUnitState(new BuildState(unit, true));
				if (hitWorldObject){
		    			hitWorldObject.TargetedByPlayer(); 
				}
				return;
			}
        	}

		if (unit.attacker){
			Collider attackCol = WorkManager.FindEnemyTargetCollider(hitObject, unit.worldObject);
			if (attackCol){
				AudioManager.Instance.Play("AttackCommand", unit.audioSource);
			if (hitWorldObject){
				hitWorldObject.TargetedByPlayer(); 
			}
			unit.SetUnitState(new AttackState(unit, true));
			unit.SetColliderFocus(attackCol);
			return;
		    }
		}

		if (unit.reviver) {
			if (unit.reviver.IssueReviveCommand(hitObject)){
				AudioManager.Instance.Play("AllyInteractCommand");
				if (hitWorldObject){
					hitWorldObject.TargetedByPlayer(); 
				}
				unit.SetUnitState(new ReviveState(unit, true));
				return;
			}        
		}

		if (unit.mover){
			if (hitWorldObject && WorkManager.AreWorldObjectsOnSameTeam (hitWorldObject, unit.worldObject) == true) {
				AudioManager.Instance.Play("AllyInteractCommand");
				unit.mover.ClearMovement();
				//unit.mover.SecondaryInteractGameObjectAtDistance (hitObject, 1.2f);
				unit.mover.PrimaryInteractGameObjectAtDistance(hitObject, 1.2f);
				unit.SetUnitState(new MoveState(unit, true));
				return;
			}            
			if (hitWorldObject == null){
				//AudioManager.Instance.Play("MoveCommand");
				unit.mover.ClearMovement();
				unit.player.groupFormationManager.DetermineFormationGroup(null, hitPoint);
				unit.SetUnitState(new MoveState(unit, true));
				return;
			}
		}

    	}

	public virtual void MouseClickLeft (Vector3 hitPoint){
		unit.mover.EnableMoveInput();
		if (unit.builder) {
	    		if (unit.builder.CheckToGiveNewBuildOrder()){
				AudioManager.Instance.Play("SelectionChange");
				unit.SetUnitState(new BuildState(unit, true));
	    		}
		}
	}


	public virtual bool StateMidAnimation(){
		return false; 
	}


	public virtual bool IsEngagedWithGO (GameObject engagementTarget){
        	if (unit.mover.IsInRangeOfPrimaryTarget(engagementTarget)) {
			if (unit.mover.IsFacingPrimaryTarget(engagementTarget)){
            			return true;
			}
        	}
        	return false;
    	}

	public virtual void AnimationClimaxEvent(RTS.EAnimation state){

	}

	public virtual void AnimationCompletionEvent(RTS.EAnimation state){

	}


	public virtual void ReactToColliderFocus(Collider colFocus){

	}

	public virtual void SelfExitState(RTS.EAnimation state){
		if (unit.paramManager.IsDead){
            		return;
        	}
        	manuallyInitatedState = false;      
	}

	public virtual void ExitRoutine(){

	}



}
