using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class ApproachStateS : SpawnBaseState {
	private Vector3 approachPoint; 

	private Coroutine updateApproachPoint;
	private Coroutine positionOnSurface; 
	private Coroutine lookForNewTargets; 
	private Coroutine considerAttacking; 
	private Coroutine considerPatrolling; 
	private float approachSpeed; 


	public ApproachStateS (Spawn spawn, bool manualInit) : base(spawn, manualInit){
		spawn.StopAllCoroutines();
		if (spawn.targetCollider){
			approachSpeed = spawn.paramManager.ApproachMoveVelocity; 
			//approachPoint = spawn.FindOffCenterApproachPoint(spawn.targetCollider.transform.position); 
		}
		updateApproachPoint = spawn.StartCoroutine(UpdateApproachPoint());
		positionOnSurface = spawn.StartCoroutine(PositionOnSurface());
		lookForNewTargets = spawn.StartCoroutine(LookForNewTargets());
		considerAttacking = spawn.StartCoroutine(ConsiderAttacking());

	}

	public override void UpdateState(){
		spawn.MoveForward(approachSpeed);
		AugmentRotation(); 
        if (spawn.targetCollider == null || spawn.targetCollider.gameObject.activeInHierarchy == false){
            if (!spawn.AbleToFindNewTarget()){
				spawn.SetSpawnState(new PatrolStateS(spawn, false));
			}
        } else if (spawn.InRangeForAtack()){
			spawn.SetSpawnState(new AttackStateS(spawn, false));
		}

	}

	private void AugmentRotation(){
		if (spawn.AugmentRotationNeeded(approachPoint)){
			spawn.AugmentRotationSpeed (approachPoint); 
		} 
	}

	private IEnumerator UpdateApproachPoint(){
		while (this != null){
			if (spawn.targetCollider){
				approachPoint = spawn.FindOffCenterApproachPoint(spawn.targetCollider.transform.position); 
			}
			yield return mediumWait; 
		}
	}

	private IEnumerator PositionOnSurface(){
		while (this != null){
			spawn.PositionOnSurface(); 
			yield return shortWait;
		}
	}

	private IEnumerator LookForNewTargets(){
		while (this != null){
			spawn.LookForTarget();
			yield return mediumWait; 
		}
	}

	private IEnumerator ConsiderAttacking(){
		while (this != null){
			// if (spawn.InRangeForAtack()){
			// 	spawn.SetSpawnState(new AttackStateS(spawn, false));
			// }
			yield return mediumWait; 
		}

	}



	public override void ExitRoutine(SpawnBaseState state){

	}




}
