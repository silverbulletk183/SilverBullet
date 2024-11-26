using UnityEngine;

public interface IHitable
{
    void Hit(GameObject hitObject, Vector3 hitPoint, Vector3 hitNormal);
}
