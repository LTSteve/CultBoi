﻿using UnityEngine;
using System;
using UnityEngine.UI;

public class PressButtonInterfaceMessage : InterfaceMessage
{
    [HideInInspector]
    public KeyCode key;

    public static PressButtonInterfaceMessage Instance;

    private void Start()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(key) || Input.GetButton("knock on door"))
        {
            Feedback?.Invoke();
        }
    }

    public void SetText(KeyCode interactKey)
    {
        key = interactKey;

        var text = GetComponentInChildren<Text>();

        text.text = "F | (A)";//key.ToString();
    }
}