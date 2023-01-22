using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{    
    public void PlayAgain()
    {
        SceneManager.LoadScene("Scene 1");
    }
}
