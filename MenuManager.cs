using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{   
    //Declaramos la variable sharedInstance como pública y estática para que pueda ser accedida desde cualquier script.
    public static MenuManager sharedInstance;

    //Declaramos la variable MenuCanvas para controlar el canvas del menú.
    public Canvas MenuCanvas;

    public void Awake()
    {           
         //Comprobamos si sharedInstance es nulo y lo inicializamos.
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    //El método ShowMainMenu activa el canvas del menú.
    public void ShowMainMenu()
    {
        MenuCanvas.enabled = true;
    }
    
    //El método HideMainMenu desactiva el canvas del menú.
    public void HideMainMenu()
    {
        MenuCanvas.enabled = false;
    }

    //El método ExitGame sale del juego.
    public void ExitGame()
    {  
         
    //Comprobamos si estamos en el editor de Unity o no para salir del juego.    
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }   
}
