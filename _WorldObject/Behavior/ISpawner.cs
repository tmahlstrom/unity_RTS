using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawner {

	bool ReadyToBeginSpawning();
	void InitiateSpawnProcess(); 

	void ConcludeSpawnProcess();
	bool IsSpawnProcessStarted();
	void SpawnClimaxEvent();
	void SubtractDeadSpawnFromPopulationCount();
	void AddNewSpawnToPopulationCount();

	bool ReadyToSpawnerSpecial();
	void InitiateSpawnerSpecial();
	void ConcludeSpawnerSpecial();

}
