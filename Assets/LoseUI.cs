using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseUI : MonoBehaviour
{
    public static LoseUI Instance { get; private set; }
    [SerializeField] private Button btnNext;

    private void Awake()
    {
        Instance = this;
        Hide();
        btnNext.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.Summary);
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
