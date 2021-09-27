using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    public Transform objectToFollow;
    public Vector2 positionOffset;
    [Range(0.0f, 10.0f)]
    public float cameraSpeed = 5;

    void FixedUpdate()
    {
        Vector3 cameraFollowPosition = objectToFollow.position + new Vector3(positionOffset.x, positionOffset.y, transform.position.z);

        Vector3 cameraMoveDir = (cameraFollowPosition - transform.position).normalized;
        float distance = Vector3.Distance(cameraFollowPosition, transform.position);

        if (distance > 0)
        {
            Vector3 newCameraPosition = transform.position + cameraMoveDir * distance * cameraSpeed * Time.deltaTime;

            float distanceAfterMoving = Vector3.Distance(newCameraPosition, cameraFollowPosition);

            if (distanceAfterMoving > distance)
            {
                newCameraPosition = cameraFollowPosition;
            }

            transform.position = newCameraPosition;
        }
    }
}
