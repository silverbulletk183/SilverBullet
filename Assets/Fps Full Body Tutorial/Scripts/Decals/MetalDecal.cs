using UnityEngine;

public class MetalDecal : MonoBehaviour, IHitable
{

    [SerializeField] private GameObject decalPrefab;

    public void Hit(GameObject hitObject, Vector3 hitPoint, Vector3 hitNormal)
    {
        GameObject decal = Instantiate(decalPrefab, hitPoint, Quaternion.LookRotation(hitNormal));
        decal.transform.SetParent(hitObject.transform);

    }
}
