using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnHienShop : MonoBehaviour
{
    public GameObject pnShop;
    public GameObject scrollVK;
    public GameObject scrollNV;

    public void HienShopKho()
    {
        pnShop.SetActive(true); 
    }
    public void AnShopKho()
    {
        pnShop.SetActive(false);
    }
    
    public void HienScrollVK()
    {
        scrollVK.SetActive(true);
        scrollNV.SetActive(false);
    }
    public void AnScrollVK()
    {
        scrollVK.SetActive(false);
        scrollNV.SetActive(true);
    }
}
