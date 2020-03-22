using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowerCount : MonoBehaviour
{
    public static FollowerCount Instance;

    public Text MyText;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        var total = CultistManagerHack.cultists.Count + DemonManagerHack.demons.Count;
        MyText.text = ""+Mathf.Clamp(total, 0, 999);
    }
}
