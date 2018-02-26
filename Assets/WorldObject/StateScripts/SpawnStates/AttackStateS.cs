using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class AttackStateS : SpawnBaseState {

	private Coroutine lookForNewTargets;
	private float attackMoveSpeed;	
	
	public AttackStateS (Spawn spawn, bool manualInit) : base(spawn, manualInit){
		spawn.StopAllCoroutines();
		lookForNewTargets = spawn.StartCoroutine(LookForNewTargets());
		if (spawn.targetCollider){
			attackMoveSpeed = spawn.paramManager.AttackMoveVelocity; 
		}
	}

	public override void UpdateState(){
        if (spawn.targetCollider != null && spawn.targetCollider.gameObject.activeInHierarchy){ //best way to check if the enemy unit is still alive? 
			spawn.MoveForward(attackMoveSpeed);
			spawn.AttackMotion(); 
			AugmentRotation();
        } else {
			spawn.SetSpawnState(new PatrolStateS(spawn, false));
		}
	}

	private IEnumerator LookForNewTargets(){
		while (this != null){
			spawn.LookForTarget();
			yield return mediumWait; 
		}
	}

	private void AugmentRotation(){
		if (spawn.AugmentRotationNeeded(spawn.targetCollider.transform.position)){
			spawn.AugmentRotationSpeed (spawn.targetCollider.transform.position); 
		} 
	}


	public override void ExitRoutine(SpawnBaseState state){

	}


}
