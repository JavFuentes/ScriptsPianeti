using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Ranking : MonoBehaviour
{   
    // Usuarios Top 10
    User[] tops = new User[10];

    // Textos de cada Top 10
    public Text top1, top2, top3, top4, top5, top6, top7, top8, top9, top10;
    
    // Texto del puntaje máximo obtenido, traído desde la memoria persistente.
    public Text myMaxScore;   

    void Start()
    {        
        myMaxScore.text = "My Max Score : " + PlayerPrefs.GetInt("maxScore", 0); 
     
    }
    
    // Método para volver al menú principal
    public void BackMenu()
    {
        SceneManager.LoadScene("Scene 1");
    }  
}
