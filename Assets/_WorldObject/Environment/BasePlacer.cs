using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlacer : MonoBehaviour {

	public GameObject structureBase;
	public bool intersectPlacement;
	public MultiDimensionalInt[] placementArray; 



	private GridSystem grid; 

	private void Awake(){
		grid = GetComponent<GridSystem>();
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
