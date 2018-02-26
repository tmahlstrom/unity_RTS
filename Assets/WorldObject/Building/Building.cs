using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;
using System;
using System.IO;
using UnityEngine.AI;

public class Building : WorldObject {

    public BuildingBaseState buildingState; 

    public WorldObject creator;
    public Building building;
    public Spawner spawner;
    public bool autoComplete; 
    protected NavMeshObstacle navObstacle;

    public bool buildingSpaceIsFree = true;

    public bool constructionHasBegun;
    protected bool isFinishedBuilding = false;
    public bool IsFinishedBuilding { get { return isFinishedBuilding; }}

	private float maxBuildProgress = 100;
	private float buildProgress = 0;
	
    protected Vector3 spawnPoint;
    protected ActiveBuildingModel activeModel;
    protected DestroyedBuildingModel destroyedModel;
    protected List< Material[] > originalMaterials = new List< Material[] >();


	protected override void Awake() {
		base.Awake();
        building = this;
		spawnPoint = transform.position;
		navObstacle = GetComponent<NavMeshObstacle> ();
        activeModel = GetComponentInChildren<ActiveBuildingModel>();
        destroyedModel = GetComponentInChildren<DestroyedBuildingModel>(true);
        spawner = GetComponentInChildren<Spawner>();
        maxBuildProgress = paramManager.MaxHitPoints; 
        StoreOriginalMaterials(); 
        buildingState = new VirtualStateB(building, false);
    }

	protected override void Start () {
		base.Start ();
	}

	protected override void Update () {
		base.Update();
        buildingState.UpdateState(); 
	}
    public void SetBuildingState (BuildingBaseState newState){
        buildingState.ExitRoutine(buildingState);
		buildingState = newState;
	}

	public void EnableNavObstacle(){
		if (navObstacle) {
			navObstacle.enabled = true; 
		}
	}

    protected virtual void OnEnable(){
        StageManager.AWaveHasBeenCompleted += RegenerateBuilding;
    }

    protected virtual void OnDisable(){
        StageManager.AWaveHasBeenCompleted -= RegenerateBuilding;
    }



	protected void RegenerateBuilding (){
        if (isFinishedBuilding){
		    ManageReviveOfWorldObject (); 
        }
	}

	protected override void ManageReviveOfWorldObject(){
		base.ManageReviveOfWorldObject ();
        SetBuildingState(new RegenerateStateB(this, false));
        if (activeModel) {
            activeModel.gameObject.SetActive(true);
        }
        if (destroyedModel){
            destroyedModel.gameObject.SetActive(false); 
        }
	}

		
	public void Construct(int amount) {
        if (!constructionHasBegun) {
            if (creator.player.organicsPossessed < paramManager.ProductionCost){
                Builder b = creator.GetComponent<Builder>(); 
                b.ReactToDestructionOfWorkInProgress(this); 
                worldObject.TakeDamage(paramManager.MaxHitPoints, transform.position, worldObject); 
                return; 
            }
            InitializeConstruction();
            TakeDamage (-1, transform.position, worldObject);
        }
		buildProgress += amount;
        int correspondingHPChange = CalculateHPBuildProportion (amount); 
        TakeDamage (-correspondingHPChange, transform.position, worldObject);
		if(buildProgress >= maxBuildProgress) {
            CompleteMyConstruction ();
		}
	}

    public void InitializeConstruction(){
        if (!constructionHasBegun) {
            if (creator.player.organicsPossessed < paramManager.ProductionCost){
                Builder b = creator.GetComponent<Builder>(); 
                b.ReactToDestructionOfWorkInProgress(this); 
                return; 
            }
            ReawakenWO();
            PayProductionCost(); 
            constructionHasBegun = true;
            JumpStartConstruction();
            SetBuildingState(new ConstructionStateB(this, false));
            return; 
        }
    }



    private void PayProductionCost(){
            creator.player.SpendOrganics(paramManager.ProductionCost);
    }

    private void JumpStartConstruction(){
        int jumpStart;
        jumpStart = (paramManager.MaxHitPoints/5);
        if (jumpStart < 2){
            jumpStart = 2; 
        }
        Construct(jumpStart);
    }

    private int CalculateHPBuildProportion(int amount){ //right now, the building hp has to be a perfect multiple of the build progress...not ideal. easiest solution is to make the maxbuildprogress == maxhp... done.
        int hpChange = Mathf.RoundToInt(amount) * paramManager.MaxHitPoints / Mathf.RoundToInt(maxBuildProgress);
        return hpChange; 
    }

    public virtual void RememberWhoMadeYou(WorldObject creator){
		this.creator = creator; 
        if (creator){
            SetBuildingMaterial(creator.player.allowedMaterial);
        }
	}

	protected virtual void ReawakenWO(){
		if (!hasStatsVisible && creator) {
            player = creator.GetComponentInParent<Player>();
            if (player) {
				transform.SetParent (player.transform.GetComponentInChildren<Buildings> ().transform, true);
				SetBuildingMaterial (player.constructingMaterial);
                StageManager.Instance.allSelectables.Add(this);
                paramManager.PlayerOwned = true; 
            }
			GiveThisWorldInfoCanvasStats (); 
			EnableNavObstacle ();
            gameObject.layer = 11;
            if (activeModel){
                activeModel.gameObject.layer = 13;
            }
            if (destroyedModel){
                destroyedModel.gameObject.layer = 13;
            }
            Selector selector = GetComponentInChildren<Selector>(); 
            if (selector){
                selector.gameObject.layer = 11;
            }
		}
	}

	protected override int ConsiderDamageMultipliers (int rawDamage){
		int updatedDamage = rawDamage; 
		if (!isFinishedBuilding && rawDamage >= 1) {
			updatedDamage = rawDamage * 10;
		}
		return updatedDamage; 
	}

	protected override void ConsiderShowingDamageOnWorldInfoCanvas(int damage, Vector3 impactPoint){
		if (isFinishedBuilding || damage >= 1) {
			base.ConsiderShowingDamageOnWorldInfoCanvas (damage, impactPoint);
		}
	}

	protected virtual void CompleteMyConstruction(){
		isFinishedBuilding = true; 
		RestoreMaterials();
		SetTeamColor();
		foreach (Player p in StageManager.Instance.listOfPlayersWithWorldInfoCanvas) {
			p.worldInfoCanvasScript.ActivateNormalHPTextColor (worldObject);
		}
		if (spawner) {
			spawner.enabled = true; 
		}
        Target target = GetComponentInChildren<Target>();
        if (target){
            target.gameObject.layer = 30; 
        }
        SetBuildingState(new IdleStateB(this, false));
	}


	protected override void GiveThisWorldInfoCanvasStats(){
		base.GiveThisWorldInfoCanvasStats ();
	}


    protected override void ManageDeathOfWorldObject() {
        base.ManageDeathOfWorldObject();
        SetBuildingState(new DeadStateB(this, false));
        if (player) {
            player.RemoveFromControlGroups(this);
            if (activeModel){
                activeModel.gameObject.SetActive(false);
            }
            if (destroyedModel){
                destroyedModel.gameObject.SetActive(true); 
            }

            if (creator){
                Builder builder = creator.GetComponent<Builder>();
                if (builder){
                    builder.ReactToDestructionOfWorkInProgress(this);
                }
            } 
        }
    }



    //*************************************
    //BEGIN MATERIALS METHODS
	//*************************************
    protected virtual void OnTriggerStay (Collider other){
        if (!constructionHasBegun) {
            Spawn spawn = other.GetComponentInParent<Spawn>();
            FlyDome flyDome = other.GetComponentInParent<FlyDome>();
            if (spawn == null && flyDome == null) {
                WorldObject otherWO = other.GetComponentInParent<WorldObject>();
                if (otherWO == null || (otherWO && otherWO != this.creator)) {
                    buildingSpaceIsFree = false;
                    if (creator) {
                        SetBuildingMaterial(creator.player.notAllowedMaterial);
                    }
                }
                if (other.gameObject.name == "BuildBlocker"){
                    buildingSpaceIsFree = false;
                    if (creator) {
                        SetBuildingMaterial(creator.player.notAllowedMaterial);
                    }
                }
            }
        }
	}

    protected virtual void OnTriggerExit (Collider other){
        if (!constructionHasBegun) {
            Spawn spawn = other.GetComponentInParent<Spawn>();
            FlyDome flyDome = other.GetComponentInParent<FlyDome>();
            if (spawn == null && flyDome == null) {
                WorldObject otherWO = other.GetComponentInParent<WorldObject>();
                if (otherWO == null || (otherWO && otherWO != this.creator)) {
                    buildingSpaceIsFree = true;
                }
                if (creator) {
                    SetBuildingMaterial(creator.player.allowedMaterial);
                }
            }
        }
	}


    	public void SetBuildingMaterial(Material material) {
		Renderer[] renderers = GetComponentsInChildren< Renderer>();
		foreach(Renderer renderer in renderers) {
            Material[] materials = renderer.materials; 
            if (materials.Length > 0){
                materials[0] = material; 
                if (materials.Length > 1){
                    materials[1] = ParameterStore.Instance.invisibleMaterial;
                }
            }
            //for (int i = 0; i < materials.Length; i++){
            //    materials[i] = material; //this works, but i'm using the below hack becaues with the current 'allowed' and 'not allowed' materials method, it only looks good if a single material is used
            //}
            renderer.materials = materials;
               // = material;
		}
	}

    private void StoreOriginalMaterials(){
        Renderer[] renderers = GetComponentsInChildren< Renderer >();
        foreach (Renderer rend in renderers){
            //originalMaterials.Add(rend.material);
            originalMaterials.Add(rend.materials);

        }
    }

	public void RestoreMaterials() {
		Renderer[] renderers = GetComponentsInChildren< Renderer >();
		if(originalMaterials.Count == renderers.Length) {
			for(int i = 0; i < renderers.Length; i++) {
				renderers[i].materials = originalMaterials[i];
			}
		}
	}

    //*************************************
    //END MATERIALS METHODS
	//*************************************
	
	
		
}
