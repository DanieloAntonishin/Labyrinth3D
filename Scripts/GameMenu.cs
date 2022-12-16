using System;
using System.IO;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public static bool isSoundsEnabled { get; private set; } // включение звук эффектов
    public static float soundsVolume { get; private set; }   // должны быть доступны для др. Скриптов


    private static GameObject MenuContent;
    public static GameObject FinishContent;
    private static TMPro.TextMeshProUGUI MenuMessage;
    private static TMPro.TextMeshProUGUI ButtonCaption;
    private static TMPro.TextMeshProUGUI StatMessage;
    private static TMPro.TextMeshProUGUI GameRecordMessage;
    private static float timeScaleNum;

    private AudioSource bgMusic; // ссылка на AudioSource фоновой музыки
    private bool bgMusicEnabled; // текущее состояние ( играет или нет )
    private float bgMusicVolume; // громкость фоновой музыки


    private const string settingsFilename = "Assets/Files/settings.txt";


    #region lifecycle
    void Start()
    {
        MenuContent       = GameObject.Find(nameof(MenuContent));
        FinishContent     = GameObject.Find(nameof(FinishContent));
        MenuMessage       = GameObject.Find(nameof(MenuMessage)).GetComponent<TMPro.TextMeshProUGUI>();
        ButtonCaption     = GameObject.Find(nameof(ButtonCaption)).GetComponent<TMPro.TextMeshProUGUI>();
        StatMessage       = GameObject.Find(nameof(StatMessage)).GetComponent<TMPro.TextMeshProUGUI>();
        GameRecordMessage = GameObject.Find(nameof(GameRecordMessage)).GetComponent<TMPro.TextMeshProUGUI>();
        timeScaleNum      = GameObject.Find("TimeScaleSlider").GetComponent<Slider>().value;

        bgMusic = GetComponent<AudioSource>();
        // получение элем на холсте
        Toggle MusicToggle  = GameObject.Find("MusicToggle").GetComponent<Toggle>();
        Toggle SoundsToggle = GameObject.Find("SoundsToggle").GetComponent<Toggle>();
        Slider MusicSlider  = GameObject.Find("MusicSlider").GetComponent<Slider>();
        Slider SoundsSlider = GameObject.Find("SoundsSlider").GetComponent<Slider>();


        if (File.Exists(settingsFilename) && LoadSettings())
        {

            MusicToggle.isOn = bgMusicEnabled;
            SoundsToggle.isOn = GameMenu.isSoundsEnabled;

            MusicSlider.value = bgMusicVolume;
            SoundsSlider.value = GameMenu.soundsVolume;
        }
        else
        {
            bgMusicEnabled = MusicToggle.isOn;
            bgMusicVolume = MusicSlider.value;

            GameMenu.isSoundsEnabled = SoundsToggle.isOn;
            GameMenu.soundsVolume = SoundsSlider.value;
        }
        if(File.Exists(GameStat.recordFilename)&&GameStat.ReadRecordFile())
        {
            GameRecordMessage.text =
                $"Checkpoint1: {GameStat.BestCheckpoint1Fill.ToString("F1")}s\n" +
                $"Checkpoint2: {GameStat.BestCheckpoint2Fill.ToString("F1")}s\n" +
                $"Checkpoint3: {GameStat.BestCheckpoint3Fill.ToString("F1")}s\n";
        }

        UpdateBgMusic();

        Time.timeScale = MenuContent.activeInHierarchy ? 0.0f : 1.0f;

        FinishContent.SetActive(false); // выкл финишного меню
    }

    void LateUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            GameMenu.Toggle();
        }
    }

    private void OnDestroy()
    {
        SaveSettings();
    }

    #endregion

    #region event handlers
    public void MenuButtonClick()
    {
        GameMenu.Hide();
    }

    public void ExitMenuButtonClick()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void MusicToggleChanged(bool isChecked)
    {
        bgMusicEnabled = isChecked;
        UpdateBgMusic();
    }

    public void MusicVolumeChanged(float value)
    {
        bgMusicVolume = value;
        UpdateBgMusic();
    }
    public void SoundsToggleChanged(bool isChecked)
    {
        GameMenu.isSoundsEnabled = isChecked;
    }
    public void SoundsVolumeChanged(float value)
    {
        GameMenu.soundsVolume = value;
    }

    #endregion

    #region inner methods
    private void SaveSettings()
    {
        System.IO.File.WriteAllText(settingsFilename,
            $"{bgMusicEnabled};{bgMusicVolume};{GameMenu.isSoundsEnabled};{GameMenu.soundsVolume}"
        );
    }
    private bool LoadSettings()
    {
        string[] data = System.IO.File.ReadAllText(settingsFilename).Split(";");
        try
        {
            bgMusicEnabled = Convert.ToBoolean(data[0]);
            bgMusicVolume = Convert.ToSingle(data[1]);
            GameMenu.isSoundsEnabled = Convert.ToBoolean(data[2]);
            GameMenu.soundsVolume = Convert.ToSingle(data[3]);
            return true;
        }
        catch(Exception ex)
        {
            Debug.LogError(ex.Message);
        }
        return false;
    }
    private void UpdateBgMusic()
    {
        bgMusic.volume = bgMusicVolume;
        if (bgMusicEnabled) 
        {
            if( !bgMusic.isPlaying) bgMusic.Play(); 
        }
        else bgMusic.Stop();
    }
    public static void Show(
       string menuMessage = "Game paused",
       string buttonText = "Continue")       // GameMenu.Show()
    {
        Time.timeScale = 0.0f;
        MenuMessage.text = menuMessage;
        ButtonCaption.text = buttonText;
        string cp1Text = GameStat.CheckPoint1Time switch
        {
            -1f => "Failed",
            0 => "No contact",
            _ => GameStat.CheckPoint1Time.ToString("F1")+"s"
        };
        string cp2Text = GameStat.CheckPoint2Time switch
        {
            -1f => "Failed",
            0 => "No contact",
            _ => GameStat.CheckPoint2Time.ToString("F1") + "s"
        };
        string cp3Text = GameStat.CheckPoint3Time switch
        {
            -1f => "Failed",
            0 => "No contact",
            _ => GameStat.CheckPoint3Time.ToString("F1") + "s"
        };
        StatMessage.text =
            $"Time in game: {GameStat.GameTime:F1} s\nCheckpoint1: {cp1Text}\n" +
            $"Checkpoint2: {cp2Text}\n" +
            $"Checkpoint3: {cp3Text}";
        MenuContent.SetActive(true);

    }
    public static void Hide()                 // GameMenu.Hide()
    {
        MenuContent.SetActive(false);
        Time.timeScale = timeScaleNum;
    }
    public static void Toggle()              // GameMenu.Toggle()
    {
        if (MenuContent.activeInHierarchy) GameMenu.Hide();
        else GameMenu.Show();
    }
    public void ChangeTimeScale(Single scale)
    {
        timeScaleNum = scale;
    }

    #endregion

}