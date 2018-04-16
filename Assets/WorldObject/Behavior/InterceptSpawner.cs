using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class InterceptSpawner : Spawner {


	protected override void Awake () {
		base.Awake ();
	}


	protected override void Start () {
		base.Start ();
		CreateSpawn (); 
		StartCoroutine ("PeriodicallyConsiderIncreasingMana");
	}

	protected override void Update () {
        	base.Update();
	}




	protected virtual IEnumerator PeriodicallyConsiderIncreasingMana (){
		while (this != null) {
			//if (worldObject.GetEnemyTargetsInRange (15).Count >= 1) {
                		unit.ChangeMana(1);
				yield return new WaitForSeconds (0.8f);
			// } else {
			// 	yield return new WaitForSeconds (3.0f); 
			//}
		}
	}

	protected override void CreateSpawn () {
        	base.CreateSpawn(); 
        	for (int i = 0; i < paramManager.SpawnPerCreate; i++) {
           		GameObject spawnGO = (GameObject)Instantiate (paramManager.SpawnablesList[0], DetermineSpawnPoint (), Quaternion.Euler (-90, 0, 0));
			spawnGO.transform.SetParent (spawnHolder.transform);
			spawnGO.SetActive (true);
			Spawn spawn = spawnGO.GetComponent< Spawn > ();
			spawn.RememberWhoMadeMe (worldObject);
		}
	}

	protected override Vector3 DetermineSpawnPoint(){
        	spawnGenerationPoint = transform.position;
        	Orb orb = transform.parent.GetComponentInChildren<Orb>();
        	if (orb){
            		spawnGenerationPoint = orb.transform.position; 
        	}
		return spawnGenerationPoint;
	}

	protected override void CreateSpawnsTransform (){
		spawnHolder = new GameObject();
        	spawnHolder.name = worldObject.name + " Spawns";
		//SpawnHolder.transform.position = DetermineSpawnPoint (); 
        	Orb orb = transform.parent.GetComponentInChildren<Orb>();
        	if (orb){
            		spawnHolder.transform.SetParent (orb.gameObject.transform); 
        	}
	}

	public override bool ReadyToSpawnerSpecial(){
		if (unit.paramManager.ManaPoints >= (unit.paramManager.MaxManaPoints/10)){
			return true; 
		}
        	return false; 
	}

	public override void InitiateSpawnerSpecial(){ 
		specialProcessActive = true; 
        	StartCoroutine("UltimateIntercept");
		SpecialSound(true); 
	}

	public override void ConcludeSpawnerSpecial(){
		specialProcessActive = false;
		SpecialSound(false);
    }

	public IEnumerator UltimateIntercept(){
		InterceptSpawn interceptSpawn = GetComponentInChildren<InterceptSpawn> ();
        	//interceptSpawn.laserCooldown = paramManager.SpawnerSpecialEffectRate;
		interceptSpawn.laserEnabled = true; 
        	while (unit.paramManager.ManaPoints > 0.0f && specialProcessActive) {
			unit.ChangeMana (-1);
            		yield return new WaitForSeconds (paramManager.SpawnerSpecialDurationFactor);
		}
        	unit.unitState.SelfExitState(RTS.EAnimation.SpawnerSpecial);
		// interceptSpawn.laserCooldown = paramManager.SpawnCooldown;
		interceptSpawn.laserEnabled = false; 
    }

	private void SpecialSound(bool toggle){
		if (worldObject.audioSource){
			if (paramManager.SpecialAudioClip){
				if (toggle == true){
					AudioManager.Instance.Play(paramManager.SpecialAudioClip, worldObject.audioSource);
				} else {
					if (worldObject.audioSource.clip == paramManager.SpecialAudioClip){
						worldObject.audioSource.Stop(); 
					}
				}
			}
		}
	}
}
