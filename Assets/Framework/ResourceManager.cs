using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RTS {
	public static class ResourceManager {



		public static float MinCameraHeight { get { return 10; } }
		public static float MaxCameraHeight { get { return 40; } }
		private static Texture2D healthyTexture, damagedTexture, criticalTexture;
		public static Texture2D HealthyTexture { get { return healthyTexture; } }
		public static Texture2D DamagedTexture { get { return damagedTexture; } }
		public static Texture2D CriticalTexture { get { return criticalTexture; } }

		private static Vector3 invalidPosition = new Vector3(-99999, -99999, -99999);
		public static Vector3 InvalidPosition { get { return invalidPosition; } }

		private static GUISkin selectBoxSkin;
		public static GUISkin SelectBoxSkin { get { return selectBoxSkin; } }

		public static void StoreSelectBoxItems(GUISkin skin, Texture2D healthy, Texture2D damaged, Texture2D critical) {
			selectBoxSkin = skin;
			healthyTexture = healthy;
			damagedTexture = damaged;
			criticalTexture = critical;
		}
			
		private static GameObjectList gameObjectList;

		public static void SetGameObjectList(GameObjectList objectList) {
			gameObjectList = objectList;
		}
		public static GameObject GetBuilding(string name) {
			return gameObjectList.GetBuilding(name);
		}

		public static GameObject GetUnit(string name) {
			return gameObjectList.GetUnit(name);
		}

		public static GameObject GetSpawn (string name){
			return gameObjectList.GetSpawn (name); 
		}

		public static GameObject GetSpawnEffect (string name){
			return gameObjectList.GetSpawnEffect (name); 
		}

		public static GameObject GetItem (string name){
			return gameObjectList.GetItem (name);
		}

		public static GameObject GetEnemyManager(string name) {
			return gameObjectList.GetEnemyManager(name);
		}

		public static GameObject GetEnemyUnit(string name) {
			return gameObjectList.GetEnemyUnit(name);
		}

		public static GameObject GetMiscellaneous(string name) {
			return gameObjectList.GetMiscellaneous(name);
		}

		public static GameObject[] GetPlayers() {
			return gameObjectList.GetPlayers ();
		}

		public static GameObject[] GetEnemyManagers() {
			return gameObjectList.GetEnemyManagers();
		}

		public static GameObject[] GetPlayersWithCanvases() {
			return gameObjectList.GetPlayers();
		}

		public static GameObject[] GetEnemyUnits() {
			return gameObjectList.GetEnemyUnits();
		}



		public static Transform GetDynamicObjects() {
			return gameObjectList.GetDynamicObjects();
		}
	}



}