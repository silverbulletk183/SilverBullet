using UnityEngine;

public class FollowTransformPosLateUpdate : MonoBehaviour
{
    [SerializeField] private Transform target;

    public Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
