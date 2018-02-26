using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class AntSpawner : Spawner {

     
    WaitForSeconds shortWait = new WaitForSeconds(0.5f);


	protected override void Awake () {
		base.Awake (); 
	}

	protected override void Start () {
		base.Start ();
	}

	protected override void Update () {
        base.Update(); 
	}


	protected override void CreateSpawn (){
        base.CreateSpawn(); 
        for (int i = 0; i < paramManager.SpawnPerCreate; i++) {
            CreateAntSpawn(paramManager.SpawnablesList[0]);
		}
	}

    public virtual void CreateAntSpawn (GameObject spawnGO){
        GameObject newSpawnGameObject = Instantiate (spawnGO, DetermineSpawnPoint (), Quaternion.Euler (-90, 0, 0));
		if (newSpawnGameObject != null) {
			newSpawnGameObject.transform.SetParent (spawnHolder.transform);
			newSpawnGameObject.SetActive (true);
			Spawn spawn = newSpawnGameObject.GetComponent< Spawn > ();
			TellSpawnWhoMadeIt (spawn);
		}
	}

	private void TellSpawnWhoMadeIt (Spawn spawn){
		spawn.RememberWhoMadeMe (worldObject); 
	}

	protected override void AddReleaseForces (Spawn spawn){

	}

	protected override Vector3 DetermineSpawnPoint (){
		Vector3 almostSpawnGenerationPoint = this.transform.position;
        almostSpawnGenerationPoint.y = transform.position.y; 
		spawnGenerationPoint = almostSpawnGenerationPoint;
		return spawnGenerationPoint; 
	}

	public override void InitiateSpawnerSpecial(){ 

	}

}
