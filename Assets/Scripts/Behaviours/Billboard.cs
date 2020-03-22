﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private new Camera camera;

    private Quaternion originalRotation;

    protected virtual void Start()
    {
        camera = Camera.main;
        originalRotation = Quaternion.Euler(Vector3.forward);// transform.rotation;
    }

    protected virtual void Update()
    {
        var rotation = camera.transform.rotation.eulerAngles;

        transform.rotation = Quaternion.Euler(0, rotation.y, 0) * originalRotation;
    }
}
