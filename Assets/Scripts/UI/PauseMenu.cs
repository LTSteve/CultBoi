using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;

    public static bool IsOpen = false;

    public AudioSource audio;
    public Transform pointer;
    public Transform menu;

    private bool justOpened = false;
    private bool justClosed = false;

    private int activeOption = -1;

    private float asdf = 0.2f;
    private float countdown = 0f;

    private Vector3[] pointerPositions = new Vector3[] { 
        new Vector3(113, 43),
        new Vector3(112,0),
        new Vector3(80, -28)
    };

    private void Awake()
    {
        Instance = this;
        menu.gameObject.SetActive(false);
    }

    public void Open()
    {
        BuyMenu.Instance.Close();
        justOpened = true;
        audio.Play();
        menu.gameObject.SetActive(true);
        IsOpen = true;
    }

    public void Close()
    {
        audio.Play();
        justClosed = true;
        menu.gameObject.SetActive(false);
        IsOpen = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Hover(int option)
    {
        audio.Play();
        activeOption = option;
        pointer.gameObject.SetActive(true);
        pointer.localPosition = pointerPositions[option];
    }

    public void UnHover(int option)
    {
        if (option != activeOption) return;
        pointer.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(justOpened || justClosed)
        {
            justClosed = justOpened = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("pause"))
        {
            if (IsOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        if (!KeyboardMouseIntentManager.mouseMode)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0)
            {
                var leftRight = Input.GetAxis("verticalright");

                leftRight = Mathf.Abs(leftRight) < 0.5f ? 0 : leftRight;

                if (leftRight != 0)
                {
                    activeOption = activeOption == 0 ? 2 : 0;
                    _movePointer();
                    countdown = asdf;
                }
            }

            var activate = Input.GetButtonDown("knock on door");

            if (activate)
            {
                if (activeOption == 0) Close();
                else Quit();
            }
        }
    }

    private void _movePointer()
    {
        audio.Play();
        pointer.gameObject.SetActive(true);
        pointer.localPosition = pointerPositions[activeOption];
    }
}
