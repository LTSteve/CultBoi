using UnityEngine;

public interface IFormationHandler
{
    Vector3 GetMyPosition(IIntentManager intent);
    void Remove(Transform transform);
}
