using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {




    private void OnDisable(){
        Collider col = GetComponent<Collider>();
        if (col){
            col.enabled = false; 
        }
    }

    private void OnEnable(){
        Collider col = GetComponent<Collider>();
        if (col){
            col.enabled = true; 
        }
    }

}
