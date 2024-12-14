using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryUI : MonoBehaviour
{
    public static SummaryUI instance {  get; private set; }

    [SerializeField] private Button btnBack;
    [SerializeField] private Button btnContiniu;

    private void Awake()
    {
        instance = this;
        btnBack.onClick.AddListener(() => 
        { 
            Loader.Load(Loader.Scene.mainHomecp);
        });
        btnContiniu.onClick.AddListener(() => {
            
        });
    }

}
