using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider musicSlider;
    public Slider efectSlider;


    // Start is called before the first frame update
    public void VolumenMusiquita()
    {
        mixer.SetFloat("musica",musicSlider.value);

    }

    public void VolumenEfectos()
    {
        mixer.SetFloat("efectos", efectSlider.value);

    }

}
