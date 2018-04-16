using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using System.Text; 
using UnityEngine.AI;

public class Builder : MonoBehaviour, IBuilder {


    private GameObject goFocus;
    private WorldObject worldObject;
    private Unit unit;
    private Player player;
    private ParamManager paramManager;
    private float constructionContributionFloat = 0.0f;
    private bool isLookingToPlaceBuilding = false;
    private Building buildingBeingPlaced;
    private Building unstartedPlacedBuilding; 
    private Building buildingBeingWorkedOn;
    private Building buildingToResumeWorkOn; 
    private Vector3 buildLocation;
    private bool finishedDoingBuilding; 


    private void Awake () {
        worldObject = GetComponent<WorldObject>(); 
        unit = GetComponent<Unit>(); 
	player = transform.root.GetComponent<Player> (); 
        paramManager = GetComponent<ParamManager>(); 
    }
	
    private void Update () {
	ControlGroupTriggeredBuildingPlacementCalcellation ();
    }

    private void OnEnable (){
        worldObject.OnWorldObjectDeathDelegate += ResetBuilder;
        //worldObject.OnWorldObjectDeathDelegate += ResumeNormalCursorFunctionality;
        worldObject.OnWorldObjectDeathDelegate += CancelBuildingPlacement;
    }

    private void OnDisable (){
        worldObject.OnWorldObjectDeathDelegate -= ResetBuilder;
        //worldObject.OnWorldObjectDeathDelegate -= ResumeNormalCursorFunctionality;
        worldObject.OnWorldObjectDeathDelegate -= CancelBuildingPlacement;
    }

	//*************************************
    //BEGIN INTERFACE METHODS
	//*************************************
    public GameObject GOfocus {get{return goFocus;}}

    public bool CheckToCancelBuildingPlacement(){
        if (isLookingToPlaceBuilding){
            CancelBuildingPlacement();
            return true;
		}
        return false;
    }

    public bool CheckForRepeatCommand(GameObject hitObject){
        Building building = hitObject.GetComponentInParent<Building> ();
        if (building && building == buildingBeingWorkedOn){
            return true;
        }
        return false;
    }

    public bool CheckToGiveResumeBuildOrder(GameObject hitObject){
        Building building = hitObject.GetComponentInParent<Building> ();
        if (building && !building.IsFinishedBuilding) {
            if (building.creator == worldObject){
                RegisterBuildingTask (building);
                return true;
            }
        }
        return false;
    }


    public bool CheckToGiveNewBuildOrder (){
        if (isLookingToPlaceBuilding) {
            if (AbleToCompleteBuildingPlacement()){
                return true; 
            }
        }
        return false; 
    }

    public void InitiateBuildingPlacement(){ //this starts off the whole process, and is called from within the WO hierarchy (which right now controls all input)
        if (!isLookingToPlaceBuilding){ //if you're already looking to place a building, ignore...although this may need to change in order to allow the temp building to change type
            if (paramManager.BuildablesList.Count > 0){ //if you are allowed to build anything, proceed
                ParamManager buildingParams = paramManager.BuildablesList[0].GetComponent<ParamManager>(); 
                if (buildingParams){
                    if (player.organicsPossessed >= buildingParams.ProductionCost){
                        BeginBuildingPlacement(paramManager.BuildablesList[0]); //...let the process begin
                        AudioManager.Instance.Play("SelectionChange");
                    } else {
                        AudioManager.Instance.Play("CannotBuild");
                    }
                }
            }
        }
	}

    public void ResetBuilder(){ 
        if (buildingBeingPlaced == null) {
            DestroyUnstartedPlacedBuilding();
            buildingBeingWorkedOn = null;
        }
    }

    public virtual bool AbleToPayResources(){
        if (buildingBeingWorkedOn){
            if (buildingBeingWorkedOn.constructionHasBegun){
                return true; 
            }
            if (worldObject.player.organicsPossessed < buildingBeingWorkedOn.paramManager.ProductionCost){
                ReactToDestructionOfWorkInProgress(buildingToResumeWorkOn); 
                DestroyUnstartedPlacedBuilding(); 
                buildingBeingWorkedOn = null;
                return false; 
            } else {
                //buildingBeingWorkedOn.InitializeConstruction();
                return true; 
            }
        } 
        return false;
    }

    public virtual void InitializeConstruction(){
        if(buildingBeingWorkedOn){
            buildingBeingWorkedOn.InitializeConstruction(); 
        }
    }


    public virtual bool CheckIfReadyToBeginBuilding() {
        if (buildingBeingWorkedOn == null){
            if (buildingToResumeWorkOn && unit.unitState.IsEngagedWithGO(buildingToResumeWorkOn.gameObject)){
                buildingBeingWorkedOn = buildingToResumeWorkOn;
                buildingToResumeWorkOn = null; 
                //buildingBeingWorkedOn.InitializeConstruction(); 
                return true; 
            }
            if (unstartedPlacedBuilding && unit.unitState.IsEngagedWithGO(unstartedPlacedBuilding.gameObject)){
                DestroyUnstartedBuildingInOccupiedSpace();
                if (unstartedPlacedBuilding){
                    buildingBeingWorkedOn = unstartedPlacedBuilding;
                    return true;
                }
            }
        }
        return false; 
    }



    public void BuildExecution(){
        if (unstartedPlacedBuilding){
            DestroyUnstartedBuildingInOccupiedSpace();
            if (unstartedPlacedBuilding){
                if (unit.unitState.IsEngagedWithGO(unstartedPlacedBuilding.gameObject)){                        
                    buildingBeingWorkedOn = unstartedPlacedBuilding;
                    unstartedPlacedBuilding = null;
                } else {return;}
            }
        }

        else if(buildingBeingWorkedOn && !buildingBeingWorkedOn.IsFinishedBuilding) {
            
            float incrementalContribution = paramManager.BuildSpeed * Time.deltaTime;
            constructionContributionFloat += incrementalContribution;
            if(constructionContributionFloat > 1) {
                int constructionContributionInt = Mathf.FloorToInt (constructionContributionFloat); 
                buildingBeingWorkedOn.Construct(constructionContributionInt);
                constructionContributionFloat = 0.0f;
                SelfParticleEffect();
                SelfAudioEffect(); 
                if (BuildingIsFinished()){
                    //StopSelfAudioEffect(); 
                    unit.unitState.SelfExitState(RTS.EAnimation.Build);
                }		 
            }
        }
    }

    public void ReactToDestructionOfWorkInProgress (Building building){
        if (building && building == buildingBeingWorkedOn){
            unit.unitState.SelfExitState(RTS.EAnimation.Build);
        }
    }

    //*************************************
    //END INTERFACE METHODS
	//*************************************











    //*************************************
    //BEING INPUT METHODS (no building objects created) 
    //*************************************



    private void RegisterBuildingTask (Building hitBuilding) { //again, this is called from the WO hierarchy right now.
        DestroyUnstartedPlacedBuilding();
        buildingBeingWorkedOn = null;
        buildingToResumeWorkOn = hitBuilding; //this control is straightforward now that i've added another building variable specifically for this
        goFocus = buildingToResumeWorkOn.gameObject; 
    }


    private void ActivateBuildingPlacementCursorRegime(){
        player.cursorManagerScript.DisableCursor();
        player.dragSelectionHandlerScript.enabled = false;
        player.gridSystem.enabled = true;
        player.DisableSelectionChange();
    }

    private void ResumeNormalCursorFunctionality(){
        //if (worldObject.currentlySelected || (worldObject.player && worldObject.player.selectedObjects.Count == 0)){
            player.cursorManagerScript.EnableCursor();
            player.dragSelectionHandlerScript.enabled = true;
            player.gridSystem.enabled = false;
            player.EnableSelectionChange();
        //}
    }

    //*************************************
    //END INPUT METHODS
    //*************************************

    //*************************************
    //BEGIN PLACEMENT METHODS creates temp building, left click finalizes placement and initiates build order
    //*************************************

    private void BeginBuildingPlacement(GameObject buildingGO) {
        buildingBeingPlaced = CreateTempBuilding(buildingGO);
        if (buildingBeingPlaced) {
            isLookingToPlaceBuilding = true;
            IEnumerator coroutine = MoveTempBuilding(buildingBeingPlaced);
            StartCoroutine(coroutine);
            ActivateBuildingPlacementCursorRegime();
        }
    }

    private Building CreateTempBuilding(GameObject buildingGO) {
        
        if (player && player.gridSystem) {
            GameObject newGameObject = Instantiate(buildingGO, player.gridSystem.GetGridLocation(), new Quaternion());
            newGameObject.transform.SetParent(ResourceManager.GetDynamicObjects(), true);
            newGameObject.SetActive(true);
            Building newBuilding = newGameObject.GetComponent<Building>();
            if (newBuilding) {
                newBuilding.RememberWhoMadeYou(worldObject);
                return newBuilding;
            }
        }
        return null;
    }

    private IEnumerator MoveTempBuilding(Building tempBuilding) {
        while (isLookingToPlaceBuilding) {
            if (player && player.gridSystem) {
                Vector3 gridLocation = player.gridSystem.GetGridLocation(); 
                if (gridLocation != buildLocation) {
                    buildLocation = gridLocation;
                    if (tempBuilding){
                        tempBuilding.transform.position = buildLocation;
                    }else {
                        CancelBuildingPlacement(); 
                    }
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }


    private bool AbleToCompleteBuildingPlacement(){
        if (buildingBeingPlaced){
            if (buildingBeingPlaced.buildingSpaceIsFree && buildingBeingPlaced.transform.position.y != -100){
                isLookingToPlaceBuilding = false;
                ResumeNormalCursorFunctionality();
                DestroyUnstartedPlacedBuilding();
                InitiateBuildOrder();
                return true; 
            }
        }
        return false; 
    }

    private void InitiateBuildOrder(){
        if (buildingBeingPlaced) {
            if (unstartedPlacedBuilding){
                DestroyUnstartedPlacedBuilding();
            }
            buildingBeingWorkedOn = null;
            unstartedPlacedBuilding = buildingBeingPlaced;
            buildingBeingPlaced = null;
            goFocus = unstartedPlacedBuilding.gameObject;
        }
    }

    //*************************************
    //END PLACEMENT METHODS
    //*************************************

    //*************************************
    //BEGIN CANCELLATION METHODS 
    //*************************************





    private void CancelBuildingPlacement(){
        if (buildingBeingPlaced && isLookingToPlaceBuilding){
            isLookingToPlaceBuilding = false;
            buildLocation = new Vector3(0, -100, 0);
            buildingBeingPlaced.transform.position = buildLocation; 
            StageManager.Instance.allSelectables.Remove(buildingBeingPlaced);
            Destroy(buildingBeingPlaced.gameObject);
            //StartCoroutine(DelayedDestroy());
        }
        ResumeNormalCursorFunctionality();
    }

    // private IEnumerator DelayedDestroy(){
    //     //yield return null;
    //     if (buildingBeingPlaced){
    //         Destroy(buildingBeingPlaced.gameObject);
    //     }
    //     yield return null;
    // }

    private void DestroyUnstartedPlacedBuilding(){
        if (unstartedPlacedBuilding && !unstartedPlacedBuilding.constructionHasBegun){
            Destroy(unstartedPlacedBuilding.gameObject);
            unstartedPlacedBuilding = null; 
        } else if (buildingBeingWorkedOn && !buildingBeingWorkedOn.constructionHasBegun){
            Destroy(buildingBeingWorkedOn.gameObject);
        }
    }

    private void DestroyUnstartedBuildingInOccupiedSpace(){
        if (unstartedPlacedBuilding && !unstartedPlacedBuilding.buildingSpaceIsFree){
            DestroyUnstartedPlacedBuilding();
            unit.unitState.SelfExitState(RTS.EAnimation.Build);
        }
    }



    private void ControlGroupTriggeredBuildingPlacementCalcellation(){ //while this ad hoc depdendence should be avoided, this is simple enough and effective for now
        if (isLookingToPlaceBuilding) {
            if (player.selectedObjects.Count > 0 && player.selectedObjects [0] != worldObject) {
                CancelBuildingPlacement (); 
            }
        }
    }

    //*************************************
    //END CANCELLATION METHODS
    //*************************************



    private bool BuildingIsFinished(){
        if (buildingBeingWorkedOn && buildingBeingWorkedOn.IsFinishedBuilding) {
            unit.unitState.SelfExitState(RTS.EAnimation.Build);
            return true;
        } else {return false;}
    }


    private void SelfParticleEffect(){
        BuildEffectSelfPositionSetter effectPositioner = GetComponentInChildren<BuildEffectSelfPositionSetter>();
        if (effectPositioner && paramManager.BuildEffectSelf){
            GameObject effect = Instantiate(paramManager.BuildEffectSelf, effectPositioner.transform.position, effectPositioner.transform.rotation);
            Vector3 modEffectPositioner = effect.transform.position; 
              
            effect.transform.position = modEffectPositioner; 
            effect.transform.SetParent (ResourceManager.GetDynamicObjects());
        }
    }

    private void SelfAudioEffect(){
        if (worldObject.audioSource){
            if (paramManager.BuildAudioClip){
                if (worldObject.audioSource.isPlaying == false){
                    AudioManager.Instance.Play(paramManager.BuildAudioClip, worldObject.audioSource);
                }
            }
        }

    }

    private void StopSelfAudioEffect(){
        if (worldObject.audioSource){
            worldObject.audioSource.Stop(); 
        }
    }




}
