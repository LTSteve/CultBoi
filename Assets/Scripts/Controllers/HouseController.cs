using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    public int MinimumSpawn = 2;
    public int MaximumSpawn = 6;

    private ITriggerHandler triggerHandler;

    private IMessageDisplayer messageHandler;

    private IHealthHandler healthHandler;

    private List<Collider> entered = new List<Collider>();

    private Transform cultistPrefab;

    private bool dead = false;

    private void Start()
    {
        triggerHandler = GetComponentInChildren<ITriggerHandler>();
        messageHandler = GetComponent<IMessageDisplayer>();
        healthHandler = GetComponent<IHealthHandler>();

        cultistPrefab = Resources.Load<Transform>("Prefab/Entities/Cultist");

        if (triggerHandler != null)
        {
            triggerHandler.IsTriggered += OnEnter;
            triggerHandler.IsUnTriggered += UnEnter;
        }

        if(messageHandler != null)
        {
            messageHandler.Feedback += OnResponse;
        }

        if(healthHandler != null)
        {
            healthHandler.Died += OnFinished;
        }
    }

    private void OnFinished(Transform obj)
    {
        if (dead) return;
        dead = true;

        var player = WorldGenerator.Instance.player;

        var spawnCount = UnityEngine.Random.Range(MinimumSpawn, MaximumSpawn + 1);

        for(var i = 0; i < spawnCount; i++)
        {
            var cultist = Instantiate(cultistPrefab, transform.position, Quaternion.identity);

            var intentManager = cultist.GetComponent<DemonIntentManager>();
            if (intentManager != null)
            {
                intentManager.formation = player.GetComponent<IFormationHandler>();
                intentManager.parent = player.transform;
            }
        }
    }

    private void OnEnter(Collider collider)
    {
        if (!entered.Contains(collider))
            entered.Add(collider);
        messageHandler?.Display();
    }

    private void UnEnter(Collider collider)
    {
        if(entered.Contains(collider))
            entered.Remove(collider);
        messageHandler?.Hide();
    }

    private void OnResponse()
    {
        if (entered == null || entered.Count == 0) return;

        var atTheDoor = entered[0].gameObject.GetComponent<IHouseTaker>();

        if (atTheDoor != null)
        {
            healthHandler.Damage(atTheDoor.DoorDamage * Time.deltaTime);
        }
    }
}
