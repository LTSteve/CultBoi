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

        buyMenu.Toggle((demonNo) => {

            DemonManagerHack spawn;

            spawn = demonPrefabs.FirstOrDefault(x => x.demonType == demonNo);

            if (spawn == null) { return false; }

            if (!_sacrifice(demonNo, spawn)) { return false; }

            return true;
        });
    }

    private bool _sacrifice(int demon, DemonManagerHack toSpawn)
    {
        if(demon == 0)
        {

            if (CultistManagerHack.cultists.Count < 1)
            {
                return false;
            }

            var cultistsToSacrifice = CultistManagerHack.cultists.GetRange(0, 1);
            CultistManagerHack.cultists.RemoveRange(0, 1);

            StartCoroutine(_staggeredSacrifice(cultistsToSacrifice,toSpawn));

            return true;
        }

        if (!DemonManagerHack.demons.ContainsKey(demon-1) || DemonManagerHack.demons[demon-1].Count < 2)
        {
            return false;
        }

        var toSacrifice = DemonManagerHack.demons[demon-1].GetRange(0, 2);
        DemonManagerHack.demons[demon-1].RemoveRange(0, 2);

        StartCoroutine(_staggeredSacrificeDemons(toSacrifice, toSpawn, 1));

        return true;
    }

    IEnumerator _staggeredSacrifice(List<CultistManagerHack> cultists, DemonManagerHack toSpawn)
    {
        yield return null;

        while(cultists.Count > 0)
        {
            var cultist = cultists[0];
            var location = cultist.transform.position;
            cultist.GetComponent<IHealthHandler>()?.Damage(10000);
            cultists.RemoveAt(0);

            var demon = Instantiate(
                toSpawn,
                location, Quaternion.identity);

            var intentManager = demon.GetComponent<DemonIntentManager>();
            if (intentManager != null)
            {
                intentManager.formation = GetComponent<IFormationHandler>();
                intentManager.parent = transform;
            }

            yield return new WaitForSeconds(UnityEngine.Random.value * 0.1f);
        }
    }

    IEnumerator _staggeredSacrificeDemons(List<DemonManagerHack> demons, DemonManagerHack toSpawn, int numberToSpawn)
    {
        var spawned = 0;

        yield return new WaitForSeconds(0.5f);
        while (demons.Count > 0)
        {
            var toKill = demons[0];
            var location = toKill.transform.position;

            toKill.GetComponent<IHealthHandler>()?.Damage(10000);
            demons.RemoveAt(0);

            if(spawned < numberToSpawn)
            {
                spawned++;

                var demon = Instantiate(
                    toSpawn,
                    location, Quaternion.identity);

                var intentManager = demon.GetComponent<DemonIntentManager>();
                if (intentManager != null)
                {
                    intentManager.formation = GetComponent<IFormationHandler>();
                    intentManager.parent = transform;
                }
            }

            yield return new WaitForSeconds(UnityEngine.Random.value * 0.1f);
        }
    }
}