using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Perfil : MonoBehaviour
{   
    //Variables que almacenarán el texto ingresado en los input fields
    private string nickName;
    private string solWallet;

    //Referencia al texto que mostrará el nick y la wallet
    public Text nick, wallet;

    //Referencia al texto que mostrará el puntaje máximo guardado
    public Text maxScore;

    //Método para obtener el puntaje actual guardado en la clase GUIManager
    public int Score()
    {
        return GUIManager.resultScore;
    }
    
    void Start()
    {   
        //Obtiene el puntaje máximo guardado en PlayerPrefs con una clave "maxScore", si no existe devuelve 0
        int MaxScore = PlayerPrefs.GetInt("maxScore", 0);

        //Muestra el puntaje máximo en el texto maxScore
        maxScore.text = "My Score : " + MaxScore;        
    }

    void Update()
    {
        //Muestra el nick almacenado
        nick.text = PlayerPrefs.GetString("nickName", "");

        //Muestra la wallet almacenada
        wallet.text = PlayerPrefs.GetString("solWallet", "");
    }

    // Este método se llama cuando el usuario ingresa un nuevo nick en el input field para el perfil.
    // Almacena el nuevo nick en la variable nickName y lo guarda en la memoria persistente con PlayerPrefs.
    public void ReadStringInputForNick(string s)
    {
        nickName = s;
        PlayerPrefs.SetString("nickName", nickName);
    }

    // Este método se llama cuando el usuario ingresa una nueva wallet en el input field para el perfil.
    // Almacena la nueva wallet en la variable solWallet y la guarda en la memoria persistente con PlayerPrefs.
    public void ReadStringInputForWallet(string s)
    {
        solWallet = s;
        PlayerPrefs.SetString("solWallet", solWallet);
    }

    // La función BackMenu carga la escena "Scene 1" cuando se activa el botón "BackMenu".    
    public void BackMenu()
    {
        SceneManager.LoadScene("Scene 1");
    }  
}
