using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class InterceptSpawnLaser : MonoBehaviour {

	public bool isShowingLaser = false; //externally, only used for the tutoral right now
	private LineRenderer lineRenderer;
	private IEnumerator coroutine;
//	ParticleSystem laserParticleSystem; 
//	ParticleSystem.Particle []ParticleList;
//	InterceptSpawn interceptSpawn; 
//	GameObject laserParticleObject; 
	Collider targetCollider; 
	private InterceptSpawn interceptSpawn;


	void Awake (){
		lineRenderer = GetComponent<LineRenderer> (); 
		interceptSpawn = GetComponent<InterceptSpawn> ();
//		laserParticleSystem = GetComponentInChildren<ParticleSystem> ();
//		if (laserParticleSystem) {
//			ParticleList = new ParticleSystem.Particle[laserParticleSystem.maxParticles];
//		}
//		interceptSpawn = GetComponent<InterceptSpawn> (); 
	}
		
	public void ShowLaserFromToCollider (Vector3 startPoint, Vector3 endPoint, Collider targetCollider){
		this.targetCollider = targetCollider;
		if (this.targetCollider != null) {
			if (coroutine == null){
				coroutine = ShowLaserToPointCoroutine (startPoint, endPoint); 
				StartCoroutine (coroutine);
			}
		}
	}

	private IEnumerator ShowLaserToPointCoroutine (Vector3 startPoint, Vector3 endPoint){
		if (isShowingLaser) {
			yield return 0;
		}
		if (targetCollider != null) {
			isShowingLaser = true; 
			lineRenderer.SetPosition (0, this.transform.position);
			lineRenderer.SetPosition (1, targetCollider.transform.position);
			lineRenderer.enabled = true;
			interceptSpawn.ExecuteDestructionOfTargetColliderSpawn (targetCollider); 
//		laserParticleObject = Instantiate(ResourceManager.GetWorldObject("LaserParticle"), transform.position, transform.rotation);
//		laserParticleObject.transform.LookAt (targetCollider.transform);
//		Destroy (laserParticleObject.gameObject, 1); 

			yield return new WaitForSeconds (0.02f);
			TurnOffLaser ();
			isShowingLaser = false; 
			coroutine = null; 
		}

		//		if (laserParticleSystem) {
		//			ShootParticle ();
		//		}
	}

//	private void ShootParticle(){
//		laserParticleSystem.Emit (1);
//		laserParticleSystem.GetParticles (ParticleList);
//		for (int i = 0; i < ParticleList.Length; ++i) {
//			ParticleList [i].position = Vector3.zero; 
//		}
//		laserParticleSystem.SetParticles (ParticleList, laserParticleSystem.particleCount);
//	}

	private void TurnOffLaser (){
		lineRenderer.enabled = false;
	}


}
