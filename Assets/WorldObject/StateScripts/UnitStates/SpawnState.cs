using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class SpawnState : UnitBaseState {

	public SpawnState (Unit unit, bool manualInit) : base(unit, manualInit){
		unit.mover.ClearMovement();
		unit.mover.DisableRotation(); 
		if (unit.spawner.ReadyToBeginSpawning()){
			unit.spawner.InitiateSpawnProcess();
			unit.animationManager.ChangeAnimation(RTS.EAnimation.Move, false);
			unit.animationManager.ChangeAnimation(RTS.EAnimation.Spawn, true);
		} else {
            		SelfExitState(RTS.EAnimation.Spawn);
		}
	}

	public override void UpdateState(){

	}

	public override bool StateMidAnimation(){
		if (unit.spawner.IsSpawnProcessStarted()){
			return true; 
		}
        	return false; 
    	}


	public override void AnimationClimaxEvent(RTS.EAnimation state){
		unit.spawner.SpawnClimaxEvent(); 
    	}

    	public override void AnimationCompletionEvent(RTS.EAnimation state){
		unit.spawner.ConcludeSpawnProcess();
        	SelfExitState(RTS.EAnimation.Spawn);
	}

	public override void ExitRoutine(){
		unit.mover.EnableRotation();
        	unit.animationManager.ChangeAnimation(RTS.EAnimation.Spawn, false);
	}

	public override void SelfExitState(RTS.EAnimation state){
		if (state == RTS.EAnimation.Spawn){
			base.SelfExitState(state);
			unit.SetUnitState(new IdleState(unit, false));
		}
	}


}
