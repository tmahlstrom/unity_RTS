using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class HealSpawner: Spawner {

	protected override void Awake () {
		base.Awake ();
	}

	protected override void Start () {
		base.Start ();
	}

	protected override void Update () {
		base.Update();
	}

	protected override void CreateSpawn () {
        	base.CreateSpawn();
        	for (int i = 0; i < paramManager.SpawnPerCreate; i++) {
			CreateHealSpawn (HealSpawnType.Level_1, null);
			if (unit) {
				unit.ChangeMana (6); 
			}
		}
	}

	protected virtual void CreateHealSpawn (HealSpawnType type, WorldObject targetWO){
		GameObject newSpawnGameObject = null;
		switch (type) {
		case HealSpawnType.Level_1:
			newSpawnGameObject = (GameObject)Instantiate (ResourceManager.GetSpawn ("HealSpawnLevel_1"), DetermineSpawnPoint (), Quaternion.Euler (-90, 0, 0));
			break;
		case HealSpawnType.Level_2:
			newSpawnGameObject = (GameObject)Instantiate (ResourceManager.GetSpawn ("HealSpawnLevel_2"), DetermineSpawnPoint (), Quaternion.Euler (-90, 0, 0));
			break;
		}
		if (newSpawnGameObject != null) {
			newSpawnGameObject.transform.SetParent (spawnHolder.transform);
			newSpawnGameObject.SetActive (true);
			Spawn spawn = newSpawnGameObject.GetComponent< Spawn > ();
			TellSpawnWhoMadeIt (spawn);
			if (targetWO != null) {
				spawn.RegisterTargetAssignment (targetWO); 
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



	protected override void AddReleaseForces (Spawn spawn){
		spawn.GetComponent<Rigidbody> ().AddForce (0, releaseForce, releaseForce);
	}

	public override void InitiateSpawnerSpecial(){ 
		base.InitiateSpawnerSpecial (); 
		if (unit.paramManager.ManaPoints >= unit.paramManager.MaxManaPoints && this != null) {
            		StartCoroutine("HealAll");
		}
	}


	public IEnumerator HealAll(){
		unit.ChangeMana (-100); 
		if (worldObject.player) {
			Units unitsWrapper = worldObject.player.GetComponentInChildren<Units> ();
			Unit[] units = null; 
			if (unitsWrapper) {
				units = unitsWrapper.GetComponentsInChildren<Unit> (); 
			}
			foreach (Unit unit in units) {
				if (unit != this.unit) {
					CreateHealSpawn (HealSpawnType.Level_2, unit.GetComponent<WorldObject> ());
					yield return new WaitForSeconds (0.3f);
				}
			}
		}
	}

}
