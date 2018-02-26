using UnityEngine;
using System.Collections;
using RTS;
using System.Runtime.Remoting.Messaging;
using System;
using System.Configuration;
using System.Security.Cryptography;


public class UserInput : MonoBehaviour {
	
	private Player player;
	private GridSystem gridSystem; 
	private CameraControl cameraControl; 


	void Awake () {
		player = GetComponent< Player >();
		cameraControl = FindObjectOfType<CameraControl>(); 
		if (player) {
			gridSystem = player.gridSystem;
		}
	}

	void Update () {
		if(player) {
//			MoveCamera();
			RotateCamera();
			CameraZoom(); 
			MouseActivity ();
			KeyBoardActivity ();
		}
	}

	private bool actionButtonHeldDown = false; 
	private float actionButtonDownTime;
	private float actionButtonTriggerTime = 0.5f; 

	//*******************************
	//BEGIN KEYBOARD ACTIVITY
	//*******************************
	private void KeyBoardActivity() {
		if (Input.GetKeyDown (KeyCode.Space)){
			if (player.selectedObjects.Count == 1 && player.selectedObjects [0].player == player) {
				player.selectedObjects [0].SpacebarToWO(); 
			}
		}


		// if (Input.GetKeyDown (KeyCode.Space) && actionButtonHeldDown == false) {
		// 	actionButtonDownTime = Time.time;
		// 	actionButtonHeldDown = true;
		// }
		// if (Input.GetKeyUp (KeyCode.Space)) {
		// 	actionButtonHeldDown = false;
		// 	if (player.selectedObjects.Count != 0 && player.selectedObjects [0].player == player) {
		// 		if (player.selectedObjects [0].GetComponent<WorldObject> () ?? null) {
		// 			foreach (WorldObject worldObject in player.selectedObjects) {
		// 				worldObject.SpacebarToWO ();
		// 			}
		// 		}
		// 	}
		// }
		// if (Time.time >= actionButtonDownTime + actionButtonTriggerTime && actionButtonHeldDown == true) {
		// 	actionButtonHeldDown = false;
		// 	if (player.selectedObjects.Count != 0 && player.selectedObjects [0].player == player) {
		// 		if (player.selectedObjects [0].GetComponent<WorldObject> () ?? null) {
		// 			foreach (WorldObject worldObject in player.selectedObjects) {
		// 				worldObject.SpacebarHeldToWO ();
		// 			}
		// 		}
		// 	}		
		// }    


//		if (Input.GetKeyDown (KeyCode.Space) && player.selectedObjects.Count != 0 && player.selectedObjects[0].player == player) {
//			if(player.selectedObjects[0] ?? null){
//			if (player.selectedObjects [0].GetComponent<WorldObject> () ?? null) {
//					foreach (WorldObject worldObject in player.selectedObjects) {
//						worldObject.BeginAction ();
//					}
//				}
//			}
//		}
//
//		if (Input.GetKey (KeyCode.Space) && player.selectedObjects.Count != 0 && player.selectedObjects[0].player == player) {
//			if(player.selectedObjects[0] ?? null){
//				if (player.selectedObjects [0].GetComponent<WorldObject> () ?? null) {
//					foreach (WorldObject worldObject in player.selectedObjects) {
//						worldObject.ContinueWithSustainedAction ();
//					}
//				}
//			}
//		}

		if (!Input.GetKey (KeyCode.LeftControl) && 
			((Input.GetKeyDown (KeyCode.Alpha1) && player.controlGroup1.Count != 0) || 
				(Input.GetKeyDown (KeyCode.Alpha2) && player.controlGroup2.Count != 0) || 
				(Input.GetKeyDown (KeyCode.Alpha3) && player.controlGroup3.Count != 0) || 
				(Input.GetKeyDown (KeyCode.Alpha4) && player.controlGroup4.Count != 0) || 
				(Input.GetKeyDown (KeyCode.Alpha5) && player.controlGroup5.Count != 0) || 
				(Input.GetKeyDown (KeyCode.Alpha6) && player.controlGroup6.Count != 0) || 
				(Input.GetKeyDown (KeyCode.Alpha7) && player.controlGroup7.Count != 0) || 
				(Input.GetKeyDown (KeyCode.Alpha8) && player.controlGroup8.Count != 0) || 
				(Input.GetKeyDown (KeyCode.Alpha9) && player.controlGroup9.Count != 0) || 
				(Input.GetKeyDown (KeyCode.Alpha0) && player.controlGroup0.Count != 0))) {
			player.RecallControlGroups ();
		}

		if (Input.GetKeyDown (KeyCode.Alpha1) && Input.GetKey (KeyCode.LeftControl) && player.selectedObjects.Count != 0 && player.selectedObjects[0].player == player) {
			player.SetControlGroup1 ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha2) && Input.GetKey (KeyCode.LeftControl) && player.selectedObjects.Count != 0 && player.selectedObjects[0].player == player) {
			player.SetControlGroup2 ();
		}

		if (Input.GetKeyDown (KeyCode.Alpha3) && Input.GetKey (KeyCode.LeftControl) && player.selectedObjects.Count != 0 && player.selectedObjects[0].player == player) {
			player.SetControlGroup3 ();
		}

		if (Input.GetKeyDown (KeyCode.Alpha4) && Input.GetKey (KeyCode.LeftControl) && player.selectedObjects.Count != 0 && player.selectedObjects[0].player == player) {
			player.SetControlGroup4 ();
		}

		if (Input.GetKeyDown (KeyCode.Alpha5) && Input.GetKey (KeyCode.LeftControl) && player.selectedObjects.Count != 0 && player.selectedObjects[0].player == player) {
			player.SetControlGroup5 ();
		}

		if (Input.GetKeyDown (KeyCode.Alpha6) && Input.GetKey (KeyCode.LeftControl) && player.selectedObjects.Count != 0 && player.selectedObjects[0].player == player) {
			player.SetControlGroup6 ();
		}

		if (Input.GetKeyDown (KeyCode.Alpha7) && Input.GetKey (KeyCode.LeftControl) && player.selectedObjects.Count != 0 && player.selectedObjects[0].player == player) {
			player.SetControlGroup7 ();
		}
		
		if (Input.GetKeyDown (KeyCode.Alpha8) && Input.GetKey (KeyCode.LeftControl) && player.selectedObjects.Count != 0 && player.selectedObjects[0].player == player) {
			player.SetControlGroup8 ();
		}

		if (Input.GetKeyDown (KeyCode.Alpha9) && Input.GetKey (KeyCode.LeftControl) && player.selectedObjects.Count != 0 && player.selectedObjects[0].player == player) {
			player.SetControlGroup9 ();
		}

		if (Input.GetKeyDown (KeyCode.Alpha0) && Input.GetKey (KeyCode.LeftControl) && player.selectedObjects.Count != 0 && player.selectedObjects[0].player == player) {
			player.SetControlGroup0 ();
		}
	}

	public bool ShiftIsBeingHeld(){
		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)){
			return true;
		}
		return false;
	}

	//*******************************
	//END KEYBOARD ACTIVITY
	//*******************************


	//*******************************
	//BEGIN MOUSE ACTIVITY
	//*******************************

	private void MouseActivity() {
		if (Input.GetMouseButtonDown (0)) {
			if (gridSystem) {
				Vector3 gridLocation = gridSystem.GetGridLocation (); 
			}
			LeftMouseClickPre ();
		} else if (Input.GetMouseButtonDown (1)) {
			RightMouseClickPre ();
		}
		MouseHover ();
	}

	private void LeftMouseClickPre() {
		GameObject hitObject = WorkManager.FindHitObject(Input.mousePosition);
		if (hitObject){
			WorldObject worldObject = hitObject.transform.GetComponentInParent< WorldObject > ();
			if (worldObject) {
				player.FigureOutSelectionAfterLeftClickOnThisWorldObject(worldObject); 
			}
			if(player.selectedObjects.Count == 1 && player.selectedObjects[0] != null && player.selectedObjects[0].paramManager.PlayerOwned) {
				player.selectedObjects[0].MouseClickLeft(WorkManager.FindHitPoint(Input.mousePosition));
			}
		}
	}
		


	private void RightMouseClickPre() {
		if(player && !Input.GetKey(KeyCode.LeftAlt) && player.selectedObjects.Count != 0 && player.selectedObjects[0] != null) {
			WorldObject selectedWO = player.selectedObjects [0].GetComponent<WorldObject> ();
			GameObject hitObject; 
			Vector3 hitPoint; 
			if (player.selectedObjects.Count == 1 && selectedWO != null) {
				RaycastHit closestHit = WorkManager.FindNonSelfHitObject (Input.mousePosition, selectedWO);
				if (closestHit.collider == null){
					return; 
				}
				hitObject = closestHit.collider.gameObject;
				hitPoint = closestHit.point; 
			} else {
                hitObject = WorkManager.FindHitObject(Input.mousePosition);
				hitPoint = WorkManager.FindHitPoint(Input.mousePosition);
			}
			for (int i = 0; i < player.selectedObjects.Count; i++) {
                if (player.selectedObjects[i] != null && player.selectedObjects[i].paramManager.PlayerOwned){
					player.selectedObjects[i].MouseClickRight (hitObject, hitPoint);
				}
			}
		}
	}

	private void MouseHover() {
		if (player && player.dragSelectionHandlerScript && player.cursorManagerScript){
            
            if (player.dragSelectionHandlerScript.IsLeftClickDragInputOccurring()) {
                player.cursorManagerScript.RegisterCursorState (CursorState.DefaultCursor); 
                return;
            }

			GameObject hoverObject = WorkManager.FindHitObject (Input.mousePosition);
            WorldObject hoveredWorldObject = null; 
            if (hoverObject){
                hoveredWorldObject = hoverObject.GetComponentInParent<WorldObject>();
            }
			if (hoveredWorldObject == null) {
				player.cursorManagerScript.RegisterCursorState (CursorState.DefaultCursor); 
				return;
			}
			if (hoveredWorldObject) {
				hoveredWorldObject.HoverEffect = true; 
				Player hoverPlayer = hoveredWorldObject.transform.root.GetComponent<Player> ();
				EnemyManager hoverEnemyManager = hoveredWorldObject.transform.root.GetComponent<EnemyManager> ();
				if (hoverPlayer == null && hoverEnemyManager == null) {
					player.cursorManagerScript.RegisterCursorState (CursorState.DefaultCursor); 
					return; 
				}
				if (hoverPlayer && player == hoverPlayer) {
					player.cursorManagerScript.RegisterCursorState (CursorState.AllyCursor); 
					return;
				}
				if (hoverEnemyManager || (hoverPlayer && player != hoverPlayer)) {
					player.cursorManagerScript.RegisterCursorState (CursorState.EnemyCursor); 
					return; 
				}
			}
		}
	}
	//*******************************
	//END MOUSE ACTIVITY
	//*******************************


	//*******************************
	//BEGIN CAMERA ACTIVITY
	//*******************************

	private float cameraMovementTimer;
	private bool onRight;
	private bool onLeft; 
	private void RotateCamera() {
		float xpos = Input.mousePosition.x;
		if (xpos < 10){
			if (onLeft == false){
				cameraMovementTimer = 0f; 
				onLeft = true;
				onRight = false;
			}
			cameraMovementTimer += Time.deltaTime; 
				cameraControl.RotateCamera(true); 
		}
		else if (xpos > (Screen.width - 10)){
			if (onRight == false){
				cameraMovementTimer = 0f; 
				onRight = true;
				onLeft = false;
			}
			cameraMovementTimer += Time.deltaTime; 
			cameraControl.RotateCamera(false); 
		}
		else if (cameraMovementTimer > 0f){
			cameraMovementTimer = 0f; 
		}
	}

	private void CameraZoom(){
		if (Input.mouseScrollDelta.y > 0){
			cameraControl.ZoomCamera(true); 
		}
		if (Input.mouseScrollDelta.y < 0){
			cameraControl.ZoomCamera(false); 
		}
	}



	//*******************************
	//END CAMERA ACTIVITY
	//*******************************
			



}


