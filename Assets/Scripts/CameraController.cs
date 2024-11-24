using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float panSpeed = 5f;
    private float moveSpeed = 5f;
    private float scrollScale = 1f;
    private float sensitivity = 3f;
    public Transform target;
    public Boolean following;

    private float distance = 5f;
    public Vector3 cameraOffset;

    void Start()
    {
        cameraOffset = new Vector3(5, 0, 0);
    }

    public void setFollowing(bool follow)
    {
        following = follow;
    }

    void LateUpdate()
    {
        if (following)
        {
            float amount = 0 - Input.mouseScrollDelta.y * scrollScale;
            distance += amount;
            if (distance < 3) distance = 3; else if (distance > 20) distance = 20;

            Vector3 position = target.position + cameraOffset;

            transform.position = position;
            var old = transform.position;

            float movementX = 0, movementY = 0;
            if (Input.GetMouseButton(1))
            {
                movementX = Input.GetAxis("Mouse X") * panSpeed;
                movementY = -Input.GetAxis("Mouse Y") * panSpeed;
            }
            transform.RotateAround(target.position, Vector3.up, movementX);
            transform.RotateAround(target.position, Vector3.right, movementY);
            cameraOffset += (old - transform.position);
            cameraOffset = ClampMagnitude(transform.position - target.position, distance, distance);

            transform.LookAt(target);
            return;
        }

        Vector3 pos = new Vector3(0, 0, 0);
        Quaternion rot = transform.rotation;

        if (Input.GetMouseButton(1))
        {
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            rot = Quaternion.Euler(rot.eulerAngles.x - mouseY, rot.eulerAngles.y + mouseX, rot.eulerAngles.z);
        }

        if (Input.GetKey("w"))
        {
            pos += transform.forward * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey("s"))
        {
            pos += -transform.forward * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey("a"))
        {
            pos += -transform.right * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey("d"))
        {
            pos += transform.right * moveSpeed * Time.deltaTime;
        }

        moveSpeed -= 0 - Input.mouseScrollDelta.y * scrollScale;
        Debug.Log(moveSpeed);

        transform.position += pos;
        transform.rotation = rot;
    }

    public static Vector3 ClampMagnitude(Vector3 v, float max, float min)
    {
        double sm = v.sqrMagnitude;
        if (sm > (double)max * (double)max) return v.normalized * max;
        else if (sm < (double)min * (double)min) return v.normalized * min;
        return v;
    }

    public float getMoveSpeed()
    {
        return moveSpeed;
    }
}
