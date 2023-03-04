using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScore : MonoBehaviour
{   
    //Referencia a los textos que mostrarán el puntaje actual y el puntaje máximo guardado
    public Text scoreText, maxScore;

    //Método para obtener el puntaje actual guardado en la clase GUIManager
    public int Score()
    {
        return GUIManager.resultScore;
    }
    
    void Start()
    {   
        //Obtiene el puntaje máximo guardado en PlayerPrefs con una clave "maxScore", si no existe devuelve 0
        int MaxScore = PlayerPrefs.GetInt("maxScore", 0);

        //Muestra el puntaje actual en el texto scoreText
        scoreText.text = "Score : " + Score();

        //Muestra el puntaje máximo en el texto maxScore
        maxScore.text = "Max :" + MaxScore;
    }
}
