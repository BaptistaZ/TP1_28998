using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GerirPontuacao : MonoBehaviour
{
    public static GerirPontuacao instance;
    private Text scoreText;
    public int score = 0;

    void Awake()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        MakeSingleton();
    }

    void Start()
    {
        AddScore(0);
    }

    private void Update()
    {
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
            scoreText.text = score.ToString();
        }
    }

    void MakeSingleton()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        if  (score > PlayerPrefs.GetInt("HighScore", 0))
            PlayerPrefs.SetInt("HighScore", score);

        scoreText.text = score.ToString();
    }

    public void ResetScore()
    {
        score = 0;
    }
}
