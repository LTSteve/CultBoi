﻿using UnityEngine;
using System;
using System.Collections.Generic;

public class CultistManagerHack : MonoBehaviour
{
    public static List<CultistManagerHack> cultists = new List<CultistManagerHack>();

    private void Start()
    {
        cultists.Add(this);
    }
}