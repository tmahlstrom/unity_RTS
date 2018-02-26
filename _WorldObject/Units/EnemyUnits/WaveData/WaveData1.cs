using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class WaveData1 : WaveData {



	protected FieldInfo[] waveArray; 
	protected int numberOfWaves; 

	public List<GameObject> wave1 = new List<GameObject>();
	public List<GameObject> wave2 = new List<GameObject>();
	public List<GameObject> wave3 = new List<GameObject>();
	public List<GameObject> wave4 = new List<GameObject>();
	public List<GameObject> wave5 = new List<GameObject>();
	public List<GameObject> wave6 = new List<GameObject>();
	public List<GameObject> wave7 = new List<GameObject>();
	public List<GameObject> wave8 = new List<GameObject>();
	public List<GameObject> wave9 = new List<GameObject>();
	public List<GameObject> wave10 = new List<GameObject>();

	private List<List<GameObject>> listOfWaves = new List<List<GameObject>>(); 


	protected override void Awake (){

		if (wave1.Count > 0) {
			listOfWaves.Add (wave1);
		}
		if (wave2.Count > 0) {
			listOfWaves.Add (wave2);
		}
		if (wave3.Count > 0) {
			listOfWaves.Add (wave3);
		}
		if (wave4.Count > 0) {
			listOfWaves.Add (wave4);
		}
		if (wave5.Count > 0) {
			listOfWaves.Add (wave5);
		}
		if (wave6.Count > 0) {
			listOfWaves.Add (wave6);
		}
		if (wave7.Count > 0) {
			listOfWaves.Add (wave7);
		}
		if (wave8.Count > 0) {
			listOfWaves.Add (wave8);
		}
		if (wave9.Count > 0) {
			listOfWaves.Add (wave9);
		}
		if (wave10.Count > 0) {
			listOfWaves.Add (wave10);
		}
		numberOfWaves = listOfWaves.Count; 
	}


	public override List<GameObject> GetWaveObjectList (int waveNumber){
		if (waveNumber <= numberOfWaves) {
			int locationInArray = waveNumber - 1;
			return listOfWaves [locationInArray];
		}
		return new List<GameObject>();
	}

	

}
	
