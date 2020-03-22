using UnityEngine;
using System;

public class DeathScreen : MonoBehaviour
{
    public static DeathScreen Instance;

    private Transform Player;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void Open(Transform player )
    {
        Player = player;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
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
}