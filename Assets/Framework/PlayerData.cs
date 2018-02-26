using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;




[Serializable] 
public class SavaData {

	public bool beatNormalDemo; 
	public bool beatHardDemo; 

}

public class PlayerData : MonoBehaviour{

	public bool beatNormalDemo; 
	public bool beatHardDemo; 

	private void OnEnable(){
		Load(); 
	}

	private void OnDisable(){
		Save(); 
	}

	public void Load(){
		if (File.Exists(Application.persistentDataPath + "/playerInfo.dat")){
			BinaryFormatter bf = new BinaryFormatter(); 
			FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.OpenOrCreate); 
			SavaData data = (SavaData)bf.Deserialize(file);
			file.Close(); 

			beatNormalDemo = data.beatNormalDemo;
			beatHardDemo = data.beatHardDemo; 
		}
	}

	public void Save(){
		BinaryFormatter bf = new BinaryFormatter(); 
		FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.OpenOrCreate); 
		SavaData newData = new SavaData(); 
		newData.beatNormalDemo = beatNormalDemo; 
		newData.beatHardDemo = beatHardDemo; 
		bf.Serialize(file,newData); 
		file.Close();
	}


	public void ClearSave(){
		Debug.Log("clear save called");
		if (File.Exists (Application.persistentDataPath + "/playerInfo.dat")){
      		File.Delete(Application.persistentDataPath + "/playerInfo.dat");
			beatNormalDemo = false;
			beatHardDemo = false; 
		}
	}


}
