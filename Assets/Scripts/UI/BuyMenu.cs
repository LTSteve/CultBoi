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

    public Image Outline;
    public Color OutlineShakeColor;
    private Color outlineBaseColor;
    private Vector3 baseLocation;

    private void Awake()
    {
        Instance = this;
        outlineBaseColor = Outline.color;
        baseLocation = Outline.transform.position;
        gameObject.SetActive(false);
    }

    public void Open(Func<int, bool> callback = null)
    {
        gameObject.SetActive(true);
        justOpened = true;
        Callback = callback;
    }

    public void Close()
    {
        gameObject.SetActive(false);
        justClosed = true;
        Callback = null;
    }

    public void Buy(int choice)
    {
        if (Callback == null) return;
        
        var success = Callback.Invoke(choice);

        if (!success)
        {
            //do a shake
            doAShake = 0.15f;
        }
    }

    private void Update()
    {
        if (justOpened || justClosed) {
            justOpened = justClosed = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gameObject.activeSelf)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        doAShake -= Time.deltaTime;
        if (doAShake > 0)
        {
            Outline.color = OutlineShakeColor;
            Outline.transform.position = baseLocation + UnityEngine.Random.insideUnitSphere;
        }
        else
        {
            Outline.color = outlineBaseColor;
            Outline.transform.position = baseLocation;
        }
    }
}