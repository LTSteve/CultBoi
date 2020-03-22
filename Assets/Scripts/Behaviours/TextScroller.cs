using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextScroller : MonoBehaviour
{
    public static bool Open = true;

    public float Rate = 10f;

    public float Tick = 0.2f;

    public AudioSource Voiceover;

    public AudioClip[] Clips;

    public string[] Lines;

    private string currentLine;
    private int currentLetter;

    private float tick = 0f;

    private Text ui;

    private void Start()
    {
        ui = GetComponent<Text>();
        currentLine = Lines[0];
        var asdf = new List<string>(Lines);
        asdf.RemoveAt(0);
        Lines = asdf.ToArray();

        ui.text = string.Empty;
    }

    private void Update()
    {
        tick -= Time.deltaTime;

        if(tick > 0f)
        {
            return;
        }

        if(currentLetter >= currentLine.Length)
        {
            if(Input.GetMouseButtonDown(0) || Input.GetButtonDown("knock on door"))
            {
                if(Lines.Length > 0)
                {
                    currentLine = Lines[0];
                    var asdf = new List<string>(Lines);
                    asdf.RemoveAt(0);
                    Lines = asdf.ToArray();
                    currentLetter = 0;

                    tick = Tick;
                }
                else
                {
                    Open = false;
                    transform.parent.gameObject.SetActive(false);
                }
            }
            return;
        }

        currentLetter = (int)Mathf.Clamp(currentLetter + Rate, 1, currentLine.Length);

        ui.text = currentLine.Substring(0, currentLetter);

        tick = Tick;
    }
}
