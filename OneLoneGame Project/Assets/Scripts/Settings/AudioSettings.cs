using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour {

    public AudioMixer MainVolume;
    public AudioMixer MusicVolume;
    public AudioMixer EffectsVolume;
	public void setMainVolume(float volume)
    {
        MainVolume.SetFloat("Volume", volume);
    }
    public void setMusicVolume(float volume)
    {
        MusicVolume.SetFloat("Volume", volume);
    }
    public void setEffectsVolume(float volume)
    {
        EffectsVolume.SetFloat("Volume", volume);
    }
}
