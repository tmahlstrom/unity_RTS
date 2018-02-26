using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class PatrolStateS : SpawnBaseState {

	private Vector3 patrolPoint; 
	private Coroutine updatePatrolPoint;
	private Coroutine positionOnSurface; 
	private Coroutine lookForTargets; 
	private float patrolMoveSpeed; 

	// Use this for initialization
	public PatrolStateS (Spawn spawn, bool manualInit) : base(spawn, manualInit){
		spawn.StopAllCoroutines();
		patrolPoint = spawn.FindPatrolPoint ();

		lookForTargets = spawn.StartCoroutine(LookForTargets());
		updatePatrolPoint = spawn.StartCoroutine(UpdatePatrolPoint());
		positionOnSurface = spawn.StartCoroutine(PositionOnSurface());
		
	}

	public override void UpdateState(){
		spawn.MoveForward(spawn.paramManager.PatrolVelocity);
		AugmentRotation();
		if (spawn.targetCollider != null){
			spawn.SetSpawnState(new ApproachStateS(spawn, false));
		}
	}

	private void AugmentRotation(){
		if (spawn.AugmentRotationNeeded(patrolPoint)){
			spawn.AugmentRotationSpeed (patrolPoint); 
		} 
	}


	private IEnumerator UpdatePatrolPoint(){
		while (this != null){
			if (spawn.CheckForArrival(patrolPoint)){
				patrolPoint = spawn.FindPatrolPoint ();
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

	private IEnumerator LookForTargets(){
		while (this != null){
			spawn.LookForTarget();
			yield return mediumWait; 
		}
	}


	public override void ExitRoutine(SpawnBaseState state){

	}

}
