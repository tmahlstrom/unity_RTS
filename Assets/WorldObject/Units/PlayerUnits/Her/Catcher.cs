using UnityEngine;
using System.Collections;
using RTS;
using UnityEngine.EventSystems;
using System.IO;

public class Catcher: Unit {

	protected override void Awake () {
		base.Awake ();
	}
		
	protected override void Start () {
		base.Start ();
	}

	protected override void Update () {
		base.Update();
	}

	protected override void ManageDeathOfWorldObject(){
		base.ManageDeathOfWorldObject();
		StageManager.Instance.DefeatConditionsAreMet(); 
	}

	protected override void SpacebarPressed(){ 
		base.SpacebarPressed ();
	}

}
