using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class BuildState : UnitBaseState {

	public BuildState (Unit unit, bool manualInit) : base(unit, manualInit){
		GetInPositionToBuild();
	}

	public override void UpdateState(){
		if (unit.builder.CheckIfReadyToBeginBuilding()){
			if (unit.builder.AbleToPayResources()){
				unit.builder.InitializeConstruction();
				PerformBuild();
			} else {
                SelfExitState(RTS.EAnimation.Build);
			}
		}
	}

	private void GetInPositionToBuild(){
        	if (unit.builder.GOfocus){
			if (!IsEngagedWithGO(unit.builder.GOfocus)){
				unit.mover.PrimaryInteractGameObjectAtDistance(unit.builder.GOfocus, 0.6f);
                		unit.animationManager.ChangeAnimation(RTS.EAnimation.Build, false);
				if (unit.mover.IsInRangeOfPrimaryTarget(unit.builder.GOfocus) == false){
                    			unit.animationManager.ChangeAnimation(RTS.EAnimation.Move, true);
				} else{
                    			unit.animationManager.ChangeAnimation(RTS.EAnimation.Move, false);
				}
			 }
		}
	}
	
	private void PerformBuild (){
		unit.animationManager.ChangeAnimation(RTS.EAnimation.Move, false);
		unit.animationManager.ChangeAnimation(RTS.EAnimation.Build, true);
	}


	public override void MouseClickRight (GameObject hitObject, Vector3 hitPoint) {
		if (unit.builder.CheckToCancelBuildingPlacement()){
			return; 
		}
		if (unit.builder.CheckForRepeatCommand(hitObject)){
			WorldObject wo = hitObject.gameObject.GetComponentInParent<WorldObject>();
			if (wo){
				wo.TargetedByPlayer(); 
			}			
			return;
		}
		if (unit.builder.CheckToGiveResumeBuildOrder(hitObject)){
			WorldObject wo = hitObject.gameObject.GetComponentInParent<WorldObject>();
			if (wo){
				wo.TargetedByPlayer(); 
			}
			GetInPositionToBuild(); 
            		return;
        	}
		base.MouseClickRight(hitObject, hitPoint);
	}

	public override void MouseClickLeft(Vector3 hitPoint){
		if (unit.builder.CheckToGiveNewBuildOrder()){
			GetInPositionToBuild();
			return;
        	}
	}



    	public override void AnimationClimaxEvent(RTS.EAnimation state){
		if (state == RTS.EAnimation.Build){
            		unit.builder.BuildExecution();
		}
    	}

	public override void ExitRoutine(){
        	unit.animationManager.ChangeAnimation(RTS.EAnimation.Build, false);
		unit.builder.ResetBuilder();
	}
	
	public override void SelfExitState(RTS.EAnimation state){
		if (state == RTS.EAnimation.Build){
			base.SelfExitState(state);
            		unit.mover.ClearMovement();
           		unit.SetUnitState(new IdleState(unit, false));
		}
	}



}
