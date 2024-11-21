using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public ThanhMau thanhMau;
    public int hphientai;
    public int hptoida=100;

    private void Start()
    {
        hphientai = hptoida;
        thanhMau.capnhatthanhmau(hphientai, hptoida);
    }
    public void OnHitByLaser()
    {
        hphientai -= 1;
        thanhMau.capnhatthanhmau(hphientai, hptoida);
        // Xử lý khi bị bắn bởi tia laser
        Debug.Log(gameObject.name + " bị bắn bởi tia laser!");

        // Thực hiện các hành động khác (giảm máu, hiệu ứng, v.v.)
    }
}
