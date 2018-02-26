using UnityEngine;
using System.Collections;
using RTS;
using UnityEngine.EventSystems;
using System.IO;

public class Healer : Unit {

	private Quaternion aimRotation;
	public int autoAttackReleaseForce; 
	public float autoBulletRangeLife;


	protected override void Awake () {
		base.Awake ();
	}

	protected override void Start () {
		base.Start ();
	}

	protected override void Update () {
		base.Update();
	}

	protected override void SpacebarPressed(){ 
		base.SpacebarPressed (); 
	}

}
