using UnityEngine;
using System;
using UnityEngine.UI;

public class BuyMenu : MonoBehaviour
{
    public static BuyMenu Instance;

    private bool justOpened = false;
    private bool justClosed = false;

    private Func<int,bool> Callback;

    private float doAShake = 0f;

    public AudioSource audio;

    public Image Hand;
    public Color OutlineShakeColor;

    private Color outlineBaseColor;
    private Vector3 baseLocation;

    private Vector3 handRoot;

    private int active = -1;

    private bool isOpen = false;

    private void Awake()
    {
        Instance = this;
        handRoot = Hand.transform.localPosition;
        outlineBaseColor = Hand.color;
        baseLocation = Hand.transform.localPosition;
        gameObject.SetActive(false);
    }

    public void Open(Func<int, bool> callback = null)
    {
        gameObject.SetActive(true);
        justOpened = true;
        Callback = callback;
        audio.Play();
    }

    public void Close()
    {
        Hand.gameObject.SetActive(false);
        gameObject.SetActive(false);
        justClosed = true;
        Callback = null;
    }

    public void Toggle(Func<int, bool> callback = null)
    {
        isOpen = !isOpen;
        if (isOpen) Open(callback);
        else Close();
    }

    public void Buy(int choice)
    {
        if (Callback == null) return;

        audio.Play();

        var success = Callback.Invoke(choice);

        if (!success)
        {
            //do a shake
            doAShake = 0.15f;
        }
    }

    public void Hover(int choice)
    {
        active = choice;
        audio.Play();
        Hand.gameObject.SetActive(true);
        Hand.transform.localPosition = handRoot + Vector3.right * (choice * 60 - 130);
        baseLocation = Hand.transform.localPosition;
    }

    public void UnHover(int choice)
    {
        if(choice == active)
            Hand.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (justOpened || justClosed) {
            justOpened = justClosed = false;
            return;
        }

        doAShake -= Time.deltaTime;
        if (doAShake > 0)
        {
            Hand.color = OutlineShakeColor;
            Hand.transform.localPosition = baseLocation + UnityEngine.Random.insideUnitSphere;
        }
        else
        {
            Hand.color = outlineBaseColor;
            Hand.transform.localPosition = baseLocation;
        }
    }
}