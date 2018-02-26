using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class MoveState : UnitBaseState {

	public MoveState (Unit unit, bool manualInit) : base(unit, manualInit){
		//unit.mover.ResetInteractions();
		unit.animationManager.ChangeAnimation(RTS.EAnimation.Move, true);
	}

 



	public override void SelfExitState(RTS.EAnimation state){
		if (state == RTS.EAnimation.Move){
            manuallyInitatedState = false;
            unit.animationManager.ChangeAnimation(RTS.EAnimation.Move, false);
            manuallyInitatedState = false;
            //unit.mover.ClearMovement(); this causes problem of turning to look at previous move point after arrival at basic ally interact
            unit.SetUnitState(new IdleState(unit, false));
		}
	}
}
