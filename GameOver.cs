using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{    
    // La función PlayAgain carga la escena "Scene 1" cuando se activa el botón "Play Again".    
    public void PlayAgain()
    {
        SceneManager.LoadScene("Scene 1");
    }
}
