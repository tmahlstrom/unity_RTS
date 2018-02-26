using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour {

	private GridCollider gridCollider; 
	private int gridWidth;
	private int gridHeight; 
	private int gridLeftMost; 
	private int gridBottomMost; 
	private Vector3 gridPosition; 
	private const float tileSize = 0.5f; //this currently only works with this tile size (see the double for loop)
    private Vector3 mouseOffset; 

	private float selectionX = -9999;
	private float selectionZ = -9999;
	private float selectionY = -9999;


	private void Awake(){
		BoxCollider collider = GetComponent<BoxCollider>(); 
        mouseOffset = new Vector3(-20, -5, 0); 
		if (collider) {
			gridWidth = Mathf.RoundToInt(collider.size.x);
			gridHeight = Mathf.RoundToInt(collider.size.z);
			gridPosition = collider.gameObject.transform.position; 
			gridLeftMost = Mathf.RoundToInt(gridPosition.x + gridWidth * -0.5f);
			//Debug.Log(gridLeftMost); 
            gridBottomMost = Mathf.RoundToInt(gridPosition.z + gridHeight * -0.5f);
			//Debug.Log(gridBottomMost); 

		}
	}


	private void Update (){
		UpdateSelection ();
		DrawGrid();
		DrawSelection (); 
	}

	private void OnDisable(){
		selectionX = -9999;
		selectionZ = -9999;
	}

	private void UpdateSelection (){
		if (!Camera.main){
			return; 
		}
		RaycastHit hit; 
        if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition + mouseOffset), out hit, 50.0f, LayerMask.GetMask ("Grid"))) {
            selectionX = Mathf.Floor(2.0f * hit.point.x) / 2.0f;
			//Debug.Log(selectionX);
            if (selectionX < gridLeftMost){
				Debug.Log(gridLeftMost);
                selectionX = -9999; 
            }
            if (selectionX == gridLeftMost + gridWidth) {
				Debug.Log(gridLeftMost + "2");
                selectionX = -9999;
            }

			selectionZ = Mathf.Floor(2.0f * hit.point.z) / 2.0f;
			//Debug.Log(selectionZ);
            if (selectionZ < gridBottomMost) {
								Debug.Log(gridLeftMost + "sdf ");

                selectionZ = -9999;
            }
            if (selectionZ == gridBottomMost + gridHeight) {
												Debug.Log(gridLeftMost + "sdfwerqwerqwrqwre ");

                selectionZ = -9999;
            }
			selectionY = Mathf.Floor(2.0f * hit.point.y) / 2.0f;

        } else {
			selectionX = -9999;
			selectionZ = -9999;
		}

	}

	private void DrawGrid(){
		Vector3 totalWidth = Vector3.right * gridWidth;
		Vector3 totalHeight = Vector3.forward * gridHeight;
		Vector3 tileWidth = Vector3.right * tileSize;
		Vector3 tileHeight = Vector3.forward * tileSize;
		for (int i = gridLeftMost; i <= gridLeftMost + (gridWidth * (1/tileSize)); i++) {
			Vector3 start = tileWidth * i + new Vector3(0, gridPosition.y, gridBottomMost) + gridLeftMost * tileWidth;
			Debug.DrawLine (start, start + totalHeight); 
			for (int j = gridBottomMost; j <= gridBottomMost + (gridHeight* (1/tileSize)); j++) {
				start = tileHeight * j + new Vector3(gridLeftMost ,gridPosition.y,0) + gridBottomMost * tileHeight; 
				Debug.DrawLine (start, start + totalWidth); 
			}
		}
	}

	private void DrawSelection(){
		if (selectionX >= -9998 && selectionZ >= -9998) {
			Debug.DrawLine(
				Vector3.forward  * selectionZ + Vector3.right * selectionX,
				Vector3.forward  * (selectionZ + tileSize) + Vector3.right * (selectionX + tileSize)); 
		}
	}

	public Vector3 GetGridLocation(){
		if (selectionX >= -9998 && selectionZ >= -9998) {
			return new Vector3 (selectionX + tileSize, gridPosition.y, selectionZ + tileSize);
		}
		return new Vector3(0,-100,0) ; 
	}
}
