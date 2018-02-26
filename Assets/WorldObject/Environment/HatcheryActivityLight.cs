using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatcheryActivityLight : MonoBehaviour {

    private Material material; 

    private void Awake(){
        Renderer rend = GetComponent<Renderer>();
        if (rend){
            material = rend.material; 
        }
    }

    public void LightActivity(bool toggle){
        if (material){
            if (toggle == true){
                material.EnableKeyword("_EMISSION");
            } else{
                material.DisableKeyword("_EMISSION");
            }
        }
    }

}
