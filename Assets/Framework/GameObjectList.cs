using UnityEngine;
using System.Collections;
using RTS;

public class GameObjectList : MonoBehaviour {

	private static bool created = false;
	public GameObject[] buildings;
	public GameObject[] units;
	public GameObject[] miscellaneous;
	public GameObject[] players;
	public GameObject[] enemyManagers;
	public GameObject[] enemyUnits;
	public GameObject[] items;
	public GameObject[] spawn; 
	public GameObject[] spawnEffects; 

	[Space(20)]
	public Transform dynamicObjects; 


	void Awake() {
		if(!created) {
			DontDestroyOnLoad(transform.gameObject);
			ResourceManager.SetGameObjectList(this);
			created = true;
		} else {
			Destroy(this.gameObject);
		}
	}

	void Start () {
		
	}
	
	void Update () {
	
	}

	public GameObject GetBuilding(string name) {
		for(int i = 0; i < buildings.Length; i++) {
			Building building = buildings[i].GetComponent< Building >();
			if(building && building.name == name) return buildings[i];
		}
		return null;
	}

	public GameObject GetUnit(string name) {
		for(int i = 0; i < units.Length; i++) {
			Unit unit = units[i].GetComponent< Unit >();
			if(unit && unit.name == name) return units[i];
		}
		return null;
	}

	public GameObject GetSpawn(string name) {
		for(int i = 0; i < spawn.Length; i++) {
			Spawn spawnGO = spawn[i].GetComponent< Spawn >();
			if(spawnGO && spawnGO.name == name) return spawn[i];
		}
		return null;
	}

	public GameObject GetSpawnEffect(string name) {
		for(int i = 0; i < spawnEffects.Length; i++) {
			if (spawnEffects [i].name == name){
				return spawnEffects [i];
			}
		}
		return null;
	}

	public GameObject GetItem(string name) {
		for(int i = 0; i < items.Length; i++) {
			if (items [i].name == name){
				return items [i];
			}
		}
		return null;
	}


	public GameObject GetEnemyManager(string name) {
		for(int i = 0; i < enemyManagers.Length; i++) {
			EnemyManager enemyManager = enemyManagers[i].GetComponent< EnemyManager >();
			if(enemyManager && enemyManager.name == name) return enemyManagers[i];
		}
		return null;
	}

	public GameObject GetEnemyUnit(string name) {
		for(int i = 0; i < enemyUnits.Length; i++) {
			Unit enemyUnit = enemyUnits[i].GetComponent< Unit >();
			if(enemyUnit && enemyUnit.name == name) return enemyUnits[i];
		}
		return null;
	}

	public GameObject GetMiscellaneous(string name) {
		foreach(GameObject go in miscellaneous) {
			if(go.name == name) return go;
		}
		return null;
	}

	public GameObject[] GetPlayers() {
		return players;
	}

	public GameObject[] GetEnemyManagers(){
		return enemyManagers; 
	}

	public GameObject[] GetEnemyUnits(){
		return enemyUnits;
	}
		

		
	public Transform GetDynamicObjects(){
		return dynamicObjects;
	}

}
