using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public delegate void ScoreUpdateDelegate(int newScore);
    public ScoreUpdateDelegate OnScoreUpdate;
    public delegate void GameOverDelegate();
    public GameOverDelegate OnGameOver;
    private int m_Score = 0;
    private bool m_GameOver = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public bool gameOver
    {
        get
        {
            return m_GameOver;
        }
        set
        {
            m_GameOver = value;
            if (m_GameOver)
            {
                if (OnGameOver != null)
                    OnGameOver();

                PlayerPrefs.SetInt("hiScore", Mathf.Max(m_Score, PlayerPrefs.GetInt("hiScore", 0)));
            }
        }
    }
    public int score
    {
        get
        {
            return m_Score;
        }
    }

    public void AddToScore(int addition)
    {
        m_Score += addition;
        if (OnScoreUpdate != null)
            OnScoreUpdate(m_Score);
    }
}
