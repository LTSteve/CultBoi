using System.Collections;
using UnityEngine;

public class PlayerRotator : MonoBehaviour, IRotator
{
    public float RotationDegrees = 45f;

    public float RotationTime = 0.5f;

    private Quaternion? rotateTo = null;
    private float completeness = 1f;

    public void Rotate(float intent)
    {
        if (intent == 0) return;

        var rotateFrom = rotateTo.HasValue ? rotateTo.Value : transform.rotation;
        rotateTo = rotateFrom * Quaternion.Euler(0, RotationDegrees * intent, 0);
        completeness = 0f;
    }

    void Update()
    {
        if (!rotateTo.HasValue) return;

        completeness += Time.deltaTime / RotationTime;

        transform.rotation = Quaternion.Lerp(transform.rotation, rotateTo.Value, completeness);

        if(completeness >= 1)
        {
            rotateTo = null;
        }
    }
}