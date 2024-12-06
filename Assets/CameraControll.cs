using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target; 
    public float smoothSpeed = 0.2f;
    public Vector3 offset;

    private float _initialY; 

    void Start()
    {
        _initialY = transform.position.y;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = new Vector3(transform.position.x, Mathf.Max(_initialY, target.position.y) + offset.y, transform.position.z);


            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

         
            transform.position = smoothedPosition;
        }
    }
}