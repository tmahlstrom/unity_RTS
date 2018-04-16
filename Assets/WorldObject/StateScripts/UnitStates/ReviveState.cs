using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class ReviveState : UnitBaseState {

	private Coroutine reviveSound; 

	public ReviveState (Unit unit, bool manualInit) : base(unit, manualInit){
		GetInPositionToRevive();
	}

	public override void UpdateState(){
		if (unit.reviver.performingRevive == false){
			if (unit.reviver.GOfocus && IsEngagedWithGO(unit.reviver.GOfocus)){
				PerformRevive(); 
			}
		}
	}

	private void GetInPositionToRevive(){
        	if (unit.reviver.GOfocus){
			if (!IsEngagedWithGO(unit.reviver.GOfocus)){
				unit.mover.PrimaryInteractGameObjectAtDistance(unit.reviver.GOfocus, 0.9f);
                		unit.animationManager.ChangeAnimation(RTS.EAnimation.Revive, false);
				if (reviveSound != null){
					unit.StopCoroutine(reviveSound); 
				}
				reviveSound = unit.StartCoroutine(ReviveSound(false));
                		unit.animationManager.ChangeAnimation(RTS.EAnimation.Move, true);
			} else{
				PerformRevive();
			}
		}
	}

	private void PerformRevive (){
		unit.animationManager.ChangeAnimation(RTS.EAnimation.Move, false);
		unit.animationManager.ChangeAnimation(RTS.EAnimation.Revive, true);
		unit.reviver.BeginRevive(); 
		if (reviveSound != null){
			unit.StopCoroutine(reviveSound); 
		}
		reviveSound = unit.StartCoroutine(ReviveSound(true)); 
	}



	public override void MouseClickRight (GameObject hitObject, Vector3 hitPoint) {
		if (unit.reviver.CheckForRepeatCommand(hitObject)){
			WorldObject wo = hitObject.gameObject.GetComponentInParent<WorldObject>();
			if (wo){
				wo.TargetedByPlayer(); 
			}
			return;
		}
		if (unit.reviver.IssueReviveCommand(hitObject)){
			WorldObject wo = hitObject.gameObject.GetComponentInParent<WorldObject>();
			if (wo){
				wo.TargetedByPlayer(); 
			}
			GetInPositionToRevive();
			
            	return;
        	}
		base.MouseClickRight(hitObject, hitPoint);
	}

	public override void AnimationClimaxEvent(RTS.EAnimation state){
		if (state == RTS.EAnimation.Revive){
		    unit.reviver.ReviveExecution();
		    SelfExitState(RTS.EAnimation.Revive);
		}
    	}

	private IEnumerator ReviveSound(bool toggle){
		if (unit.worldObject.audioSource){
			if (unit.paramManager.SpecialAudioClip){
				if (toggle == true){
					yield return new WaitForSeconds(0.4f);
					AudioManager.Instance.Play(unit.paramManager.SpecialAudioClip, unit.worldObject.audioSource);
				} else {
					if (unit.worldObject.audioSource.clip == unit.paramManager.SpecialAudioClip){
						unit.worldObject.audioSource.Stop(); 
					}
				} 
			}
		}
	}


	public override void ExitRoutine(){
		unit.animationManager.ChangeAnimation(RTS.EAnimation.Revive, false);
		if (reviveSound != null){
			unit.StopCoroutine(reviveSound); 
		}
		unit.StartCoroutine(ReviveSound(false)); 
		unit.reviver.ResetReviver();
	}

	public override void SelfExitState(RTS.EAnimation state){
		if (state == RTS.EAnimation.Revive){
			base.SelfExitState(state);
			unit.mover.ClearMovement();
			unit.SetUnitState(new IdleState(unit, false));
		}
	}





}
