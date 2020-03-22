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

    private Vector3 startingPos;

    private float asdf = 0.2f;
    private float countdown = 0f;

    private void Awake()
    {
        Instance = this;
        handRoot = Hand.transform.localPosition;
        outlineBaseColor = Hand.color;
        baseLocation = Hand.transform.localPosition;
        startingPos = gameObject.transform.localPosition;
        gameObject.transform.localPosition = startingPos + Vector3.down * 80;
    }

    public void Open(Func<int, bool> callback = null)
    {
        justOpened = true;
        Callback = callback;
        audio.Play();
        gameObject.transform.localPosition = startingPos;
    }

    public void Close()
    {
        Hand.gameObject.SetActive(false);
        justClosed = true;
        Callback = null;
        gameObject.transform.localPosition = startingPos + Vector3.down * 80;
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
        _movePointer();
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

        if (!KeyboardMouseIntentManager.mouseMode)
        {
            countdown -= Time.deltaTime;
            if(countdown <= 0)
            {
                var leftRight = Input.GetAxis("horizontalright");

                leftRight = Mathf.Abs(leftRight) < 0.5f ? 0 : leftRight;

                if (leftRight < 0)
                {
                    active = (active + 2) % 3;
                    countdown = asdf;
                }
                else if (leftRight > 0)
                {
                    active = (active + 1) % 3;
                    countdown = asdf;
                }
                
                if(leftRight != 0)
                    _movePointer();
            }

            var activate = Input.GetButtonDown("knock on door");

            if (activate)
            {
                Buy(active);
            }
        }
    }

    private void _movePointer()
    {
        audio.Play();
        Hand.gameObject.SetActive(true);
        Hand.transform.localPosition = handRoot + Vector3.right * (active * 50 - 58);
        baseLocation = Hand.transform.localPosition;
    }
}