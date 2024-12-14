using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserUI : MonoBehaviour
{
    // Start is called before the first frame update
    public static UserUI Instance { get; private set; }
    [SerializeField] private GameObject userItem;
    [SerializeField] private Transform content;
    private void Awake()
    {
        Instance = this;
    }
    public void PopulateShop(List<User> users) // ??i 'user' thành 'users'
    {
        foreach (var user in users)
        {
            GameObject item = Instantiate(userItem, content);
            UserItemUI ui = item.GetComponent<UserItemUI>();
            if (ui != null)
            {
                Debug.Log("set characterdata" + user);
                ui.SetupUserData(user);
            }
        }
    }

}
