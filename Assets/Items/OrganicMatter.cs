using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganicMatter : Item {

	protected override void OnTriggerEnter(Collider other){
		Builder builder = other.gameObject.GetComponentInParent<Builder> (); 
		if (builder) {
			//builder.unit.ChangeMana (25);
			Destroy (this.gameObject);
		}
	}


}
