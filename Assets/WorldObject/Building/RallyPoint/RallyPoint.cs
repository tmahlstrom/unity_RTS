using UnityEngine;
using System.Collections;

public class RallyPoint : MonoBehaviour {

	public void Enable () {
		Renderer[] renderers = GetComponentsInChildren< Renderer >();
		foreach(Renderer renderer in renderers) renderer.enabled = true;
		Light[] lights = GetComponentsInChildren<Light> ();
		foreach(Light light in lights) light.enabled = true;
	}

	public void Disable () {
		Renderer[] renderers = GetComponentsInChildren< Renderer >();
		foreach(Renderer renderer in renderers) renderer.enabled = false;
		Light[] lights = GetComponentsInChildren<Light> ();
		foreach(Light light in lights) light.enabled = false;
	}
}
