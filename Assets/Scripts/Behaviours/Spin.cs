using UnityEngine;

public class Spin : MonoBehaviour
{
    public bool SpinX;
    public bool SpinY;
    public bool SpinZ;

    //degrees
    public float Rate = 180f;

    void Update()
    {
        var rot = Rate * Time.deltaTime;

        var xRot = SpinX ? rot : 0;
        var yRot = SpinY ? rot : 0;
        var zRot = SpinZ ? rot : 0;

        transform.rotation *= Quaternion.Euler(xRot, yRot, zRot);
    }
}
