using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public void OnHitByLaser()
    {
        // Xử lý khi bị bắn bởi tia laser
        Debug.Log(gameObject.name + " bị bắn bởi tia laser!");

        // Thực hiện các hành động khác (giảm máu, hiệu ứng, v.v.)
    }
}
