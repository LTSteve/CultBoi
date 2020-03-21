using System;
using UnityEngine;
public abstract class InterfaceMessage : MonoBehaviour
{
    public Action Feedback { get; set; }
}