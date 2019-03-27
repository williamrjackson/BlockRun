using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI currentScoreDisplay;
    [SerializeField]
    private TMPro.TextMeshProUGUI hiScoreDisplay;

    void Start()
    {
        GameManager.Instance.OnScoreUpdate += OnScoreUpdate;
        hiScoreDisplay.text = "HiScore: " + PlayerPrefs.GetInt("hiScore", 0).ToString();
    }

    private void OnScoreUpdate(int newScore)
    {
        currentScoreDisplay.text = "Score: " + newScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
