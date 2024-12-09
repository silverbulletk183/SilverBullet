using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EVENTSanhcho : MonoBehaviour
{

    public GameObject panelsettingUI;
    public GameObject panelTaikhoan;
    public GameObject panelAmthanh;

    public GameObject paneltuchien;
    public GameObject btntuchien;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void opensetting()
    {
        panelsettingUI.SetActive(true);
    }
    public void closesetting()
    {
        panelsettingUI.SetActive(false);

    }
    public void openpaneTaikhoan()
    {
        panelTaikhoan.SetActive(true);
        panelAmthanh.SetActive(false);
    }
    public void openpaneAmthanh()
    {
        panelTaikhoan.SetActive(false);
        panelAmthanh.SetActive(true);
    }
    public void Dangxuat()
    {
        SceneManager.LoadScene(0);
    }

    public void opentuchien()
    {
        paneltuchien.SetActive(true);
        btntuchien.SetActive(false);
    }


}
