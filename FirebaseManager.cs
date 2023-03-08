using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Firebase.Firestore;
using Firebase;
using Firebase.Extensions;
using System.Linq;


public class FirebaseManager : MonoBehaviour
{   
  // Colección de usuarios
  private ArrayList Users = new ArrayList();
    
  // Variables traidas de la memoria persistente 
  private string nick;
  private string wallet;
  private int score;

  // Usuarios Top 10
  User[] tops = new User[10];

  // Textos de cada Top 10
  public Text top1, top2, top3, top4, top5, top6, top7, top8, top9, top10;

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
        user.Nick = $"{documentDictionary["Nick"]}";
        user.Wallet = $"{documentDictionary["Wallet"]}";        
        user.Score = int.Parse(documentDictionary["Score"].ToString());
        
        Debug.Log($"Usuario {document.Id} almacenado correctamente.");        
        Users.Add(user);
      }

      // Indica que se leyeron todos los datos y se guardaron en el ArrayList "Users".
      Debug.Log("Leídos todos los datos de la colección de users y almacenados en el ArrayList.");

      // Ordenar la lista de usuarios por puntaje (de mayor a menor) y convertirla a una lista
      List<User> sortedUsers = Users.Cast<User>().OrderByDescending(u => u.Score).ToList();

      // Almacenar los 10 usuarios con los mejores puntajes en el array tops
      for (int i = 0; i < 10 && i < sortedUsers.Count; i++)
      {
        tops[i] = sortedUsers[i];
      }

      top1.text = tops[0].Nick + "  " + tops[0].Score;
      top2.text = tops[1].Nick + "  " + tops[1].Score;
      top3.text = tops[2].Nick + "  " + tops[2].Score;
      top4.text = tops[3].Nick + "  " + tops[3].Score;
      top5.text = tops[4].Nick + "  " + tops[4].Score;
      top6.text = tops[5].Nick + "  " + tops[5].Score;
      top7.text = tops[6].Nick + "  " + tops[6].Score;
      top8.text = tops[7].Nick + "  " + tops[7].Score;
      top9.text = tops[8].Nick + "  " + tops[8].Score;
      top10.text = tops[9].Nick + "  " + tops[9].Score;
      
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

