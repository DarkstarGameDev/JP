using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class DataPersistense : MonoBehaviour
{
    public static DataPersistense instance;
    public static string playerName;
    public static int highScore;

    private void Start() 
    {
        //Makes it a singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            try
            {
                File.ReadAllText(Application.dataPath + "/highscore.json");
                highScore = JsonUtility.FromJson<Data>(File.ReadAllText(Application.dataPath + "/highscore.json")).score;
            }
            catch
            {
                highScore = 0;
                playerName = "";
                File.Create(Application.dataPath + "/highscore.json");
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void begin(TMP_InputField field)
    {
        playerName = field.text;
        SceneManager.LoadScene(1);

    }

    public void quit()
    {
        Application.Quit();
    }

    public void setHighScore(int score)
    {
        if(score > highScore)
        {
            highScore = score;
            saveBestScore();
            changeBestScore();
        }
    }

    public void changeBestScore()
    {
        Text score = GameObject.Find("BS").GetComponent<Text>();
        string json = File.ReadAllText(Application.dataPath + "/highscore.json");
        Data data = JsonUtility.FromJson<Data>(json);

        score.text = "Best Score: " + data.name + " : " + data.score;
    }

    public void saveBestScore()
    {
        Data data = new Data(playerName, highScore);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.dataPath + "/highscore.json", json);
    }
}

[System.Serializable]
public class Data
{
    public string name;
    public int score;

    public Data(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}