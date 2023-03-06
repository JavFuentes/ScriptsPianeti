using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GUIManager : MonoBehaviour
{   
    // Texto para mostrar el puntaje, los movimientos y el puntaje máximo.
    public Text movesText, scoreText, maxScoreText;

    // Contadores para el número de movimientos y el puntaje.
    private int moveCounter;
    private int score;

    // El puntaje máximo anterior, que se carga desde la memoria persistente.
    public int previousMaxScore;

    // El puntaje actual, que se usa para enviar el puntaje a la escena de Game Over.
    public static int resultScore = 0;

    // Propiedad para obtener y establecer el puntaje.
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

    // Propiedad para obtener y establecer el contador de movimientos.    
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

            // Si el contador de movimientos llega a 0, inicia la rutina de Game Over.
            if (moveCounter <= 0)
            {
                moveCounter = 0;
                StartCoroutine(GameOver());
            }
        }
    }

    // Instancia compartida de GUIManager para acceder a sus variables y métodos.
    public static GUIManager sharedInstance;

    void Start()
    {   
        // Si no hay una instancia de GUIManager, esta instancia se convierte en la instancia compartida.
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        // Si ya hay una instancia de GUIManager, se destruye esta instancia.
        else
        {
            Destroy(gameObject);
        }

        // Inicializar el puntaje y el contador de movimientos en 0 y 5, respectivamente.
        score = 0;
        moveCounter = 5;
        movesText.text = "Moves : " + moveCounter;
        scoreText.text = "Score : " + score;

        // Cargar el puntaje máximo anterior desde la memoria persistente y mostrarlo en pantalla.
        previousMaxScore = PlayerPrefs.GetInt("maxScore", 0);
        maxScoreText.text = "Max :" + previousMaxScore;
    }
    
    // Rutina que se inicia cuando el contador de movimientos llega a 0.
    private IEnumerator GameOver()
    {   
        // Asignar el puntaje actual al puntaje resultante.
        resultScore = score;

        // Cargar el puntaje máximo anterior desde la memoria persistente.
        previousMaxScore = PlayerPrefs.GetInt("maxScore", 0);
        
        // Si el puntaje resultante es mayor que el puntaje máximo anterior, actualizar el puntaje máximo.
        if (resultScore > previousMaxScore)
        {
            PlayerPrefs.SetInt("maxScore", resultScore);

            if(PlayerPrefs.GetInt("maxScore", 0) >= 250){
                // Esperar hasta que el tablero termine de moverse.
                yield return new WaitUntil(() => !BoardManager.sharedInstance.isShifting);

                // Esperar un segundo y luego cargar la escena de Game Over.
                yield return new WaitForSeconds(1f);    

                SceneManager.LoadScene("Perfil");
            }
          
        }

        // Esperar hasta que el tablero termine de moverse.
        yield return new WaitUntil(() => !BoardManager.sharedInstance.isShifting);

        // Esperar un segundo y luego cargar la escena de Game Over.
        yield return new WaitForSeconds(1f);        
        SceneManager.LoadScene("GameOverScene");
    }   
}
