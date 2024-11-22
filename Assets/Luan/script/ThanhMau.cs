
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class ThanhMau : MonoBehaviour
{
    public Image hpimg;
    public void capnhatthanhmau(float hphientai, float hptoida)
    {
        hpimg.fillAmount = hphientai / hptoida;
        if (hphientai < 70)
        {
            hpimg.color = Color.blue;
        }
        if (hphientai < 30)
        {
            hpimg.color = Color.red;
        }
    }
}
