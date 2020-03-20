using UnityEngine;

public class SpawnDemonActionHandler : MonoBehaviour, IActionHandler
{
    public int ActionNumber = 1;

    private Controller demonPrefab;

    void Start()
    {
        demonPrefab = Resources.Load<Controller>("Prefab/Entities/Demon");
    }

    public void HandleAction(IIntentManager intent)
    {
        if (ActionNumber < 1 || ActionNumber > 3) return;
        if (ActionNumber == 1 && !intent.action1) return;
        if (ActionNumber == 2 && !intent.action2) return;
        if (ActionNumber == 3 && !intent.action3) return;

        //TODO: nicer spawning

        var demon = Instantiate(demonPrefab, transform.position, Quaternion.identity);

        var intentManager = demon.GetComponent<DemonIntentManager>();
        if(intentManager != null)
        {
            intentManager.formation = GetComponent<IFormationHandler>();
            intentManager.parent = transform;
        }
    }
}