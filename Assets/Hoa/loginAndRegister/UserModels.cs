using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserModels 
{
    public UserModels(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
    public string username { get; set; }
    public string password { get; set; }
 

}
