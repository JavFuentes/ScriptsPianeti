using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Ranking : MonoBehaviour
{   
    //Textos del Top 10
    public Text top1, top2, top3, top4, top5, top6, top7, top8, top9, top10;

    //Texto del puntaje máximo obtenido, traído desde la memoria persistente.
    public Text  myMaxScore;
   

    void Start()
    {        
        myMaxScore.text = "My Max Score : " + PlayerPrefs.GetInt("maxScore", 0); 
    }
    
    public void BackMenu()
    {
        SceneManager.LoadScene("Scene 1");
    }  
}
