using UnityEngine;
using System;

public class DeathScreen : MonoBehaviour
{
    public static DeathScreen Instance;

    private Transform Player;

    public AudioClip[] Clips;
    public AudioSource source;

    private bool latch = false;

    private int activeOption = -1;

    private float asdf = 0.2f;
    private float countdown = 0f;

    private Vector3[] pointerPositions = new Vector3[] {
        new Vector3(113, 43),
        new Vector3(94, -4)
    };

    public Transform pointer;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void Open(Transform player )
    {
        if (latch) return;

        BuyMenu.Instance.Close();
        Player = player;
        gameObject.SetActive(true);

        source.PlayOneShot(Clips[UnityEngine.Random.Range(0, Clips.Length)]);
        latch = true;
    }

    public void Close()
    {
        gameObject.SetActive(false);
        latch = false;
    }

    public void Continue()
    {
        Player.position = WorldGenerator.StartPoint;//TODO: regenerate world
        Player.GetComponent<IAnimationHandler>()?.Reset();
        Player.GetComponent<IHealthHandler>()?.Reset();
        HealthBar.Instance.Reset();
        CopAlert.Reset();
        Close();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Hover(int option)
    {
        activeOption = option;
        _movePointer();
    }

    public void UnHover(int option)
    {
        if (option != activeOption) return;
        pointer.gameObject.SetActive(false);
    }


    private void Update()
    {
        if (!KeyboardMouseIntentManager.mouseMode)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0)
            {
                var leftRight = Input.GetAxis("verticalright");

                leftRight = Mathf.Abs(leftRight) < 0.5f ? 0 : leftRight;

                if (leftRight != 0)
                {
                    activeOption = (activeOption  + 1) % 2;
                    _movePointer();
                    countdown = asdf;
                }
            }

            var activate = Input.GetButtonDown("knock on door");

            if (activate)
            {
                if (activeOption == 0) Continue();
                else Quit();
            }
        }
    }

    private void _movePointer()
    {
        pointer.gameObject.SetActive(true);
        pointer.localPosition = pointerPositions[activeOption];
    }
}