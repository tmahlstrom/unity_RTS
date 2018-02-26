using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushedStateS : SpawnBaseState {
	private float pushedMoveSpeed = 0.01f;
	private Vector3 pushPoint; 
	private float startTime;
	private float pushDuration = 1f; 

	private float i; 

	public PushedStateS (Spawn spawn, bool manualInit) : base(spawn, manualInit){
		spawn.StopAllCoroutines();
		startTime = Time.time; 
		pushPoint = spawn.UpdatePushDirection(spawn.transform);
		GetPushed(); 
	}

	private void GetPushed(){
		
	}

	public override void UpdateState(){
		i = pushedMoveSpeed * (1 / ((Time.time - startTime) + 0.5f));
		spawn.transform.position = Vector3.Lerp(spawn.transform.position, pushPoint, 1 - (1 - i)); 
		if (Time.time - startTime > pushDuration){
			spawn.SetSpawnState(new PatrolStateS(spawn, false));
		}
	}

}
