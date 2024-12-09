using Unity.Netcode;
using UnityEngine;

public class FleshDecal : MonoBehaviour, IHitable
{
    [SerializeField] private GameObject decalPrefab;

    private void Start()
    {
        
    }
    public void Hit(GameObject hitObject, Vector3 hitPoint, Vector3 hitNormal)
    {
        // Tạo hiệu ứng decal tại vị trí va chạm.
        GameObject decal = Instantiate(decalPrefab, hitPoint, Quaternion.LookRotation(hitNormal));
        decal.transform.SetParent(hitObject.transform);

        // Chỉ server mới được xử lý giảm máu.
        
    }

   
}
