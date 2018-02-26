using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionIndicatorOrb : MonoBehaviour {



    //public float FadeOutDuration = 2.0f; 


    //Awake(){
    //    if (selectionIndicatorOrb) { //this was for the selection orb
    //        startOrbColor = selectionIndicator.gameObject.GetComponent<Renderer> ().material.color;
    //        endOrbColor = Color.clear;
    //    }
    //}
    //public void Start (){ // orb stuff
    //    if (selectionIndicatorOrb) {
    //        selectionIndicatorOrb.gameObject.GetComponent<Renderer> ().material.SetColor ("_Color", endOrbColor);
    //    }
    //}





    //public void Enable () {
    //    currentlyEnabled = true;
    //    if (selectionIndicatorOrb) {
    //        selectionIndicatorOrb.gameObject.GetComponent<Renderer> ().material.SetColor ("_Color", startOrbColor);     
    //    }

    //    Light[] lights = GetComponentsInChildren< Light >();
    //    foreach(Light light in lights) light.enabled = true;

    //    Projector[] projectors = GetComponentsInChildren< Projector >();
    //    foreach(Projector projector in projectors) projector.enabled = true;
    //}

    //Color currentColor; 
    //if (selectionIndicator) {
    //    for (float t = 0.0f; t < FadeOutDuration; t += Time.deltaTime) {
    //        currentColor = Color.Lerp (startOrbColor, endOrbColor, t / FadeOutDuration);
    //        selectionIndicator.gameObject.GetComponent<Renderer> ().material.SetColor ("_Color", currentColor);
    //        if (currentlyEnabled == true){
    //            selectionIndicator.gameObject.GetComponent<Renderer> ().material.SetColor ("_Color", startOrbColor);
    //            yield break; 
    //        }
    //        yield return 0.05f;
    //    }
    //}



    //public void Disable (){
    //    currentlyEnabled = false;
    //    StartCoroutine ("DisableCoroutine"); 
    //}

    //public IEnumerator DisableCoroutine () {


    //    Light[] lights = GetComponentsInChildren< Light >();
    //    foreach(Light light in lights) light.enabled = false;

    //    Projector[] projectors = GetComponentsInChildren< Projector >();
    //    foreach(Projector projector in projectors) projector.enabled = false;


    //}


}
