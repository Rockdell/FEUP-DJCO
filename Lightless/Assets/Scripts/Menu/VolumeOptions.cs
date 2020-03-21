using UnityEngine;
using UnityEngine.UI;

public class VolumeOptions : MonoBehaviour {

    public Slider musicVolume;
    public Slider efxVolume;

    void Start() {
        float tmp;
        GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().masterMixer.GetFloat("MusicVol", out tmp);
        musicVolume.value = Mathf.Pow(10f, (tmp / 20f));
        GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().masterMixer.GetFloat("EfxVol", out tmp);
        efxVolume.value = Mathf.Pow(10f, (tmp / 20f));
    }

    public void SetAudioManagerMusicLevel(float sliderValue) {
        AudioManager.Instance.SetMusicLevel(sliderValue);
    }

    public void SetAudioManagerEfxLevel(float sliderValue) {
        AudioManager.Instance.SetEfxLevel(sliderValue);
    }

}


