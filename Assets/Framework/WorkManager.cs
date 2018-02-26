using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine.AI;

namespace RTS {
	public static class WorkManager {

        public static string[] rightClickable = new string[] {"Ground", "Unit", "Building", "Item"}; 


		public static GameObject FindHitObject(Vector3 origin) {
			Ray ray = Camera.main.ScreenPointToRay(origin);
			RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 50.0f, LayerMask.GetMask(rightClickable))) return hit.collider.gameObject;
			return null;
		}


		public static RaycastHit FindNonSelfHitObject(Vector3 origin, WorldObject self) {
			Ray ray = Camera.main.ScreenPointToRay(origin);
			RaycastHit[] hits = Physics.RaycastAll(ray, 50.0f, LayerMask.GetMask(rightClickable));
			List<RaycastHit> listOfNonSelfHits = new List<RaycastHit>();
			for (int i = 0; i < hits.Length; i++) {
				if (hits[i].transform.gameObject.tag != "InvisibleToClick"){
					WorldObject colliderWorldObject = hits[i].transform.GetComponentInParent<WorldObject> ();
					if (colliderWorldObject == null || (colliderWorldObject && colliderWorldObject != self)) {
						listOfNonSelfHits.Add (hits [i]); 
					}
				}
			}
			RaycastHit closestHit = new RaycastHit(); 
			float closestDistance = Mathf.Infinity;
			foreach (RaycastHit hit in listOfNonSelfHits) {
				float distance = Vector3.Distance (hit.point, origin);
				if (hit.collider.gameObject.tag == "Environment") {
					distance += 100f;
				}
				if (distance < closestDistance) {
					closestDistance = distance; 
					closestHit = hit; 
				}
			}
			return closestHit; 
		}


		public static Vector3 FindHitPoint(Vector3 origin) {
			Ray ray = Camera.main.ScreenPointToRay(origin);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit)) return hit.point;
			return ResourceManager.InvalidPosition;
		}

		public static Collider FindEnemyTargetCollider (GameObject hitGO, WorldObject hitterWO){
			Collider targetCol = null;
			WorldObject hitWO = hitGO.GetComponentInParent<WorldObject> ();
				if (hitterWO && hitWO && WorkManager.AreWorldObjectsOnSameTeam(hitWO, hitterWO) == false) {
				Target attackTarget = hitWO.GetComponentInChildren<Target>();
					if (attackTarget){
						targetCol = attackTarget.GetComponent<Collider>();
					}
				}
			return targetCol; 
		}



		//**************************************
		//BEGIN CHECK IF OBJECTS ARE ON SAME TEAM
		//**************************************
		public static bool? AreWorldObjectsOnSameTeam (WorldObject object1, WorldObject object2){
			if (object1 != null && object2 != null) {
				if (object1.player != null) {
					if (object2.player != null) {
						if (object1.player.username == object2.player.username) {
							return true; 
						}
						if (object1.player.username != object2.player.username) {
							return false; 
						}
					}
					if (object2.enemyManager ?? null) {
						return false;
					}
				}
				if (object1.enemyManager != null) {
					if (object2.player != null) {
						return false;
					}
					if (object2.enemyManager != null) {
						if (object1.enemyManager.username == object2.enemyManager.username) {
							return true;
						}
						if (object1.enemyManager.username != object2.enemyManager.username) {
							return false;
						}
					}
				}
			}
			return null;
		}
		//**************************************
		//END CHECK IF OBJECTS ARE ON SAME TEAM
		//**************************************


		//**************************************
		//BEGIN DETERMINE ITEM FROM A LIST
		//**************************************
		public static Collider DetermineNearestCollider(WorldObject seeker, List<Collider> listOfColliders){
			Collider nearestCol = null;
			float comparisonVar = Mathf.Infinity;
			foreach (Collider potentialNearestCol in listOfColliders) {
				if (potentialNearestCol) {
					Vector3 distance = potentialNearestCol.transform.position - seeker.transform.position;
					float sqrDistance = distance.sqrMagnitude;
					if (sqrDistance < comparisonVar) {
						nearestCol = potentialNearestCol;
						comparisonVar = sqrDistance;
					}
				}
			}
			return nearestCol;
		}

        public static Collider DetermineNearestCollider(Vector3 viewPoint, List<Collider> listOfColliders){
            Collider nearestCol = null;
            float comparisonVar = Mathf.Infinity;
            foreach (Collider potentialNearestCol in listOfColliders) {
                if (potentialNearestCol) {
                    Vector3 distance = potentialNearestCol.transform.position - viewPoint;
                    float sqrDistance = distance.sqrMagnitude;
                    if (sqrDistance < comparisonVar) {
                        nearestCol = potentialNearestCol;
                        comparisonVar = sqrDistance;
                    }
                }
            }
            return nearestCol;
        }

        public static Collider DetermineNearestCollider(GameObject seeker, List<Collider> listOfColliders){
            Collider nearestCol = null;
            float comparisonVar = Mathf.Infinity;
            foreach (Collider potentialNearestCol in listOfColliders) {
                if (potentialNearestCol) {
                    Vector3 distance = potentialNearestCol.transform.position - seeker.transform.position;
                    float sqrDistance = distance.sqrMagnitude;
                    if (sqrDistance < comparisonVar) {
                        nearestCol = potentialNearestCol;
                        comparisonVar = sqrDistance;
                    }
                }
            }
            return nearestCol;
        }

		public static Collider DetermineNearestVisibleCollider(WorldObject seeker, List<Collider> listOfColliders){
			Collider nearestVisibleCol = null;
			float comparisonVar = Mathf.Infinity;
			foreach (Collider potentialNearestVisibleCol in listOfColliders) {
				if (potentialNearestVisibleCol && seeker && Physics.Raycast (seeker.transform.position, potentialNearestVisibleCol.transform.position, Mathf.Infinity)) {
					Vector3 distance = potentialNearestVisibleCol.transform.position - seeker.transform.position;
					float sqrDistance = distance.sqrMagnitude;
					if (sqrDistance < comparisonVar) {
						nearestVisibleCol = potentialNearestVisibleCol;
						comparisonVar = sqrDistance;
					}
				}
			}
			return nearestVisibleCol;
		}

		public static Collider DetermineLowestHPTargetInRange (WorldObject seeker, List<Collider> listOfColliders){
			Collider lowestHPCol = null; 
			float comparisonVar = 1.0f;
			foreach (Collider potentialLowestHPCol in listOfColliders) {
				WorldObject potentialLowestHPWO = potentialLowestHPCol.GetComponentInParent<WorldObject> (); 
				if (potentialLowestHPWO && potentialLowestHPWO.paramManager.HealthPercentage != 0) {
					if (potentialLowestHPWO.paramManager.HealthPercentage < comparisonVar) {
						lowestHPCol = potentialLowestHPCol;
						comparisonVar = potentialLowestHPWO.paramManager.HealthPercentage;
					}
				}
			}
			return lowestHPCol;
		}
		//**************************************
		//END FIND NEAREST COLLIDER IN LIST
		//**************************************


        //********************************
        //BEGIN NEW-TYPE DETERMINE OBJECTS IN RANGE
        //********************************
        static List<Collider> playerTargetsInRange = new List<Collider>();
		static List<Collider> playerUnitTargetsInRange = new List <Collider>();
		static List<Collider> enemyTargetsInRange = new List<Collider>();
		static List<Collider> enemySpawnTargetsInRangeNEW = new List<Collider>();


		

        public static List<Collider> DeterminePlayerTargetsInRange(Vector3 viewPoint, float range){
            playerTargetsInRange.Clear();
            int playerUnitLayerMask = 29;
            int playerBuildingLayerMask = 30; 
            int playerTargetLayerMask = (1 << playerUnitLayerMask) | (1 << playerBuildingLayerMask); 
            Collider[] playerCollidersInRange = Physics.OverlapSphere (viewPoint, range, playerTargetLayerMask);
            foreach (Collider col in playerCollidersInRange) {
                playerTargetsInRange.Add (col);
            }
            return playerTargetsInRange; 
        }

        public static Collider DetermineNearestPlayerTargetColliderInRange (Vector3 viewPoint, float range){
            return (DetermineNearestCollider(viewPoint, DeterminePlayerTargetsInRange(viewPoint, range)));
        }
		        public static List<Collider> DeterminePlayerUnitTargetsInRange(Vector3 viewPoint, float range){
            playerUnitTargetsInRange.Clear();
            int playerUnitLayerMask = 29;
            int playerTargetLayerMask = (1 << playerUnitLayerMask);
            Collider[] playerUnitCollidersInRange = Physics.OverlapSphere (viewPoint, range, playerTargetLayerMask);
            foreach (Collider col in playerUnitCollidersInRange) {
                playerUnitTargetsInRange.Add (col);
            }
            return playerUnitTargetsInRange; 
        }

        public static Collider DetermineNearestPlayerUnitTargetColliderInRange (Vector3 viewPoint, float range){
            return (DetermineNearestCollider(viewPoint, DeterminePlayerUnitTargetsInRange(viewPoint, range)));
        }

		public static List<Collider> DetermineEnemyTargetsInRange(Vector3 viewPoint, float range){
            enemyTargetsInRange.Clear();
            int enemyUnitLayerMask = 26;
            int enemyBuildingLayerMask = 27; 
            int enemyTargetLayerMask = (1 << enemyUnitLayerMask) | (1 << enemyBuildingLayerMask); 
            Collider[] enemyCollidersInRange = Physics.OverlapSphere (viewPoint, range, enemyTargetLayerMask);
            foreach (Collider col in enemyCollidersInRange) {
				int bLayerMask = 14;
				int barrierLayerMask = (1 << bLayerMask);
				if (Physics.Raycast(viewPoint, col.transform.position, range, barrierLayerMask) == false){
                	enemyTargetsInRange.Add (col);
				}
            }
            return enemyTargetsInRange; 
        }

        public static Collider DetermineNearestEnemyTargetColliderInRange (Vector3 viewPoint, float range){
            return (DetermineNearestCollider(viewPoint, DetermineEnemyTargetsInRange(viewPoint, range)));
        }

		public static List<Collider> DetermineEnemySpawnTargetsInRange(Vector3 viewPoint, float range){
            enemySpawnTargetsInRangeNEW.Clear();
            int enemySpawnLayerMask = 28;
			int enemyTargetLayerMask = (1 << enemySpawnLayerMask);
            Collider[] enemyCollidersInRange = Physics.OverlapSphere (viewPoint, range, enemyTargetLayerMask);
            foreach (Collider col in enemyCollidersInRange) {
                enemySpawnTargetsInRangeNEW.Add (col);
            }
            return enemySpawnTargetsInRangeNEW; 
        }

        public static Collider DetermineNearestEnemySpawnTargetColliderInRange (Vector3 viewPoint, float range){
            return (DetermineNearestCollider(viewPoint, DetermineEnemySpawnTargetsInRange(viewPoint, range)));
        }


        //********************************
        //END NEW-TYPE DETERMINE OBJECTS IN RANGE
        //********************************
			
		//********************************
		//BEGIN DETERMINE OBJECTS IN RANGE
		//********************************
		static List<Collider> enemyUnitTargetsInRange;
		static List<Collider> allyUnitTargetsInRange; 
		static List<Collider> enemySpawnTargetsInRange;
		static List<Collider> enemyTargetsInRangeOLD; 
		static Target[] selfTargets;

		public static List<Collider> DetermineEnemySpawnTargetsInRange(WorldObject seeker, float range){
			enemySpawnTargetsInRange = seeker.enemySpawnTargetsInRange; 
			enemySpawnTargetsInRange.Clear (); 
			Collider[] collidersInRange = Physics.OverlapSphere (seeker.transform.position, range);
			foreach (Collider col in collidersInRange) {
				Spawn spawn = col.gameObject.GetComponentInParent<Spawn> ();
				if (spawn != null) {
					if (AreWorldObjectsOnSameTeam (seeker, spawn.worldObject) == false) {
							enemySpawnTargetsInRange.Add (col);
					}
				}
			}
			return enemySpawnTargetsInRange;
		}

		public static List<Collider> DetermineEnemyUnitTargetsInRange(WorldObject seeker, float range){
			enemyUnitTargetsInRange = seeker.enemyUnitTargetsInRange; 
			enemyUnitTargetsInRange.Clear (); 
            int enemyUnitTargetLayerMask = 1<<26;
            Collider[] enemyUnitCollidersInRange = Physics.OverlapSphere (seeker.transform.position, range, enemyUnitTargetLayerMask);
            foreach (Collider col in enemyUnitCollidersInRange) {
                enemyUnitTargetsInRange.Add (col);
				//	//WorldObject tarWorldObject = tar.GetComponentInParent<WorldObject> ();
				//	//if (tarWorldObject){
				//	//	if (AreWorldObjectsOnSameTeam (seeker, tarWorldObject) == false) {
				//	//		enemyUnitTargetsInRange.Add (col);
				//	//	}
				//	//}
				//}
			}
			return enemyUnitTargetsInRange;
		}

		public static List<Collider> DetermineEnemyWorldObjectsInRange(WorldObject seeker, float range){
			enemyTargetsInRangeOLD = seeker.enemyTargetsInRange; 
			enemyTargetsInRangeOLD.Clear (); 
			Collider[] collidersInRange = Physics.OverlapSphere (seeker.transform.position, range);
			foreach (Collider col in collidersInRange) {
				WorldObject worldObject = col.gameObject.GetComponentInParent<WorldObject> ();
				if (worldObject != null) {
					if (AreWorldObjectsOnSameTeam (seeker, worldObject) == false) {
						enemyTargetsInRangeOLD.Add (col);
					}
				}
			}
			return enemyTargetsInRangeOLD;
		}

		public static List<Collider> DetermineAllyUnitTargetsInRange(WorldObject seeker, float range){
			allyUnitTargetsInRange = seeker.allyUnitTargetsInRange; 
			allyUnitTargetsInRange.Clear (); 
			Collider[] collidersInRange = Physics.OverlapSphere (seeker.transform.position, range);
			selfTargets = seeker.selfTargets;
			foreach (Collider col in collidersInRange) {
				Unit unit = col.GetComponentInParent<Unit> (); 
				Target tar = col.gameObject.GetComponent<Target> ();
				if (tar && unit){
					WorldObject tarWorldObject = tar.GetComponentInParent<WorldObject> ();
					if (tarWorldObject) {
						if (AreWorldObjectsOnSameTeam (seeker, tarWorldObject) == true) {
							allyUnitTargetsInRange.Add (col);
						}
					}
				}
			}
			if (selfTargets != null) {
				foreach (Target tar in selfTargets) {
					Collider tarcol = tar.GetComponentInParent<Collider> ();
					allyUnitTargetsInRange.Remove (tarcol);
				}
			}
			return allyUnitTargetsInRange;
		}

		//**************************
		//END DETERMINE OBJECTS IN RANGE
		//**************************

		//**************************
		//BEGIN VARIOUS GENERAL CALCULATIONS
		//**************************

		public static bool DetermineIfNearNavObstacle (Vector3 seekerPoint, float range){
			Collider[] collidersInRange = Physics.OverlapSphere (seekerPoint, range);
			foreach (Collider col in collidersInRange) {
				NavMeshObstacle obstacle = col.gameObject.GetComponentInParent<NavMeshObstacle> ();
				if (obstacle != null) {
					return true;
				}
			}
			return false; 
		}

		public static bool IsTargetColliderStillAlive (Collider targetCol){
			if (targetCol){
				WorldObject wo = targetCol.GetComponentInParent<WorldObject>();
				if (wo && !wo.paramManager.IsDead){
					return true; 
				}
			}
			return false; 
		}


		//**************************
		//END VARIOUS GENERAL CALCULATIONS
		//**************************
	}
}