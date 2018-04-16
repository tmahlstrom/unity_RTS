using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class SpecialState : UnitBaseState {


	public SpecialState (Unit unit, bool manualInit) : base(unit, manualInit){
		unit.mover.ClearMovement();
		unit.mover.DisableMoveInput();
		unit.mover.DisableRotation();
		unit.animationManager.ChangeAnimation(RTS.EAnimation.Move, false); 
		unit.animationManager.ChangeAnimation(RTS.EAnimation.SpawnerSpecial, true); 
		unit.StartCoroutine(DelayedStartOfSpecial()); 
	}

	public override void SpaceBar(){
        	SelfExitState(RTS.EAnimation.SpawnerSpecial);
	}

	private IEnumerator DelayedStartOfSpecial(){
		yield return new WaitForSeconds (0.5f);
		if (unit.unitState == this){
			unit.spawner.InitiateSpawnerSpecial();
		}
	}

	public override void ExitRoutine(){
		unit.mover.EnableMoveInput();
		unit.spawner.ConcludeSpawnerSpecial();
        	unit.animationManager.ChangeAnimation(RTS.EAnimation.SpawnerSpecial, false);
	}

	public override void SelfExitState(RTS.EAnimation state){
		//ExitRoutine(); 
		unit.SetUnitState(new IdleState(unit, false));
	}

}
