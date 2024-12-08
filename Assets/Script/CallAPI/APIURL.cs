using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIURL : MonoBehaviour
{
    public static string APIBaseUrl= "https://silverbulletapi.onrender.com/api/";
    //public static string APIBaseUrl= "http://localhost:3000/api/";
    public static string UserLogin = APIBaseUrl+"user/login";
    public static string UserRegiter = APIBaseUrl + "user";
    public static string UserUpdate = APIBaseUrl + "user";
    public static string UserImage = APIBaseUrl + "userimage?id=";
    public static string Character = APIBaseUrl + "character";
    public static string Gun = APIBaseUrl + "gun";
    public static string CharacterImage = APIBaseUrl + "characterimage?id=";
    public static string GunImage = APIBaseUrl + "gunimage?id=";
    public static string CreateUserGun = APIBaseUrl + "usergun";
    public static string GetUserGun = APIBaseUrl + "usergun?id_user=";
    public static string CreateUserCharacter = APIBaseUrl + "usercharacter";
    public static string GetUserCharacter = APIBaseUrl + "usercharacter?id_user=";
    public static string GetUserSelect = APIBaseUrl + "userselection?id_user=";
    public static string UpdateUserSelect = APIBaseUrl + "userselection";
}
