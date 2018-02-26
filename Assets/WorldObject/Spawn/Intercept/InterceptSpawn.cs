using UnityEngine;
using System.Collections;
using RTS;
using System.Net;

public class InterceptSpawn : Spawn {


	public float laserCooldown;
	public bool laserEnabled;
	private GameObject effect;
	private InterceptSpawnLaser laser; 
	private float currentRechargeTime;




	protected override void Awake (){
		base.Awake ();
		laser = GetComponent<InterceptSpawnLaser> ();  
	}

	protected override void Start(){
		base.Start ();
		if (mySpawner) {
			laserCooldown = mySpawnerWorldObject.paramManager.SpawnerSpecialDurationFactor; 
		}
	}

	protected override void Update(){
		if (laserEnabled){
			LookForTarget();
			currentRechargeTime += Time.deltaTime; 
			if (targetCollider && currentRechargeTime >= laserCooldown) {
				DrawLaser ();
				currentRechargeTime = 0.0f; 
			}
		}
	}

	public override void ReactToSpawnerDeath(){
		//Invoke("TimedDeath", 7); 
	}

    public override void LookForTarget(){
		if (mySpawnerWorldObject && mySpawnerWorldObject.paramManager.PlayerOwned) {
            targetCollider = WorkManager.DetermineNearestEnemySpawnTargetColliderInRange(transform.position, paramManager.AggroRange);
		} else if (mySpawnerWorldObject && !mySpawnerWorldObject.paramManager.PlayerOwned){
            targetCollider = WorkManager.DetermineNearestEnemySpawnTargetColliderInRange(transform.position, paramManager.AggroRange);
        }
	}
	public override bool InRangeForAtack(){
		if (targetCollider){
			if (Vector3.Distance (transform.position, targetCollider.transform.position) < paramManager.AttackRange){
				return true;
			}
		}
		return false; 
	}

	protected virtual void DrawLaser (){
		laser.ShowLaserFromToCollider (transform.position, targetCollider.transform.position, targetCollider);
	}


	public virtual void ExecuteDestructionOfTargetColliderSpawn (Collider destroyedTarget){
		Spawn targetSpawn = destroyedTarget.GetComponentInParent<Spawn> ();
		if (targetSpawn) {
			targetSpawn.ResolveBeingHitByIntercept ();
		}
	}

}
