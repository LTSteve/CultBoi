using System;
using UnityEngine;
public interface IMessageDisplayer
{
    Action Feedback { get; set; }
    void Display();
    void Hide();
}