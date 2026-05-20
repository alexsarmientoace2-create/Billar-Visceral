using UnityEngine;
using UnityEngine.Audio; // Requerido para el Mixer
using UnityEngine.UI;    // Requerido para el Slider

public class SettingsManager : MonoBehaviour
{
    public AudioMixer myMixer;
    public Slider volumeSlider;

    public void SetVolume()
    {
        float volume = volumeSlider.value;
        // Convertimos el valor lineal del slider (0-1) a logarítmico (decibelios)
        // La fórmula es: 20 * log10(valor)
        myMixer.SetFloat("MasterVol", Mathf.Log10(volume) * 20);
    }
}