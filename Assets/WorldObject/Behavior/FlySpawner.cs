using System.Collections;
using UnityEngine;
using RTS;

public class FlySpawner : Spawner {

	protected override void Awake () {
		base.Awake ();


	}

	protected override void Start () {
		base.Start ();
	}
	
	protected override void Update () {
		base.Update (); 
	}

    protected override void CreateSpawn (){
        base.CreateSpawn();
		for (int i = 0; i < paramManager.SpawnPerCreate; i++) {
            CreateFlySpawn(paramManager.SpawnablesList[0]);
		}
	}

    protected virtual void CreateFlySpawn (GameObject flyGO){
        GameObject newSpawnGameObject = (GameObject)Instantiate (flyGO, DetermineSpawnPoint (), Quaternion.Euler (-90, 0, 0));;
		if (newSpawnGameObject != null) {
			newSpawnGameObject.transform.SetParent (spawnHolder.transform);
			newSpawnGameObject.SetActive (true);
			Spawn spawn = newSpawnGameObject.GetComponent< Spawn > ();
			TellSpawnWhoMadeIt (spawn);
			if (domeCenter != Vector3.zero){
				spawn.SavePushDirection(domeCenter);
			}
		}
	}

	private void TellSpawnWhoMadeIt (Spawn spawn){
		spawn.RememberWhoMadeMe (worldObject); 
	}


	protected override Vector3 DetermineSpawnPoint (){
        SpawnPoint spawnPointObject = GetComponentInChildren<SpawnPoint>();
        if (spawnPointObject){
            return spawnPointObject.transform.position;
        }
		Vector3 almostSpawnGenerationPoint = this.transform.position;
		almostSpawnGenerationPoint.y += 1.5f;
		almostSpawnGenerationPoint += transform.forward * 0.5f;
		spawnGenerationPoint = almostSpawnGenerationPoint;
		return spawnGenerationPoint; 
	}

	public override void InitiateSpawnerSpecial(){ 
		base.InitiateSpawnerSpecial (); 
		if (unit.paramManager.ManaPoints >= unit.paramManager.MaxManaPoints && this != null) {
            StartCoroutine("Flurry");
		}
	}


	public IEnumerator Flurry(){
		int shotCost = 25;
        CreateFlySpawn (paramManager.SpawnablesList[0]);	
		unit.ChangeMana (-shotCost);
		yield return new WaitForSeconds (0.3f); 
        CreateFlySpawn (paramManager.SpawnablesList[0]);
		unit.ChangeMana (-shotCost);		
		yield return new WaitForSeconds (0.3f); 
        CreateFlySpawn (paramManager.SpawnablesList[0]);
		unit.ChangeMana (-shotCost);
		yield return new WaitForSeconds (0.3f); 
        CreateFlySpawn (paramManager.SpawnablesList[0]);
		unit.ChangeMana (-shotCost);
	}
}
