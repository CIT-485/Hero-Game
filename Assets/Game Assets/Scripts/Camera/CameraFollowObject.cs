using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    public Transform objectToFollow;
    public Vector2 positionOffset;
    [Range(0.0f, 10.0f)]
    public float cameraSpeed = 5;

    public List<ShakeEvent> shakeEvents = new List<ShakeEvent>();
    public List<ShakeEvent> removeFromShakeEvents = new List<ShakeEvent>();
    
    private Vector3 totalShakeAmount;

    public class ShakeEvent
    {
        public float shakeAmount;
        public float shakeTimeElapsed;
        public float shakeTime;
        public bool shakeEndByTime;
        public ValueWrapper<bool> shakeEnd = new ValueWrapper<bool>(false);

        public ShakeEvent(float shakeAmount, float shakeTime, bool shakeEndByTime)
        {
            this.shakeAmount = shakeAmount;
            this.shakeTimeElapsed = 0;
            this.shakeTime = shakeTime;
            this.shakeEndByTime = shakeEndByTime;
        }
        public ShakeEvent(float shakeAmount, ValueWrapper<bool> shakeEnd)
        {
            this.shakeAmount = shakeAmount;
            this.shakeEnd = shakeEnd;
        }
    }

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

        if (shakeEvents.Count > 0)
        {
            totalShakeAmount = Vector3.zero;
            for (int i = 0; i < shakeEvents.Count; i++)
            {
                shakeEvents[i].shakeTimeElapsed += Time.deltaTime;
                AddShake(shakeEvents[i].shakeAmount);
                if (shakeEvents[i].shakeEndByTime && shakeEvents[0].shakeTimeElapsed > shakeEvents[0].shakeTime)
                    removeFromShakeEvents.Add(shakeEvents[i]);
                else if (!shakeEvents[i].shakeEndByTime && !shakeEvents[i].shakeEnd.Value)
                    removeFromShakeEvents.Add(shakeEvents[i]);
            }
            foreach (ShakeEvent evt in removeFromShakeEvents)
                shakeEvents.Remove(evt);
            removeFromShakeEvents.Clear();
            transform.position = totalShakeAmount;
        }
    }

    public void Shake(float shakeAmount, float shakeTime)
    {
        shakeEvents.Add(new ShakeEvent(shakeAmount, shakeTime, true));

    }
    public void Shake(float shakeAmount, ValueWrapper<bool> shakeEnd)
    {
        shakeEvents.Add(new ShakeEvent(shakeAmount, shakeEnd));
    }

    void AddShake(float shakeAmount)
    {
        totalShakeAmount += objectToFollow.position + (Vector3)positionOffset + (Vector3)Random.insideUnitCircle * shakeAmount + Vector3.back * 10;
    }
}