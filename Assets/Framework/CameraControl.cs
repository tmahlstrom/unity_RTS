using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {


	public float moveSpeed = 2f;
	public float rotateSpeed = 2f;
	public CameraPositions[] positions;
	private Transform targetTransform; 
	private int positionNumber; 
	private bool movementNeeded; 
	private bool ableToRotate = false; 
	public float rotationSpeed = 15; 
	private int continuationCounter; 
	public float minZoomDistance = 21; 
	public float maxZoomDistance = 12;


	public delegate void CameraArrived();
	public event CameraArrived OnCameraArrival;


	private static CameraControl instance; 
	public static CameraControl Instance {
		get {
			if (instance == null){
				instance = GameObject.FindObjectOfType<CameraControl>(); 
			} 
			if (instance == null){
				Debug.Log("No camera control found"); 
			}
			return instance; 
		}
	}

	private void Start(){
		if (positions != null && positions.Length > 0) {
			transform.position = positions[0].subPositions[0].position;
			transform.rotation = positions[0].subPositions[0].rotation;
			//UpdateCameraTargetPosition(false);
			//movementNeeded = true;
		}
	}

	void LateUpdate () {
		if (movementNeeded){
			transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, Time.deltaTime * moveSpeed);//Mathf.Clamp((targetTransform.position - transform.position).sqrMagnitude * 8, .2f, .2f));
         	transform.rotation = Quaternion.RotateTowards(transform.rotation, targetTransform.rotation, Time.deltaTime * rotateSpeed);
			if ((transform.position - targetTransform.position).sqrMagnitude < 0.1f){
				if (Quaternion.Angle(transform.rotation, targetTransform.rotation) < 1.0f){
					movementNeeded = false;
					if (continuationCounter > 0){
						continuationCounter -= 1; 
						UpdateCameraTargetPosition(); 
					} else {
						if (targetTransform.gameObject.name == "Battle"){
							ableToRotate = true; 
						}
						if (OnCameraArrival != null){
							OnCameraArrival(); 
						}
					}
				}
			}
		}
	}

	public void UpdateCameraTargetPosition(float moveS = 99f, int continueUpdateFor = 0){
		if (positions.Length > positionNumber + 1){
			targetTransform = positions[positionNumber + 1].subPositions[0];
			if (moveS == 99){
				moveS = moveSpeed;
			}
			float requiredTime = Vector3.Distance(transform.position, targetTransform.position) / moveS;
			float rotationNeeded = Quaternion.Angle(transform.rotation, targetTransform.rotation);
			moveSpeed = moveS;
			rotateSpeed = rotationNeeded / requiredTime; 
			positionNumber += 1;
			movementNeeded = true;
			ableToRotate = false; 
			continuationCounter = continueUpdateFor; 
		}
	}

	public void RotateCamera (bool clockwise){
		//Debug.Log(positionNumber); 
		if (ableToRotate){
			if (clockwise == true){
				transform.RotateAround(new Vector3(24,20,64), Vector3.up, Time.deltaTime * rotationSpeed); 
			} else {
				transform.RotateAround(new Vector3(24,20,64), Vector3.up, Time.deltaTime * -rotationSpeed); 
			}
		}
	}

	public void ZoomCamera (bool zoomIn){
		if (ableToRotate){
			if (zoomIn == true && Vector3.Distance(transform.position, new Vector3(24,20,64)) > minZoomDistance){
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(24,20,64), Time.deltaTime * rotationSpeed); 
				transform.RotateAround(new Vector3(24,20,64), transform.right, Time.deltaTime * -rotationSpeed * 4);
			} else if (zoomIn == false && Vector3.Distance(transform.position, new Vector3(24,20,64)) < maxZoomDistance) {
				Vector3 dif = transform.position - new Vector3(24,20,64); 
				transform.position = Vector3.MoveTowards(transform.position, transform.position - dif, Time.deltaTime * -rotationSpeed);
				transform.RotateAround(new Vector3(24,20,64), transform.right, Time.deltaTime * rotationSpeed * 4);
			}
		}
	}

}
