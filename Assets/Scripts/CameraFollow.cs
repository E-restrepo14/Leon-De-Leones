using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    [SerializeField] Transform target;
    float smoothSpeed = 80f;

    private void LateUpdate() 
    {
        Vector3 newPosition = target.position + offset;
        SmoothTranslate(newPosition);
    }

    void SmoothTranslate(Vector3 desiredPosition) // ABSTRACTION
    {
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }
}
