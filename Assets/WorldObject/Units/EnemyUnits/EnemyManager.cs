using UnityEngine;
using System.Collections;
using RTS;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

	public string username;
	public Color teamColor;

	private int spawnPointCounter; 
	EnemyElevator[] enemyElevators; 
	EnemyUnits enemyUnits;
	SpawnPoint[] spawnPoints; 

	List<GameObject> unitsToSend = new List<GameObject> (); 

	WaveData waveData; 


	private void Awake(){
		waveData = GetComponent<WaveData> (); 
		enemyUnits = GetComponentInChildren< EnemyUnits > ();
		enemyElevators = GetComponentsInChildren<EnemyElevator> (); 
		spawnPoints = GetComponentsInChildren<SpawnPoint>(); 
	}

	private void Start(){
		StageManager.Instance.listOfEnemyManagers.Add (this);
	}

	public void SendWave(int waveNumber){
        if (waveData){
    		unitsToSend = waveData.GetWaveObjectList(waveNumber);
    		if (unitsToSend != null && unitsToSend.Count > 0) {
				print ("starting wave #" + waveNumber);
    			StartCoroutine ("SendNextWave"); 
    		}
        }
		if (waveData == null || unitsToSend == null || (unitsToSend != null && unitsToSend.Count == 0)){
			Debug.Log("victory/scene change que");
		}
	}

	private IEnumerator SendNextWave (){
		int elevatorsToRaise = DetermineElevatorsToRaise();
		RaiseElevators (elevatorsToRaise); 
		yield return new WaitForSeconds (5.0f);
		foreach (GameObject objectToSpawn in unitsToSend) {
			Spawn (objectToSpawn.name, FindSpawnPoint ());
		}
		spawnPointCounter = 0; 
		yield return new WaitForSeconds (7.0f);
		LowerAllElevators ();
	}

	private int DetermineElevatorsToRaise(){
		if (enemyElevators.Length == 1){
			return 1; 
		}
		int eleToRaise;
		if (unitsToSend.Count == 1 || unitsToSend.Count == 3){
			eleToRaise = 1; 
		} else if (unitsToSend.Count == 2) {
			eleToRaise = 2; 
		} else {
			eleToRaise = 3; 
		}
		return eleToRaise; 
	}

	private Vector3 FindSpawnPoint(){
		int numberOfEnemies  = unitsToSend.Count; 
		SpawnPoint spawnPoint = null;
		if (numberOfEnemies == 2 && spawnPointCounter == 0 && enemyElevators.Length > 1){
			spawnPointCounter += 3; 
		}
		if (numberOfEnemies == 2 && spawnPointCounter == 0 && enemyElevators.Length == 1){
			spawnPointCounter += 1; 
		}
		spawnPoint = spawnPoints[spawnPointCounter];
		spawnPointCounter++;
		return spawnPoint.gameObject.transform.position;
	}


	private void RaiseElevators (int numberOfElevators){
		if (numberOfElevators == 1){
			enemyElevators[0].RaiseElevator();
		}
		if (numberOfElevators == 2){
			enemyElevators[1].RaiseElevator();
			enemyElevators[2].RaiseElevator();
		}
		if (numberOfElevators == 3){
			foreach (EnemyElevator elevator in enemyElevators) {
				elevator.RaiseElevator ();
			}
		}		
	}

	private void LowerAllElevators (){
		foreach (EnemyElevator elevator in enemyElevators) {
			elevator.LowerElevator ();
		}
	}

	public void Spawn(string nameOfObjectToSpawn, Vector3 whereToSpawn){
		GameObject objectToSpawn = (GameObject)Instantiate (ResourceManager.GetEnemyUnit (nameOfObjectToSpawn), whereToSpawn, this.transform.rotation);
		objectToSpawn.transform.parent = enemyUnits.transform;
		objectToSpawn.SetActive (true);
	}

}
