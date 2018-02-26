using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class IdleStateB : BuildingBaseState {

	public IdleStateB (Building building, bool manualInit) : base (building, manualInit) { 
		//building.StopAllCoroutines(); 
	}

	public override void UpdateState(){
		if (building.spawner){
			if (building.spawner.ReadyToBeginSpawning()){
				building.spawner.SpawnClimaxEvent(); 
				building.spawner.ConcludeSpawnProcess(); //when an animation is made for this, make it work like the unit spawner 
			}
		}

	}

}
