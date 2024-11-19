using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HomeUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button btnOptional;
    [SerializeField] private Button btnStart;
    private void Start()
    {
        btnOptional.onClick.AddListener(() =>
        {
            OptionalUI.Instance.Show();
        });
        btnStart.onClick.AddListener(SilverBulletGameLobby.Instance.QuickJoin);
    }

}
