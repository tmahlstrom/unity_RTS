using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class Spawner : MonoBehaviour, ISpawner {

    //*******************************************
    //BEGIN INTERFACE 
    //*******************************************

    public bool ReadyToBeginSpawning(){
        if (!spawnProcessStarted){
            if (CooldownIsComplete() && CanSupportPopulationIncrease(1)) {
                return true; 
            }
        }
        return false; 
    }


    public virtual void InitiateSpawnProcess(){
        spawnProcessStarted = true; 
    }

    public virtual void ConcludeSpawnProcess(){
        ResetCurrentRechargeTime();
        spawnProcessStarted = false; 
    }
    

    public void SpawnClimaxEvent(){
        CreateSpawn();
        SelfEffect(); 
        SpawnAudio(); 
    }

    public bool IsSpawnProcessStarted(){
        return spawnProcessStarted; 
    }


	public virtual void AddNewSpawnToPopulationCount(){
		spawnPopulationCounter ++;
	}

	public virtual void SubtractDeadSpawnFromPopulationCount(){
		if (spawnPopulationCounter > 0) {
			spawnPopulationCounter--;
		}
	}   

	public virtual bool ReadyToSpawnerSpecial(){
        return false; 
	}

	public virtual void InitiateSpawnerSpecial(){

	}

    public virtual void ConcludeSpawnerSpecial(){

    }



    //*******************************************
    //END INTERFACE 
    //*******************************************



    protected ParamManager paramManager;

	protected WorldObject worldObject;
	protected Unit unit;

     

    protected int spawnPopulationCounter = 0;
	protected float currentRechargeTime = 0.0f;
    protected bool spawnProcessStarted; 
    protected bool specialProcessActive; 

	protected Vector3 spawnGenerationPoint;
	protected GameObject spawnHolder;
	protected int releaseForce = 15;
	protected Spawner spawner; 

    protected Vector3 domeCenter; 


	protected virtual void Awake () {
        paramManager = GetComponent<ParamManager>(); 
		worldObject = GetComponent<WorldObject> (); 
		unit = GetComponent<Unit> ();
		spawner = this;
        FlyDome dome = GetComponentInParent<FlyDome>(); 
        if (dome){
            domeCenter = dome.transform.position; 
        }
        if (worldObject){
            worldObject.OnWorldObjectDeathDelegate += NotifySpawnsOfDeath;
        }
	}

	protected virtual void Start () {
        currentRechargeTime = paramManager.SpawnCooldown - Random.Range(1, 3);
		CreateSpawnsTransform (); 
	}

    protected virtual void Update() {
        currentRechargeTime += Time.deltaTime;
    }

	protected void OnEnable(){
		spawnPopulationCounter = 0; 
	}


    protected virtual void CreateSpawn(){

    }

	protected virtual void CreateSpawnsTransform (){
		spawnHolder = new GameObject();
		spawnHolder.name = worldObject.name + " Spawns";
		if (worldObject.player) {
			spawnHolder.transform.SetParent (worldObject.player.GetComponentInChildren<Spawns> ().gameObject.transform, true);
            return; 
		}
		if (worldObject.enemyManager) {
			spawnHolder.transform.SetParent (worldObject.enemyManager.GetComponentInChildren<Spawns> ().gameObject.transform, true);
            return; 
		}
	}

	protected virtual Vector3 DetermineSpawnPoint(){
		return spawnGenerationPoint;
	}

    protected bool CooldownIsComplete() {
        if (currentRechargeTime >= paramManager.SpawnCooldown) return true;
        return false;
    }

    protected bool CanSupportPopulationIncrease(int popIncrease){
        if (spawnPopulationCounter + popIncrease <= paramManager.MaxSpawnPopulation) return true;
        return false; 
    }


	protected virtual void AddReleaseForces (Spawn spawn){
		
	}


    protected virtual void ResetCurrentRechargeTime(){
        currentRechargeTime = 0.0f; 
    }


    protected void SelfEffect(){
        SpawnEffectSelfPositionSetter effectPositioner = GetComponentInChildren<SpawnEffectSelfPositionSetter>();
        if (effectPositioner && paramManager.SpawnEffectSelf){
            GameObject effect = Instantiate(paramManager.SpawnEffectSelf, effectPositioner.transform.position, effectPositioner.transform.rotation);
            effect.transform.SetParent (ResourceManager.GetDynamicObjects());
        }
    }

    protected void SpawnAudio(){
        if (worldObject.audioSource){
            if (paramManager.SpawnAudioClip){
                AudioManager.Instance.Play(paramManager.SpawnAudioClip, worldObject.audioSource);
                // worldObject.audioSource.clip = paramManager.AttackAudioClip; 
                // worldObject.audioSource.Play(); 
            }
        }
    }

    protected void NotifySpawnsOfDeath(){
        if (spawnHolder){
            Spawn[] spawns = spawnHolder.GetComponentsInChildren<Spawn>(); 
            foreach (Spawn spawn in spawns){
                spawn.ReactToSpawnerDeath(); 
            }
        }
    }
}
