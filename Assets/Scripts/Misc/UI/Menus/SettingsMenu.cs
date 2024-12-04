using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Assets.Scripts.Scriptable_Objects;
using Assets.Scripts.Misc.UI;
using System;
using Assets.Scripts.Menus;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Misc.Menus
{
    public class SettingsMenu : BaseUI
    {
        private class Volumes
        {
            public float currentMaster, currentSFX, currentMusic;
            public Volumes(float master, float sfx, float music)
            {
                currentMaster = master;
                currentSFX = sfx;
                currentMusic = music;
            }
            public Volumes()
            {
                currentMaster = 0;
                currentSFX = 0;
                currentMusic = 0;
            }
            public void SetVolume(float volume, VolumeType type)
            {
                switch (type)
                {
                    case VolumeType.Master:
                        currentMaster = volume;
                        break;
                    case VolumeType.SFX:
                        currentSFX = volume;
                        break;
                    case VolumeType.Music:
                        currentMusic = volume;
                        break;
                }
            }
        }


        private Volumes volumes = new Volumes();
        public enum WindowMode
        {
            FullScreen = 0,
            Windowed = 1,
            Borderless = 2
        }
        [SerializeField]
        public enum VolumeType
        {
            Master = 0,
            SFX = 1,
            Music = 2
        }
        public AudioMixer audioMixer;
        public TMP_Dropdown resolutionDropdown;
        public TMP_Dropdown windowModeDropdown;

        public Slider masterVolume;
        public Slider SFXVolume;
        public Slider MusicVolume;


        public Toggle fullscreenToggle;

        Resolution[] resolutions;


        private float currentMasterVolume, currentResolution;
        private bool oldFullscreen, currentFullscreen;


        private float defaultVolume = 0f, defaultResolution;
        private bool defaultFullscreen = true;
        private void Start()
        {
            LoadSettings();
            resolutions = Screen.resolutions;

            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();
            foreach (Resolution resolution in resolutions)
            {
                options.Add(resolution.width + " x " + resolution.height);
            }

            resolutionDropdown.AddOptions(options);



            fullscreenToggle.isOn = currentFullscreen;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private float LoadVolume(VolumeType type)
        {
            return PlayerPrefs.GetFloat($"{type.ToString()}Volume", defaultVolume);
        }


        public void SetVolume(float volume, VolumeType type)
        {
            switch (type)
            {
                case VolumeType.Master:
                case VolumeType.SFX:
                case VolumeType.Music:
                    audioMixer.SetFloat($"{type.ToString()}Volume", volume);
                    volumes.SetVolume(volume, type);
                    break;
            }
        }

        public void SetMasterVolume(float volume)
        {
            SetVolume(volume, VolumeType.Master);
        }
        public void SetSFXVolume(float volume)
        {
            SetVolume(volume, VolumeType.SFX);
        }

        public void SetMusicVolume(float volume)
        {
            SetVolume(volume, VolumeType.Music);
        }
        public void SetWindowMode(int mode)
        {

            Screen.fullScreenMode = (FullScreenMode)mode;
            //UnityEngine.Debug.Log($"Window mode set to {mode}");
        }
        public void SetFullscreen(bool isFullscreen)
        {
            currentFullscreen = isFullscreen;
            //oldFullscreen = isFullscreen;
            Screen.fullScreen = currentFullscreen;
        }

        public void SetResolution(int resolutionIndex)
        {
            Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen);
        }



        public void SaveSettings()
        {
            DebugLogger.Log(DebugData.DebugType.UI, "Saving settings");
            PlayerPrefs.SetFloat($"{VolumeType.Master.ToString()}Volume", volumes.currentMaster);
            PlayerPrefs.SetFloat($"{VolumeType.SFX.ToString()}Volume", volumes.currentSFX);
            PlayerPrefs.SetFloat($"{VolumeType.Music.ToString()}Volume", volumes.currentMusic);


            PlayerPrefs.SetFloat("fullscreen", currentFullscreen ? 1 : 0);

            PlayerPrefs.Save();
            DebugLogger.Log(DebugData.DebugType.UI, "Settings saved");

        }

        private void LoadVolumes()
        {
            volumes = new Volumes(
            LoadVolume(VolumeType.Master),
            LoadVolume(VolumeType.SFX),
            LoadVolume(VolumeType.Music));
            SetVolume(volumes.currentMaster, VolumeType.Master);
            SetVolume(volumes.currentSFX, VolumeType.SFX);
            SetVolume(volumes.currentMusic, VolumeType.Music);

            masterVolume.value = volumes.currentMaster;
            SFXVolume.value = volumes.currentSFX;
            MusicVolume.value = volumes.currentMusic;
        }
        public void LoadSettings()
        {
            DebugLogger.Log(DebugData.DebugType.UI, "Loading settings");
            //load sfx

            LoadVolumes();
            //volumes.currentMaster = PlayerPrefs.GetFloat("MasterVolume", defaultVolume);
            //load master
            //volumes.currentSFX = PlayerPrefs.GetFloat("SFXVolume", defaultVolume);

            //audioMixer.SetFloat("SFXVolume", volumes.currentSFX);
            //audioMixer.SetFloat("MasterVolume", volumes.currentMaster);
            //audioMixer.SetFloat("MusicVolume", volumes.currentMaster);




            int hi = defaultFullscreen ? 1 : 0;
            currentResolution = PlayerPrefs.GetFloat("resolution", defaultResolution);
            currentFullscreen = PlayerPrefs.GetFloat("fullscreen", defaultFullscreen ? 1 : 0) == 1;

            DebugLogger.Log(DebugData.DebugType.UI, $"Current Volumes: Master: {volumes.currentMaster}, SFX: {volumes.currentSFX}");
            DebugLogger.Log(DebugData.DebugType.UI, "Settings loaded");

        }


        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            LoadSettings();
        }
        public override void Open()
        {
            base.Open();
            LoadSettings();
        }

        public override void Close()
        {
            base.Close();
            SaveSettings();
        }

    }
}
