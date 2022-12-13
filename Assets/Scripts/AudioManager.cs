using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance { get; private set; }
    // Start is called before the first frame update
    void Awake() {
        
        //make sure there is only one audioManager regardless of scene
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }

        //Let audioManager persist between scenes to prevent music from being cut off
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        //Debug.Log("AudioManager loaded");
    }

    public void Play (string name){
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null){
            Debug.Log("Sound " + name + " does not exist. Please check your audio sources");
            return;
        }
        if (s.source == null){
            Debug.Log("AudioManager: s.source is null. Audio Name: " + s.name);
        }
        s.source.Play();
    }
}
