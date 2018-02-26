using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class DeadState : UnitBaseState {

	public DeadState (Unit unit, bool manualInit) : base(unit, manualInit){
		unit.animationManager.ChangeAnimation(RTS.EAnimation.Die, true);
		unit.mover.ClearMovement();  
		unit.mover.DisableMoveInput();
		unit.mover.DisableRotation();
	}

	public override void SpaceBar(){
		return;
	}

	public override void MouseClickRight(GameObject hitObject, Vector3 hitPoint){
		return;
	}

	public override void MouseClickLeft(Vector3 hitPoint){
		return;
	}

	public override void ExitRoutine(){

	}


}
