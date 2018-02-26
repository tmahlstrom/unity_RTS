using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using System.ComponentModel;
using UnityEngine.Events;
using UnityEngine.SceneManagement; 

public class Player : MonoBehaviour {


    [Header("Player parameters")]
    public int startingOrganics = 10;
    public int organicsPossessed; 

	[Header("References")]
	public string username;
	public List<WorldObject> selectedObjects;
	public bool hasWorldInfoCanvas = false;
	private bool selectionChangeEnabled = true; 
	protected UserInput userInput; 

	[Header("Inspector initialized variables")]
	public WorldInfoCanvas worldInfoCanvasScript;
	public DragSelectionHandler dragSelectionHandlerScript;
	public CursorManager cursorManagerScript; 
	public Camera mainCamera;
	public GroupFormationManager groupFormationManager; 
	public Color teamColor;
	public Material notAllowedMaterial, allowedMaterial, constructingMaterial;
	public int startMoney, startMoneyLimit, startPower, startPowerLimit;
	public GridSystem gridSystem;
	public GridSystem[] gridSystems; 
    public Text resourceText; 

	[Header("Control Groups")]
	public List<WorldObject> controlGroup1;
	public List<WorldObject> controlGroup2;
	public List<WorldObject> controlGroup3;
	public List<WorldObject> controlGroup4;
	public List<WorldObject> controlGroup5;
	public List<WorldObject> controlGroup6;
	public List<WorldObject> controlGroup7;
	public List<WorldObject> controlGroup8;
	public List<WorldObject> controlGroup9;
	public List<WorldObject> controlGroup0;
   	 


	void Awake() {
		userInput = GetComponent<UserInput> (); 
		groupFormationManager = GetComponent<GroupFormationManager> ();
		worldInfoCanvasScript = GetComponentInChildren<WorldInfoCanvas> ();
		dragSelectionHandlerScript = GetComponentInChildren<DragSelectionHandler> ();
		cursorManagerScript = GetComponentInChildren<CursorManager> (); 
		gridSystems = FindObjectsOfType<GridSystem>();
		if (gridSystems.Length >0){
			gridSystem = gridSystems[0];
		}
		if (worldInfoCanvasScript != null) {
			hasWorldInfoCanvas = true;
		}

       
       
	}

	void Start () {
		StageManager.Instance.listOfPlayers.Add(this);
		if (hasWorldInfoCanvas) {
			StageManager.Instance.listOfPlayersWithWorldInfoCanvas.Add (this);
		}
        organicsPossessed = startingOrganics;
        UpdateResources();
		InitializeUnits(); 
		InitializeBuildings(); 
    }

	private void InitializeUnits(){
		Units unitWrap = GetComponentInChildren<Units>();
		foreach(Transform child in unitWrap.gameObject.transform){
			child.gameObject.SetActive(true);
		}
	}

	private void InitializeBuildings(){
		Buildings buildingWrap = GetComponentInChildren<Buildings>(); 
		foreach(Transform child in buildingWrap.gameObject.transform){
			child.gameObject.SetActive(true);
		}
	}




		

	//*************************************
	//BEING REFERENCE METHODS
	//*************************************
	public List<WorldObject> GetSelectedObjects(){
		return selectedObjects; 
	}
	//*************************************
	//END REFERECE METHODS
	//*************************************

	//*************************************
	//BEING RESOURCE METHODS
	//*************************************

    public void UpdateResources(){
        if (resourceText) {
            resourceText.text = "Organics: " + organicsPossessed.ToString();
        }
    }

    public void ObtainOrganics(int amount) {
        organicsPossessed += amount;
        UpdateResources();
    }

    public void SpendOrganics (int amount){
        organicsPossessed -= amount;
        UpdateResources();
    }

	//*************************************
	//END RESOURCE METHODS
	//*************************************



	//*************************************
	//BEGIN DRAG SELECTION CONSIDERATIONS 
	//*************************************

	public void ConsiderBoxSelectedObjects () {
		DragSelectionHandler dragSelector = GetComponentInChildren< DragSelectionHandler > ();
		List<WorldObject> boxSelectedObjects = dragSelector.boxSelectedObjects;
		if (boxSelectedObjects.Count == 0) {
			return;
		}
		bool nothingNew = true; 
		for (int i = 0; i < boxSelectedObjects.Count; i++) {
			if (selectedObjects.Contains(boxSelectedObjects[i]) == false){
				nothingNew = false; 
			}
		}
		if (nothingNew){
			return; 
		}
		if (boxSelectedObjects.Count == 1) {
			ClearSelection ();
			selectedObjects.Add (boxSelectedObjects [0]);
            NotifySelectedObjects();
            return;
		} 

		int unitDragSelectedCount = 0;
		int buildingDragSelectedCount = 0;
		for (int i = 0; i < boxSelectedObjects.Count; i++) {
			if (boxSelectedObjects [i].player != null && boxSelectedObjects [i].player.username == username && boxSelectedObjects [i].GetComponent<Unit> ()) {
				unitDragSelectedCount += 1;
			}
			if (boxSelectedObjects [i].player != null && boxSelectedObjects [i].player.username == username && boxSelectedObjects [i].GetComponent<Building> ()) {
				buildingDragSelectedCount += 1;
			}
		}

		if (unitDragSelectedCount >= 1) {
			if (!Input.GetKey (KeyCode.LeftShift) && !Input.GetKey (KeyCode.RightShift)) {
				ClearSelection ();
			}
			for (int i = 0; i < boxSelectedObjects.Count; i++) {
				if (boxSelectedObjects [i].player != null && boxSelectedObjects [i].player.username == username && boxSelectedObjects [i].GetComponent<Unit> ()) {
					selectedObjects.Add (boxSelectedObjects [i]);
				}
			}
		} else if (buildingDragSelectedCount >= 1) {
			if (!Input.GetKey (KeyCode.LeftShift) && !Input.GetKey (KeyCode.RightShift)) {
				ClearSelection ();
			}
			for (int i = 0; i <boxSelectedObjects.Count; i++) {
				if (boxSelectedObjects [i].player != null && boxSelectedObjects [i].player.username == username && boxSelectedObjects [i].GetComponent<Building> ()) {
					selectedObjects.Add (boxSelectedObjects [i]);
				}
			}
		}
        NotifySelectedObjects();
    }
	//*************************************
	//END DRAG SELECTION CONSIDERATIONS 
	//*************************************


	//*************************************
	//BEGIN SELECTION CONSIDERATIONS 
	//*************************************


    public void DisableSelectionChange(){
        selectionChangeEnabled = false; 
    }

    public void EnableSelectionChange() {
        selectionChangeEnabled = true;
    }

	public void ClearSelection(){
		for (int i = 0; i < selectedObjects.Count; i++) {
			if (selectedObjects [i] != null){
                selectedObjects [i].ChangeSelectionStatus(false);
				//selectedObjects [i].currentlySelected = false;
			}
		}
		selectedObjects.Clear ();
	}

	public void FigureOutSelectionAfterLeftClickOnThisWorldObject (WorldObject worldObject){

        if (selectionChangeEnabled == false){
            return;
        }

		if (selectedObjects.Count == 1 && selectedObjects[0] == worldObject){
			return; 
		}

        if (!StageManager.Instance.allSelectables.Contains(worldObject)) {
            return;
        }

        if (!userInput.ShiftIsBeingHeld () || (worldObject.player != this && selectedObjects.Count == 0) || selectedObjects[0].player != this) {
			ClearSelection ();
			selectedObjects.Add (worldObject);

		}
		if (userInput.ShiftIsBeingHeld () && worldObject.player == this && SameTypeAsOtherSelectedWorldObjects (worldObject)) {
			selectedObjects.Add (worldObject);
		}
        NotifySelectedObjects(); 
	}

    private void NotifySelectedObjects(){
		for (int i = 0; i < selectedObjects.Count; i++){
			if (selectedObjects[i] != null){
                selectedObjects[i].ChangeSelectionStatus(true);
				//selectedObjects[i].currentlySelected = true;
			}
		}
    }

	private bool SameTypeAsOtherSelectedWorldObjects (WorldObject worldObject){
		Unit unitConsidered = worldObject.GetComponent<Unit> ();
		Building buildingConsidered = worldObject.GetComponent<Building> (); 
		if (selectedObjects.Count == 0) {
			return true; 
		}
		if (selectedObjects [0] && selectedObjects[0].GetComponent<Unit>()) {
			if (unitConsidered) {
				return true; 
			}
			if (buildingConsidered) {
				return false; 
			}
		}
		if (selectedObjects [0] && selectedObjects [0].GetComponent<Building> ()) {
			if (unitConsidered) {
				return false; 
			}
			if (buildingConsidered) {
				return true; 
			}
		}
		return false; 
	}

	//*************************************
	//END SELECTION CONSIDERATIONS 
	//*************************************


	//*************************************
	//BEGIN CONTROL GROUP CONSIDERATIONS 
	//*************************************

	public void AddMeToOpenControlGroup(WorldObject worldObject){
		if (controlGroup1.Count == 0) {
			controlGroup1.Add (worldObject);
			return;
		}
		if (controlGroup2.Count == 0) {
			controlGroup2.Add (worldObject);
			return;
		}
		if (controlGroup3.Count == 0) {
			controlGroup3.Add (worldObject);
			return;
		}
		if (controlGroup4.Count == 0) {
			controlGroup4.Add (worldObject);
			return;
		}
	}


	public void RecallControlGroups (){
		if (selectedObjects.Count != 0) ClearSelection ();
		if (Input.GetKey (KeyCode.Alpha1)) {
			for (int i = 0; i < controlGroup1.Count; i++) {
				selectedObjects.Add (controlGroup1 [i]);
			}
		}
		if (Input.GetKey (KeyCode.Alpha2)) {
			for (int i = 0; i < controlGroup2.Count; i++) {
				selectedObjects.Add (controlGroup2 [i]);
			}
		}
		if (Input.GetKey (KeyCode.Alpha3)) {
			for (int i = 0; i < controlGroup3.Count; i++) {
				selectedObjects.Add (controlGroup3 [i]);
			}
		}
		if (Input.GetKey (KeyCode.Alpha4)) {
			for (int i = 0; i < controlGroup4.Count; i++) {
				selectedObjects.Add (controlGroup4 [i]);
			}
		}
		if (Input.GetKey (KeyCode.Alpha5)) {
			for (int i = 0; i < controlGroup5.Count; i++) {
				selectedObjects.Add (controlGroup5 [i]);
			}
		}
		if (Input.GetKey (KeyCode.Alpha6)) {
			for (int i = 0; i < controlGroup6.Count; i++) {
				selectedObjects.Add (controlGroup6 [i]);
			}
		}
		if (Input.GetKey (KeyCode.Alpha7)) {
			for (int i = 0; i < controlGroup7.Count; i++) {
				selectedObjects.Add (controlGroup7 [i]);
			}
		}
		if (Input.GetKey (KeyCode.Alpha8)) {
			for (int i = 0; i < controlGroup8.Count; i++) {
				selectedObjects.Add (controlGroup8 [i]);
			}
		}
		if (Input.GetKey (KeyCode.Alpha9)) {
			for (int i = 0; i < controlGroup9.Count; i++) {
				selectedObjects.Add (controlGroup9 [i]);
			}
		}
		if (Input.GetKey (KeyCode.Alpha0)) {
			for (int i = 0; i < controlGroup0.Count; i++) {
				selectedObjects.Add (controlGroup0 [i]);
			}
		}
		NotifySelectedObjects();	
	}

	public void SetControlGroup1 (){
		controlGroup1.Clear();
		for (int i = 0; i <selectedObjects.Count; i++) {
			controlGroup1.Add (selectedObjects [i]);
		}
	}
	public void SetControlGroup2 (){
		controlGroup2.Clear();
		for (int i = 0; i <selectedObjects.Count; i++) {
			controlGroup2.Add (selectedObjects [i]);
		}
	}
	public void SetControlGroup3 (){
		controlGroup3.Clear();
		for (int i = 0; i <selectedObjects.Count; i++) {
			controlGroup3.Add (selectedObjects [i]);
		}
	}
	public void SetControlGroup4 (){
		controlGroup4.Clear();
		for (int i = 0; i <selectedObjects.Count; i++) {
			controlGroup4.Add (selectedObjects [i]);
		}
	}
	public void SetControlGroup5 (){
		controlGroup5.Clear();
		for (int i = 0; i <selectedObjects.Count; i++) {
			controlGroup5.Add (selectedObjects [i]);
		}
	}
	public void SetControlGroup6 (){
		controlGroup6.Clear();
		for (int i = 0; i <selectedObjects.Count; i++) {
			controlGroup6.Add (selectedObjects [i]);
		}
	}
	public void SetControlGroup7 (){
		controlGroup7.Clear();
		for (int i = 0; i <selectedObjects.Count; i++) {
			controlGroup7.Add (selectedObjects [i]);
		}
	}
	public void SetControlGroup8 (){
		controlGroup8.Clear();
		for (int i = 0; i <selectedObjects.Count; i++) {
			controlGroup8.Add (selectedObjects [i]);
		}
	}
	public void SetControlGroup9 (){
		controlGroup9.Clear();
		for (int i = 0; i <selectedObjects.Count; i++) {
			controlGroup9.Add (selectedObjects [i]);
		}
	}
	public void SetControlGroup0 (){
		controlGroup0.Clear();
		for (int i = 0; i <selectedObjects.Count; i++) {
			controlGroup0.Add (selectedObjects [i]);
		}
	}

	public void RemoveFromControlGroups(WorldObject deadUnit){
		controlGroup1.Remove (deadUnit);
		controlGroup2.Remove (deadUnit);
		controlGroup3.Remove (deadUnit);
		controlGroup4.Remove (deadUnit);
		controlGroup5.Remove (deadUnit);
		controlGroup6.Remove (deadUnit);
		controlGroup7.Remove (deadUnit);
		controlGroup8.Remove (deadUnit);
		controlGroup9.Remove (deadUnit);
		controlGroup0.Remove (deadUnit);
	}
	//*************************************
	//END CONTROL GROUP CONSIDERATIONS 
	//*************************************


	//*************************************
	//BEGIN UPDATE PLAYER CANVASES
	//*************************************
	public void UpdateStatsForThisWorldObject(WorldObject wo){
		if (StageManager.Instance.listOfPlayersWithWorldInfoCanvas == null || StageManager.Instance.listOfPlayersWithWorldInfoCanvas.Count == 0){
			return;
		}
		foreach (Player p in StageManager.Instance.listOfPlayersWithWorldInfoCanvas) {
			p.worldInfoCanvasScript.UpdateWorldObjectStats (wo);
		}
	}
	//*************************************
	//END UPDATE PLAYER CANVASES
	//*************************************

		
}
