using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string Id;
    public string Nick;
    public string Wallet;
    public int Score;   

    public User()
    {
    
    }

    public User(string id, string nick, string wallet, int score)
    {
        Id = id;
        Nick = nick;
        Wallet = wallet;
        Score = score;
        
    }
}

