using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnDemonActionHandler : MonoBehaviour, IActionHandler
{
    public int ActionNumber = 1;

    private DemonManagerHack[] demonPrefabs;

    private BuyMenu buyMenu;

    void Start()
    {
        buyMenu = BuyMenu.Instance;
        demonPrefabs = Resources.LoadAll<DemonManagerHack>("Prefab/Entities/Demons");
    }

    public void HandleAction(IIntentManager intent)
    {
        if (ActionNumber < 1 || ActionNumber > 3) return;
        if (ActionNumber == 1 && !intent.action1) return;
        if (ActionNumber == 2 && !intent.action2) return;
        if (ActionNumber == 3 && !intent.action3) return;

        if (buyMenu == null) return;

        buyMenu.Open((demonNo) => {

            DemonManagerHack spawn;

            var upgradeStatus = demonNo / 10;
            demonNo = demonNo % 10;

            if (upgradeStatus > 0)
            {
                if(!_sacrifice((demonNo + 1) * 2, demonNo, upgradeStatus - 1)) { return false; }

                spawn = demonPrefabs.FirstOrDefault(x => x.demonType == demonNo && x.demonUpgrade == upgradeStatus);
            }
            else
            {
                if (!_sacrifice(demonNo + 1, -1)) { return false; }

                spawn = demonPrefabs.FirstOrDefault(x => x.demonType == demonNo && x.demonUpgrade == 0);
            }

            if (spawn == null) { return false; }

            var demon = Instantiate(
                spawn,
                transform.position, Quaternion.identity);

            var intentManager = demon.GetComponent<DemonIntentManager>();
            if (intentManager != null)
            {
                intentManager.formation = GetComponent<IFormationHandler>();
                intentManager.parent = transform;
            }

            return true;
        });
    }

    private bool _sacrifice(int cost, int demonNo, int demonUpgrade = -1)
    {
        if(demonNo == -1)
        {

            if (CultistManagerHack.cultists.Count < cost)
            {
                return false;
            }

            var cultistsToSacrifice = CultistManagerHack.cultists.GetRange(0, cost);
            CultistManagerHack.cultists.RemoveRange(0, cost);

            StartCoroutine(_staggeredSacrifice(cultistsToSacrifice));

            return true;
        }



        if (!DemonManagerHack.demons.ContainsKey(demonNo) || DemonManagerHack.demons[demonNo].Count < cost)
        {
            return false;
        }

        var toSacrifice = DemonManagerHack.demons[demonNo].GetRange(0, cost);
        DemonManagerHack.demons[demonNo].RemoveRange(0, cost);

        StartCoroutine(_staggeredSacrificeDemons(toSacrifice));

        return true;
    }

    IEnumerator _staggeredSacrifice(List<CultistManagerHack> cultists)
    {
        yield return new WaitForSeconds(0.5f);

        while(cultists.Count > 0)
        {
            cultists[0].GetComponent<IHealthHandler>()?.Damage(10000);
            cultists.RemoveAt(0);

            yield return new WaitForSeconds(UnityEngine.Random.value * 0.1f);
        }
    }

    IEnumerator _staggeredSacrificeDemons(List<DemonManagerHack> demons)
    {
        yield return new WaitForSeconds(0.5f);
        while (demons.Count > 0)
        {
            demons[0].GetComponent<IHealthHandler>()?.Damage(10000);
            demons.RemoveAt(0);

            yield return new WaitForSeconds(UnityEngine.Random.value * 0.1f);
        }
    }
}