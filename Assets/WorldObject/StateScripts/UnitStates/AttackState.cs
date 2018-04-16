using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class AttackState : UnitBaseState {

	public AttackState (Unit unit, bool manualInit) : base(unit, manualInit){

	}

	public override void UpdateState(){
		if (unit.attacker.CheckIfReadyToBeginAttacking()){
			if (IsEngagedWithGO(unit.attacker.attackTargetCollider.gameObject)) {
				if (WorkManager.IsTargetColliderStillAlive(unit.attacker.attackTargetCollider) == true){
					PerformAttack();
				} else {
                    			SelfExitState(RTS.EAnimation.Attack); 
				}
			}
		}
		if (unit.attacker.attackTargetCollider == null){
            		SelfExitState(RTS.EAnimation.Attack); 
		}
	}

	private void GetInPositionToAttack(){
		unit.mover.EnableMoveInput();
        	if (unit.attacker.attackTargetCollider){
			unit.mover.PrimaryInteractGameObjectAtDistance(unit.attacker.attackTargetCollider.gameObject, unit.paramManager.AttackRange);
			if (!IsEngagedWithGO(unit.attacker.attackTargetCollider.gameObject)){
				if (unit.mover.IsInRangeOfPrimaryTarget(unit.attacker.attackTargetCollider.gameObject) == false){
                    			unit.animationManager.ChangeAnimation(RTS.EAnimation.Move, true);
                    			unit.animationManager.ChangeAnimation(RTS.EAnimation.Attack, false);
				}
			}
		}
	}


	
	private void PerformAttack (){
		unit.attacker.BeginAttackProcess();
        	unit.animationManager.ChangeAnimation(RTS.EAnimation.Move, false);
		unit.mover.DisableMoveInput();
        	unit.animationManager.ChangeAnimation(RTS.EAnimation.Attack, true);
	}

	public override void ReactToColliderFocus(Collider newTargetCol){
		unit.attacker.AttackThisCol(newTargetCol);
		GetInPositionToAttack();
	}


	public override void MouseClickRight (GameObject hitObject, Vector3 hitPoint) {
		if (hitObject){
			Collider attackCol = WorkManager.FindEnemyTargetCollider(hitObject, unit.worldObject);
			if (attackCol){
				AudioManager.Instance.Play("AttackCommand", unit.audioSource);
				WorldObject wo = attackCol.gameObject.GetComponentInParent<WorldObject>();
				if (wo){
					wo.TargetedByPlayer(); 
				}
				manuallyInitatedState = true; 
				if (unit.attacker.CheckForRepeatCommand(attackCol)){
					return;
				} else{
					unit.animationManager.ChangeAnimation(RTS.EAnimation.Attack, false);
					unit.attacker.ResetAttacker();
					ReactToColliderFocus(attackCol);
					return;
				}
			}
		}
		base.MouseClickRight(hitObject, hitPoint);
	}


    	public override void AnimationClimaxEvent(RTS.EAnimation state){
		if (state == RTS.EAnimation.Attack){
			if (unit.attacker.attackTargetCollider){
				if (unit.mover.IsInRangeOfPrimaryTarget(unit.attacker.attackTargetCollider.gameObject)){
                    			unit.attacker.AttackExecution();
					return;
				}
			}
			//AnimationCompletionEvent(state);
		}
    	}

	public override void AnimationCompletionEvent(RTS.EAnimation state){
		if (state == RTS.EAnimation.Attack){
            		unit.attacker.AttackIterationComplete();
            		CheckForAttackContinuation();
		}
	}

	private void CheckForAttackContinuation(){
		if (WorkManager.IsTargetColliderStillAlive(unit.attacker.attackTargetCollider) == false){
			unit.attacker.ResetAttacker();
            		SelfExitState(RTS.EAnimation.Attack); 
        	} else {
			if (!IsEngagedWithGO(unit.attacker.attackTargetCollider.gameObject)){
				GetInPositionToAttack();
			}
		}
		if (unit.paramManager.AttackCooldown > 0){
            		unit.animationManager.ChangeAnimation(RTS.EAnimation.Attack, false);
		}
	}

	public override bool StateMidAnimation(){
		if (unit.attacker.AttackProcessStarted()){
			return true; 
		}
        	return false; 
    	}


	public override void ExitRoutine(){
        	unit.animationManager.ChangeAnimation(RTS.EAnimation.Attack, false);
		unit.attacker.ResetAttacker();
		unit.mover.EnableMoveInput();
	}
	
	public override void SelfExitState(RTS.EAnimation state){
		if (state == RTS.EAnimation.Attack){
			base.SelfExitState(state);
			unit.mover.ClearMovement();
            		unit.SetUnitState(new IdleState(unit, false));
		}
	}

}
