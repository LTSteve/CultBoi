using System;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class ConvertHouseMessageDisplayer : MonoBehaviour, IMessageDisplayer
{
    public Action Feedback { get; set; }

    public KeyCode InteractKey = KeyCode.F;

    private PressButtonInterfaceMessage Message;

    void Start()
    {
        Message = PressButtonInterfaceMessage.Instance;

        if (Message != null)
        {
            Message.Feedback += OnFeedback;
        }
    }

    public void Display()
    {
        if (Message == null) return;
        Message?.gameObject.SetActive(true);
        Message.SetText(InteractKey);
    }

    public void Hide()
    {
        if (Message == null) return;
        Message?.gameObject.SetActive(false);
    }

    private void OnFeedback()
    {
        Feedback();
    }
}