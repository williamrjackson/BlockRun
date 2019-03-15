using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UnityAction GameOver;
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

    public bool gameOver;
    //public bool gameOver
    //{
    //    get
    //    {
    //        return gameOver;
    //    }
    //    set
    //    {
    //        //PlayerPrefs.SetInt("hiScore", Mathf.Max(score, PlayerPrefs.GetInt("hiScore", 0)));
    //        if (GameOver != null)
    //            GameOver();
    //    }
    //}
    public int score
    {
        get
        {
            return score;
        }
    }

    public void AddToScore(int addition)
    {
        
    }
}
