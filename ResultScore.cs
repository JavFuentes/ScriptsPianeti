using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScore : MonoBehaviour
{
    public Text scoreText, maxScore;

    public int Score()
    {
        return GUIManager.resultScore;
    }
    
    void Start()
    {
        int MaxScore = PlayerPrefs.GetInt("maxScore", 0);
        scoreText.text = "Score : "+ Score();
        maxScore.text = "Max :" + MaxScore;
    }
}
