using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class SitState : UnitBaseState {


	public SitState (Unit unit, bool manualInit) : base(unit, manualInit){
		unit.mover.ClearMovement();
		unit.mover.DisableMoveInput();
		unit.mover.DisableRotation();
		unit.animationManager.ChangeAnimation(EAnimation.Move, false); 
		unit.animationManager.ChangeAnimation(EAnimation.Sit, true); 
	}


	public override void ExitRoutine(){
        	unit.animationManager.ChangeAnimation(EAnimation.Sit, false);
	}
}
