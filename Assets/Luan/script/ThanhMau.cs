using Microsoft.Unity.VisualStudio.Editor;
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
    }
}
