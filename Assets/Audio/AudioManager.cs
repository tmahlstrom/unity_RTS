using UnityEngine.Audio; 
using System; 
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance { get; private set;}
    public Sound[] sounds;
    private AudioSource audioSource; 

    private void Awake() {
        Instance = this;
        audioSource = GetComponent<AudioSource>(); 
    }

    public void Play (AudioClip clip, AudioSource source = null){
        if (source == null){
            source = audioSource; 
        }

        Sound s = Array.Find(sounds, sound => sound.clip == clip); 
        if (s != null){
            source.clip = s.clip;
            source.volume = s.volume;
            source.pitch = s.pitch; 
            source.loop = s.loop; 
            source.Play();
        }
    }


    public void Play (string name, AudioSource source = null){
        if (source == null){
            source = audioSource; 
        }

        Sound s = Array.Find(sounds, sound => sound.name == name); 
        if (s != null){
            source.clip = s.clip;
            source.volume = s.volume;
            source.pitch = s.pitch; 
            source.loop = s.loop; 
            source.Play();
        }
    }


}
