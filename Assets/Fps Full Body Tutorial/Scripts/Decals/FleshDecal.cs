using UnityEngine;

public class FleshDecal : MonoBehaviour, IHitable
{

    [SerializeField] private GameObject decalPrefab;



    public ThanhMau thanhMau;
    public int hphientai;
    public int hptoida = 100;

    private void Start()
    {
        hphientai = hptoida;
        thanhMau.capnhatthanhmau(hphientai, hptoida);
    }

    public void Hit(GameObject hitObject, Vector3 hitPoint, Vector3 hitNormal)
    {
        GameObject decal = Instantiate(decalPrefab, hitPoint, Quaternion.LookRotation(hitNormal));
        decal.transform.SetParent(hitObject.transform);

        hphientai -= 90;
        thanhMau.capnhatthanhmau(hphientai, hptoida);
        // Xử lý khi bị bắn bởi tia laser
        Debug.Log(gameObject.name + " bị bắn bởi tia laser!");

        // Thực hiện các hành động khác (giảm máu, hiệu ứng, v.v.)

    }
}
