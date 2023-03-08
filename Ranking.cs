using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Ranking : MonoBehaviour
{          
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
