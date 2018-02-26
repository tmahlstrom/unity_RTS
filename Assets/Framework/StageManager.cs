using UnityEngine;
using System.Collections;
using RTS;
using System.Net;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO; 



public class StageManager : MonoBehaviour {

	public static StageManager Instance { get; private set;}
	private bool waveWasSent = false;
	private bool needToSend = false; 
	public Dictionary<Player, int> playerUnitCountDict = new Dictionary<Player, int>();
	public Dictionary<EnemyManager, int> enemyManagerUnitCountDict = new Dictionary<EnemyManager, int>();

	public delegate void WaveCompleted (); 
	public static event WaveCompleted AWaveHasBeenCompleted;

	private bool regularWaveSendActive = false; 
	private bool repeatingWaveActive = false; 
	public int currentWave; 
	public int wavesToComplete; 
	private float spawnRate = 12f; 
	private Coroutine waveMonitorCoroutine; 

	public List<WorldObject> allSelectables = new List<WorldObject>();
	public List<Player> listOfPlayers = new List<Player>();
	public List<Player> listOfPlayersWithWorldInfoCanvas = new List<Player>();
	public List<EnemyManager> listOfEnemyManagers = new List<EnemyManager>();

	private void Awake (){
		Instance = this;
	}


	private void Start () {
		StartUpdateStateOfPlayers ();
		StartUpdateStateOfEnemyManagers ();
		waveMonitorCoroutine = StartCoroutine ("WaveMonitor");
	}

	private void ResetWaveMonitor(){
		if (waveMonitorCoroutine!= null){
			StopCoroutine(waveMonitorCoroutine); 
		}
		waveMonitorCoroutine = StartCoroutine(WaveMonitor()); 
	}


	private IEnumerator WaveMonitor(){
		while (this != null) {
			if ((!AreThereEnemyUnits () && !AreThereEnemySpawns() && (waveWasSent || needToSend)) || (needToSend && currentWave == 0)) {

				if (waveWasSent && AWaveHasBeenCompleted != null) {
					AWaveHasBeenCompleted ();
					waveWasSent = false;
					if (repeatingWaveActive){
						needToSend = true;
					}
					if (CheckForWaveCompletionVictory()){
						if (waveMonitorCoroutine!= null){
							StopCoroutine(waveMonitorCoroutine); 
						}
					}
					if (regularWaveSendActive){
						yield return new WaitForSeconds(spawnRate);
					}
				}


				if (regularWaveSendActive){
					currentWave++;
					MainCanvas.Instance.UpdateWaveText(); 
					StartCoroutine("TriggerCurrentWave");
				}

				if (repeatingWaveActive){
					StartCoroutine("TriggerCurrentWave");
				}
			}
			yield return new WaitForSeconds (2); 
		}
	}



		
	private IEnumerator TriggerCurrentWave () {
		needToSend = false; 
		foreach (KeyValuePair<EnemyManager, int> entry in enemyManagerUnitCountDict) {
			entry.Key.SendWave (currentWave); 
		}
		yield return new WaitForSeconds(10.0f);
		waveWasSent = true;
	}



	public void ToggleWaveActivity(bool toggle){
		Debug.Log("where from ");
		regularWaveSendActive = toggle; 
		needToSend = true;
	}

	public void SendSingleWave(){
		StartCoroutine(SingleWave());
	}
	private IEnumerator SingleWave(){
		regularWaveSendActive = true;
		needToSend = true; 
		yield return new WaitForSeconds(3);
		regularWaveSendActive = false;
	}





	public void StartRepeatingWave(){
		regularWaveSendActive = false;
		ResetWaveMonitor();
		currentWave++;
		if (AWaveHasBeenCompleted != null) {
			AWaveHasBeenCompleted (); 
		}
		repeatingWaveActive = true; 
		needToSend = true;
	}
	public void EndRepeatingWave(){
		repeatingWaveActive = false;
		needToSend = false;
		ResetWaveMonitor(); 
	}


	public void FixedSpawnRate(float rate){
		spawnRate = rate; 
	}







    //*************************************
    //BEGIN COUNT STUFF METHODS
	//*************************************
	private bool AreThereEnemyUnits (){
		foreach (KeyValuePair<EnemyManager, int> entry in enemyManagerUnitCountDict) {
			if (entry.Value > 0) {
				return true; 
			}
		}
		return false;
	}

	private bool AreThereEnemySpawns() {
		foreach (KeyValuePair<EnemyManager, int> entry in enemyManagerUnitCountDict) {
			Spawns spawns = entry.Key.GetComponentInChildren<Spawns>();
			if (spawns) {
				Spawn singleSpawn = spawns.GetComponentInChildren<Spawn>();
				if (singleSpawn) {
					return true;
				}
			}
		}
		return false;
	}

	public void StartUpdateStateOfPlayers(){
		UpdatePlayerUnitDict();
	}

	private void UpdatePlayerUnitDict(){
		foreach (Player player in listOfPlayers){
			if (player) {
				int unitCount = 0;
				Units units = player.GetComponentInChildren<Units> ();  
				if (units) {
					unitCount = units.transform.childCount; 
				}
				playerUnitCountDict.Add (player, unitCount);
			}
		}
	}

	private void StartUpdateStateOfEnemyManagers(){
		UpdateEnemyManagerUnitDict();
	}
		
	private void UpdateEnemyManagerUnitDict(){
		foreach (EnemyManager enemyManager in listOfEnemyManagers){
			if (enemyManager) {
				int unitCount = 0;
				Units units = enemyManager.GetComponentInChildren<Units> ();  
				if (units) {
					unitCount = units.transform.childCount; 
				}
				enemyManagerUnitCountDict.Add (enemyManager, unitCount);
			}
		}
	}

	public void DealWithUnitCountChange(Unit unit, int countChange){
		if (unit.player && playerUnitCountDict.ContainsKey(unit.player)) {
			playerUnitCountDict [unit.player] += countChange; 
		}
		if (unit.enemyManager && enemyManagerUnitCountDict.ContainsKey (unit.enemyManager)) {
			enemyManagerUnitCountDict [unit.enemyManager] += countChange; 
		}
	}
    //*************************************
    //END COUNT STUFF METHODS
	//*************************************

    //*************************************
    //BEGIN END SCENE METHODS
	//*************************************

	public void DefeatConditionsAreMet(){
		StartCoroutine(DefeatCoroutine()); 

	}

	private IEnumerator DefeatCoroutine(){
		if (TutorialManager.Instance != null){
			TutorialManager.Instance.gameObject.SetActive(false); 
		}
		yield return new WaitForSeconds(2.0f); 
		MainCanvas.Instance.DefeatNotification(); 
		yield return new WaitForSeconds(1.5f); 
		MainCanvas.Instance.FadeOut(.12f);
		yield return new WaitForSeconds(5.0f); 
		SceneManager.LoadScene("TitleScreen");
	}


	private bool CheckForWaveCompletionVictory(){
		if (currentWave >= wavesToComplete){
			VictoryConditionsAreMet();
			return true; 
		}
		return false; 
	}

	public void VictoryConditionsAreMet(){
		SceneControl.Instance.VictoryDataChange(); 
		Save(); 
		StartCoroutine(VictoryCoroutine()); 
	}
	private IEnumerator VictoryCoroutine(){
		if (TutorialManager.Instance != null){
			TutorialManager.Instance.gameObject.SetActive(false); 
		}
		yield return new WaitForSeconds(1.0f); 
		MainCanvas.Instance.VictoryNotification(); 
		yield return new WaitForSeconds(1.5f); 
		MainCanvas.Instance.FadeOut(.12f);
		yield return new WaitForSeconds(5.0f); 
		SceneManager.LoadScene("TitleScreen");
	}


	protected virtual void Load(){
		PlayerData playerData = FindObjectOfType<PlayerData>();
		if (playerData){
			playerData.Load(); 
		}
	}

	protected virtual void Save(){
		PlayerData playerData = FindObjectOfType<PlayerData>();
		if (playerData){
			playerData.Save(); 
		}
	}

    //*************************************
    //END END SCENE METHODS
	//*************************************



}

