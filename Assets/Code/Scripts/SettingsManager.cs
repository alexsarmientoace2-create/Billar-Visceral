using UnityEngine;
using UnityEngine.Audio;


using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer myMixer;
    public Slider volumeSlider;

    public void SetVolume()
    {
        float volume = volumeSlider.value; // El slider debe ir de 0.0001 a 1

        // Multiplicamos por 30 en lugar de 20 y sumamos un offset
        // Esto hará que el rango sea aproximadamente de -80dB a +20dB
        float dB = (Mathf.Log10(volume) * 25) + 5;

        myMixer.SetFloat("MasterVol", dB);
    }
}