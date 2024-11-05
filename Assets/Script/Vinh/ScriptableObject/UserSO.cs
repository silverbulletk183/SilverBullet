using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "user", menuName = "data/user")]
public class UserSO : ScriptableObject
{
    public string username;
    public string sdt;
    public Sprite logo;
    public string fullname;
    public string email;
    public int level;
    public int experiencePoints;
    public List<string> achievements;
}
