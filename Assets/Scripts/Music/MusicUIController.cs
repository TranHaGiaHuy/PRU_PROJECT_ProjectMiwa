using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MusicUIController: MonoBehaviour
{
    private void Awake()
    {
        MusicVolume();
        SFXVolume();
    }

    public Slider _musicSlider, _sfxSlider;
    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
    }
    public void TogglSFX()
    {
        AudioManager.Instance.ToggleSFX();
    }
    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(_musicSlider.value);
    }
    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolume(_sfxSlider.value);
    }
}

