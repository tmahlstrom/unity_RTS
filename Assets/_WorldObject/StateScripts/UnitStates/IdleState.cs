using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class IdleState : UnitBaseState {

	public IdleState (Unit unit, bool manualInit) : base (unit, manualInit) { 
		unit.animationManager.ChangeAnimation(RTS.EAnimation.Die, false);
		unit.animationManager.ChangeAnimation(RTS.EAnimation.Move, false);
		unit.mover.EnableMoveInput();
		unit.mover.EnableRotation();
	}



}
