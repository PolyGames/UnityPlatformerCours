using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform subject;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime = 0.5f;
    private Vector3 dampSpeed;

    void Start()
    {
        transform.position = subject.position + offset;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, subject.position + offset, ref dampSpeed, smoothTime);
    }
}
