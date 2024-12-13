using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    public static WinUI Instance {  get; private set; }
    [SerializeField] private Button btnNext;

    private void Awake()
    {
        Instance = this;
        Hide();
        btnNext.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.mainHomecp);
        });
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
