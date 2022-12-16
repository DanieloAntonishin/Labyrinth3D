using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameStat : MonoBehaviour
{
    private static TMPro.TextMeshProUGUI Clock;
    private static TMPro.TextMeshProUGUI Score;
    private static UnityEngine.UI.Image CheckPoint1Image;
    private static UnityEngine.UI.Image CheckPoint2Image;
    private static UnityEngine.UI.Image CheckPoint3Image;
    
    private static float _checkpoint1Fill;
    private static float _checkpoint2Fill;
    private static float _checkpoint3Fill;

    private static float _bestCheckpoint1Fill;
    private static float _bestCheckpoint2Fill;
    private static float _bestCheckpoint3Fill;

    private static float _gameTime;
    private static int _score;

    public static string recordFilename = "Assets/Files/record.txt";

    public static float GameTime
    {
        get => _gameTime;
        set
        {
            _gameTime = value;
            UpdateTime();
        }
    }
    public static int GameScore
    {
        get => _score;
        set
        {
            _score = value;
            UpdateScore();
        }
    }
    public static float Checkpoint1Fill
    {
        get => _checkpoint1Fill;
        set
        {
            _checkpoint1Fill = value;
            UpdateCheckpoint1Fill();
        }
    }
    public static float CheckPoint1Time { get; set; } = 0;  // время прохождения _checkpoint1Fill
    public static float CheckPoint2Time { get; set; } = 0;  // время прохождения _checkpoint2Fill
    public static float CheckPoint3Time { get; set; } = 0;  // время прохождения _checkpoint3Fill
    public static float BestCheckpoint1Fill { get; private set; }
    public static float BestCheckpoint2Fill { get; private set; }
    public static float BestCheckpoint3Fill { get; private set; }
    public static float Checkpoint2Fill
    {
        get => _checkpoint2Fill;
        set
        {
            _checkpoint2Fill = value;
            UpdateCheckpoint2Fill();
        }
    }
    private static void SaveRecordFile()
    {
        System.IO.File.WriteAllText(recordFilename,
            $"{CheckPoint1Time};{CheckPoint2Time};{CheckPoint3Time}"
        );
    }

    public static bool ReadRecordFile()
    {
        string[] data = System.IO.File.ReadAllText(recordFilename).Split(";");
        try
        {
            BestCheckpoint1Fill = _bestCheckpoint1Fill = Convert.ToSingle(data[0]);
            BestCheckpoint2Fill = _bestCheckpoint2Fill = Convert.ToSingle(data[1]);
            BestCheckpoint3Fill = _bestCheckpoint3Fill = Convert.ToSingle(data[2]);
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
        return false;
    }

    public static float Checkpoint3Fill
    {
        get => _checkpoint3Fill;
        set
        {
            _checkpoint3Fill = value;
            UpdateCheckpoint3Fill();
        }
    }
    void Start()
    {
        GameStat.Clock = GameObject.Find("Clock").GetComponent<TMPro.TextMeshProUGUI>();
        GameStat.Score = GameObject.Find("ScoreValue").GetComponent<TMPro.TextMeshProUGUI>();
        GameStat.CheckPoint1Image = GameObject.Find("CheckImage1").GetComponent<UnityEngine.UI.Image>();
        GameStat.CheckPoint2Image = GameObject.Find("CheckImage2").GetComponent<UnityEngine.UI.Image>();
        GameStat.CheckPoint3Image = GameObject.Find("CheckImage3").GetComponent<UnityEngine.UI.Image>();
    }

    void LateUpdate()
    {
        GameStat.GameTime += Time.deltaTime;
    }

    private static void UpdateTime()
    {
        int t = (int)_gameTime;
        GameStat.Clock.text = $"{t / 3600 % 24:00}:{t / 60 % 60:00}:{t % 60:00}.{(int)((_gameTime - t) * 10):0}";
    }
    private static void UpdateScore()
    {
        GameStat.Score.text = $"{GameStat.GameScore:0000}";
    }
    private static void UpdateCheckpoint1Fill()
    {
        if (Checkpoint1Fill >= 0 && Checkpoint1Fill <= 1)
        {
            GameStat.CheckPoint1Image.fillAmount = Checkpoint1Fill;
            // меняем цвет изображения в зависимости от заполнения 
            GameStat.CheckPoint1Image.color = new Color(
                0.8f - Checkpoint1Fill,   // красный: больше, если fill меньше
                Checkpoint1Fill,          // зеленый: наоборот красному
                0.1f                      // синий:   постоянный
                );
        }
    }
    private static void UpdateCheckpoint2Fill()
    {
        if (Checkpoint2Fill >= 0 && Checkpoint2Fill <= 1)
        {
            GameStat.CheckPoint2Image.fillAmount = Checkpoint2Fill;
            GameStat.CheckPoint2Image.color = new Color(
                0.8f - Checkpoint2Fill,   // красный: больше, если fill меньше
                Checkpoint2Fill,          // зеленый: наоборот красному
                0.1f                      // синий:   постоянный
                );
        }
    }
    private static void UpdateCheckpoint3Fill()
    {
        if (Checkpoint3Fill >= 0 && Checkpoint3Fill <= 1)
        {
            GameStat.CheckPoint3Image.fillAmount = Checkpoint3Fill;
            GameStat.CheckPoint3Image.color = new Color(
                0.8f - Checkpoint3Fill,   // красный: больше, если fill меньше
                Checkpoint3Fill,          // зеленый: наоборот красному
                0.1f                      // синий:   постоянный
                );
        }
    }
    public static void CheckImage1Checked(bool status)
    {
        GameStat.GameScore += status ? (int)(Checkpoint1Fill * 1000) : 0;
        Checkpoint1Fill = 1;
        GameStat.CheckPoint1Image.color = status ? new Color(0.28f, 0.66f, 0.16f) : new Color(0.55f, 0.11f, 0.11f);
        GameStat.CheckPoint1Time = status ? GameTime : -1f;
        if (CheckPoint1Time > _bestCheckpoint1Fill)
        {
            _bestCheckpoint1Fill = CheckPoint1Time;
            BestCheckpoint1Fill = _bestCheckpoint1Fill; 
        }
    }
    public static void CheckImage2Checked(bool status)
    {
        GameStat.GameScore += status ? (int)(Checkpoint2Fill * 1000) : 0;
        Checkpoint2Fill = 1;
        GameStat.CheckPoint2Image.color = status ? new Color(0.28f, 0.66f, 0.16f) : new Color(0.55f, 0.11f, 0.11f);
        GameStat.CheckPoint2Time = status ? GameTime : -1f;
        if (CheckPoint2Time > _bestCheckpoint2Fill)
        {
            _bestCheckpoint2Fill = CheckPoint2Time;
            BestCheckpoint2Fill = _bestCheckpoint2Fill;
        }
    }
    public static void CheckImage3Checked(bool status)
    {
        GameStat.GameScore += status ? (int)(Checkpoint3Fill * 1000) : 0;
        Checkpoint3Fill = 1;
        GameStat.CheckPoint3Image.color = status ? new Color(0.28f, 0.66f, 0.16f) : new Color(0.55f, 0.11f, 0.11f);
        GameStat.CheckPoint3Time = status ? GameTime : -1f;
        if (CheckPoint3Time > _bestCheckpoint3Fill)
        {
            _bestCheckpoint3Fill = CheckPoint3Time;
            BestCheckpoint3Fill = _bestCheckpoint3Fill;
        }
    }
    private void OnDestroy()
    {
        SaveRecordFile();
    }
}
