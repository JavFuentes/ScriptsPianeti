using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase;
using Firebase.Extensions;


public class FirebaseManager : MonoBehaviour
{   
  // Colección de usuarios
  private ArrayList Users = new ArrayList();
    
  // Variables traidas de la memoria persistente 
  private string nick;
  private string wallet;
  private int score;

  private FirebaseApp _app;  

  void Start()
  {  
    score = PlayerPrefs.GetInt("maxScore", 0);
    nick = PlayerPrefs.GetString("nickName", "");
    wallet = PlayerPrefs.GetString("solWallet", "");

    // Revisa y resuelve las dependencias necesarias para usar Firebase.
    Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
      var dependencyStatus = task.Result;
      if (dependencyStatus == Firebase.DependencyStatus.Available) 
      {
        // Crea una instancia de FirebaseApp y la guarda en una propiedad.
        _app = Firebase.FirebaseApp.DefaultInstance;
              
        // Indica que Firebase está listo para ser usado por la aplicación.
        Login();
      } 
    
      else 
      {
        // Si no se pueden resolver las dependencias de Firebase, muestra un mensaje de error.
        UnityEngine.Debug.LogError(System.String.Format(
        "No se pudieron resolver todas las dependencias de Firebase: {0}", dependencyStatus));            
      }
    });       
  } 

  private void AddData()
  {
    // Crea una instancia de FirebaseFirestore y FirebaseAuth.
    FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
    Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

    // Obtiene una referencia al documento del usuario actual.
    DocumentReference docRef = db.Collection("users").Document(auth.CurrentUser.UserId);

    // Crea un diccionario con los datos del usuario.
    Dictionary<string, object> user = new Dictionary<string, object>
    {
      { "Nick", nick },
      { "Wallet", wallet },
      { "Score", score }       
    };

    // Agrega los datos del usuario al documento
    docRef.SetAsync(user).ContinueWithOnMainThread(task => {
    Debug.Log("Se agregaron datos al documento en la colección de usuarios..");

    //Se obtienen los datos de Firestore y se guardan en un ArrayList
    GetData();
    });       
  }

  private void GetData()
  { 
    // Crea una instancia de FirebaseFirestore y obtiene una referencia a la colección "users".
    FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
    CollectionReference usersRef = db.Collection("users");

    // Obtiene un snapshot de los documentos en la colección "users".
    usersRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
    {
      // Itera sobre los documentos y guarda sus datos en un objeto "User".
      QuerySnapshot snapshot = task.Result;
      
      foreach (DocumentSnapshot document in snapshot.Documents)
      {        
        User user = new User();
        
        Dictionary<string, object> documentDictionary = document.ToDictionary();
        
        user.Id = document.Id;
        Debug.Log($"User: {document.Id}");

        user.Nick = $"Nick: {documentDictionary["Nick"]}";   
        Debug.Log($"Nick: {documentDictionary["Nick"]}");   

        user.Wallet = $"Wallet: {documentDictionary["Wallet"]}";
        Debug.Log($"Wallet: {documentDictionary["Wallet"]}"); 
        
        user.Score = int.Parse(documentDictionary["Score"].ToString());
        Debug.Log($"Score: {documentDictionary["Score"]}"); 
        
        
        Users.Add(user);
      }

      foreach(User usuario in Users)
      {
       Debug.Log(usuario.Nick);
      }

      // Indica que se leyeron todos los datos y se guardaron en el ArrayList "users".
      Debug.Log("Leídos todos los datos de la colección de users y almacenados en el ArrayList.");
    });    
  }

  private void Login()  
  {
    // Crea una instancia de FirebaseAuth.
    Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

    // Si ya hay un usuario autentificado, agrega los datos del usuario y sale de la función.
    if(auth.CurrentUser != null)
    {
        Debug.Log("Usuario ya está autentificado.");
        AddData();
        return;
    }

    // Inicia sesión de forma anónima y, cuando termine, agrega los datos del usuario.
    auth.SignInAnonymouslyAsync().ContinueWith(task => {
      if (task.IsCanceled) 
      {
        Debug.LogError("SignInAnonymouslyAsync fue canceledo.");
        return;
      }

      if (task.IsFaulted) 
      {
        Debug.LogError("SignInAnonymouslyAsync encontró un error: " + task.Exception);
        return;
      }

      Firebase.Auth.FirebaseUser newUser = task.Result;
      Debug.LogFormat("Usuario asignado correctamente: {0} ({1})",
        newUser.DisplayName, newUser.UserId);

      AddData();
    });
  }    
}

