using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffect : MonoBehaviour {


	public float effectTime;
	private Renderer rend; 
	public Vector4 effectColor = new Vector4(0.7f, 1, 1, 0.05f); 

	private void Awake(){
		rend = GetComponent<Renderer>(); 
	}

	private void Update(){
	    if(effectTime>0){
        	if(effectTime < 450 && effectTime > 400){
            	rend.sharedMaterial.SetVector("_ShieldColor", effectColor);
        	}        
        effectTime-=Time.deltaTime * 1000;
        rend.sharedMaterial.SetFloat("_EffectTime", effectTime);
    	}
	}

	public void ContactEffect(Vector3 point){
		//foreach (ContactPoint contact in collision.contacts){
			rend.sharedMaterial.SetVector("_ShieldColor", effectColor);
            rend.sharedMaterial.SetVector("_Position", transform.InverseTransformPoint(point));
            effectTime=500;
		//}
	}

	private void OnCollisionEnter(Collision collision){
		Debug.Log(collision.gameObject.name);
		foreach (ContactPoint contact in collision.contacts){
			rend.sharedMaterial.SetVector("_ShieldColor", effectColor);
            rend.sharedMaterial.SetVector("_Position", transform.InverseTransformPoint(contact.point));
            effectTime=500;
		}
	}
}
