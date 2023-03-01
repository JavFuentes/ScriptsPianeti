using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    menu,
    inGame,
    gameOver
}

public class GameManager : MonoBehaviour
{
    public GameState currentGameState = GameState.menu;

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
        if (Input.GetButtonDown("PlayButton") &&
         currentGameState != GameState.inGame)
        {
            StartGame();          
        }
    }

    public void StartGame()
    {
        SetGameState(GameState.inGame);
    }

    public void GameOver()
    {
        SetGameState(GameState.gameOver);
    }

    public void BackToMenu()
    {
        SetGameState(GameState.menu);
    }

    private void SetGameState(GameState newGameSate)
    {
        if (newGameSate == GameState.menu)
        {          
            MenuManager.sharedInstance.ShowMainMenu();
        }
        else if (newGameSate == GameState.inGame)
        {
            //TODO: hay que preparar la escena para jugar           
            MenuManager.sharedInstance.HideMainMenu();
        }
        else if (newGameSate == GameState.gameOver)
        {
            //TODO: preparar el juego para el Game Over
            MenuManager.sharedInstance.ShowMainMenu();
        }
        this.currentGameState = newGameSate;
    }
}
