using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string username, password, email;

    public User(string username, string password, string email)
    {
        this.username = username;
        this.password = password;
        this.email = email;
    }
    public User()
    {

    }
}
