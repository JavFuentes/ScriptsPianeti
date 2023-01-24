using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GUIManager : MonoBehaviour
{
    public Text movesText, scoreText, maxScoreText;
    private int moveCounter;
    private int score;
    public int previousMaxScore;
    public static int resultScore = 0;

    public int Score
    {
        get
        {
            return score;
        }

        set
        {
            score = value;
            scoreText.text = "Score : " + score;
        }
    }
    
    public int MoveCounter
    {
        get
        {
            return moveCounter;
        }

        set
        {
            moveCounter = value;
            movesText.text = "Moves : " + moveCounter;

            if (moveCounter <= 0)
            {
                moveCounter = 0;
                StartCoroutine(GameOver());
            }
        }
    }

    public static GUIManager sharedInstance;

    void Start()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        score = 0;
        moveCounter = 10;
        movesText.text = "Moves : " + moveCounter;
        scoreText.text = "Score : " + score;

        previousMaxScore = PlayerPrefs.GetInt("maxScore", 0);
        maxScoreText.text = "Max :" + previousMaxScore;
    }
   
    private IEnumerator GameOver()
    {
        resultScore = score;
        previousMaxScore = PlayerPrefs.GetInt("maxScore", 0);
        if (resultScore > previousMaxScore)
        {
            PlayerPrefs.SetInt("maxScore", resultScore);
        }

        yield return new WaitUntil(() => !BoardManager.sharedInstance.isShifting);
        yield return new WaitForSeconds(1f);
        
        SceneManager.LoadScene("GameOverScene");
    }   
}
