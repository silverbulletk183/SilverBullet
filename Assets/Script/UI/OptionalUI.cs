using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionalUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button btnBack;
    [SerializeField] private Button btn5v5;

    public static OptionalUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        btnBack.onClick.AddListener(Hide);
        btn5v5.onClick.AddListener(() =>
        {
        });
    }
    private void Start()
    {
        
        Hide();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    
}
