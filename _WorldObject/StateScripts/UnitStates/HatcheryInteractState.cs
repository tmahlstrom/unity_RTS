using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class HatcheryInteractState : UnitBaseState {

	public HatcheryInteractState (Unit unit, bool manualInit) : base(unit, manualInit){
		GetInPositionToInteract();
	}

	public override void UpdateState(){
		if (unit.hatcheryInteracter.performingInteract == false){
			if (unit.hatcheryInteracter.GOfocus && IsEngagedWithGO(unit.hatcheryInteracter.GOfocus)){
				PerformInteract(); 
			}
		}
	}

	private void GetInPositionToInteract(){
        if (unit.hatcheryInteracter.GOfocus){
			if (!IsEngagedWithGO(unit.hatcheryInteracter.GOfocus)){
				unit.mover.PrimaryInteractGameObjectAtDistance(unit.hatcheryInteracter.GOfocus, 0.9f);
                unit.animationManager.ChangeAnimation(RTS.EAnimation.Attack, false);
				// if (interactSound != null){
				// 	unit.StopCoroutine(interactSound); 
				// }
				//interactSound = unit.StartCoroutine(InteractSound(false));
                unit.animationManager.ChangeAnimation(RTS.EAnimation.Move, true);
			} else{
				PerformInteract();
			}
		}
	}

	private void PerformInteract (){
        unit.animationManager.ChangeAnimation(RTS.EAnimation.Move, false);
        unit.animationManager.ChangeAnimation(RTS.EAnimation.Attack, true);
		unit.hatcheryInteracter.BeginInteract(); 
		// if (interactSound != null){
		// 	unit.StopCoroutine(interactSound); 
		// }
		// interactSound = unit.StartCoroutine(InteractSound(true)); 
	}



	public override void MouseClickRight (GameObject hitObject, Vector3 hitPoint) {
		WorldObject wo = hitObject.gameObject.GetComponentInParent<WorldObject>();
		if (unit.hatcheryInteracter.CheckForRepeatCommand(hitObject)){
			if (wo){
				wo.TargetedByPlayer();
			}
			return;
		}
		if (unit.hatcheryInteracter.IssueInteractCommand(wo)){
			if (wo){
				wo.TargetedByPlayer(); 
			}
			GetInPositionToInteract();
            return;
        }
		base.MouseClickRight(hitObject, hitPoint);
	}

	public override void AnimationClimaxEvent(RTS.EAnimation state){
		if (state == RTS.EAnimation.Attack){
            unit.hatcheryInteracter.InteractExecution();
            SelfExitState(RTS.EAnimation.Attack);
		}
    }

	private IEnumerator InteractSound(bool toggle){
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
        unit.animationManager.ChangeAnimation(RTS.EAnimation.Attack, false);
		// if (interactSound != null){
		// 	unit.StopCoroutine(interactSound); 
		// }
		unit.StartCoroutine(InteractSound(false)); 
		unit.hatcheryInteracter.ResetHatcheryInteracter();
	}

	public override void SelfExitState(RTS.EAnimation state = 0){
		if (state == RTS.EAnimation.Attack){
			base.SelfExitState(state);
            unit.mover.ClearMovement();
            unit.SetUnitState(new IdleState(unit, false));
		}
	}


}
