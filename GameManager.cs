using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Enumeración que define los estados del juego
public enum GameState
{
    menu,
    inGame,
    gameOver
}

// Clase que maneja el flujo del juego
public class GameManager : MonoBehaviour
{   
    // Estado actual del juego
    public GameState currentGameState = GameState.menu;

    // Instancia única del GameManager
    public static GameManager sharedInstance;
        
    void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
    }
    
    void Update()
    {   
        // Si se presiona el botón "PlayButton" y el juego no está en curso, se inicia el juego
        if (Input.GetButtonDown("PlayButton") &&
         currentGameState != GameState.inGame)
        {
            StartGame();          
        }
    }

    // Función que inicia el juego
    public void StartGame()
    {
        SetGameState(GameState.inGame);
    }

    // Función que se ejecuta cuando se termina el juego
    public void GameOver()
    {
        SetGameState(GameState.gameOver);
    }

    // Función que lleva de vuelta al menú principal
    public void BackToMenu()
    {
        SetGameState(GameState.menu);
    }

    // Función que cambia el estado del juego
    private void SetGameState(GameState newGameSate)
    {   
        // Si el nuevo estado es "menu", se muestra el menú principal
        if (newGameSate == GameState.menu)
        {          
            MenuManager.sharedInstance.ShowMainMenu();
        }

        // Si el nuevo estado es "inGame", se prepara la escena para jugar
        else if (newGameSate == GameState.inGame)
        {
            //TODO: hay que preparar la escena para jugar           
            MenuManager.sharedInstance.HideMainMenu();
        }

        // Si el nuevo estado es "gameOver", se prepara el juego para el Game Over
        else if (newGameSate == GameState.gameOver)
        {
            //TODO: preparar el juego para el Game Over
            MenuManager.sharedInstance.ShowMainMenu();
        }
        
        // Se actualiza el estado actual del juego
        this.currentGameState = newGameSate;
    }
}
