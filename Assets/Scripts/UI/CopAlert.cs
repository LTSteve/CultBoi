using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopAlert : MonoBehaviour
{
    public static CopAlert Instance;

    public static float Level { get
        {
            return Instance._maxed ? 1 : Instance._level;
        }
        set
        {
            Instance._level = Instance._maxed ? 1 : value;
        }
    }

    public float Downward = 0.5f;

    public Image Bar;

    public Text Icon;

    public Color StartColor;
    public Color EndColor;

    private float _level;
    private bool _maxed;

    private float _lastLevel = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(_level >= 1)
        {
            _maxed = true;
            _level = 1;

            Icon.color = EndColor;
        }
        else if(_lastLevel >= _level)
        {
            _level -= Time.deltaTime * Downward;
            _level = _level < 0f ? 0f : _level;
        }

        _lastLevel = _level;

        Bar.transform.localScale = new Vector3(1, _level, 1);
        Bar.color = Color.Lerp(StartColor, EndColor, _level);
    }

    public static void Reset()
    {
        Instance._maxed = false;
        Instance._level = 0;
        Instance.Icon.color = Color.white;
    }
}
