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

    private GameObject closedDoor;
    private GameObject openDoor;
    private bool doorClosed = true;

    private void Start()
    {
        if (PauseMenu.IsOpen) return;
        _doDirtyRandomGeneration();

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

    private void _doDirtyRandomGeneration()
    {
        var root = transform.Find("HouseScale");

        var doors = root.Find("Doors");
        var windows = root.Find("Windows");
        var houses = root.Find("Houses");
        var colliders = root.Find("Colliders");

        var doorColor = UnityEngine.Random.Range(0, 3);
        var doorColors = new string[] { "Brown", "Red", "White" };
        var house = UnityEngine.Random.Range(0, 5) + 1;

        var closedDoor = doors.Find(doorColors[doorColor] + "/closed");
        var openDoor = doors.Find(doorColors[doorColor] + "/open");

        closedDoor.gameObject.SetActive(true);

        windows.Find("windows" + house).gameObject.SetActive(true);

        houses.Find("house" + house).gameObject.SetActive(true);

        colliders.Find("col" + house).gameObject.SetActive(true);
    }

    private void OnFinished(Transform obj)
    {
        if (dead) return;
        dead = true;

        StartCoroutine(_doorStuff());

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
            atTheDoor.TakingHouse = true;
            healthHandler.Damage(atTheDoor.DoorDamage * Time.deltaTime);
        }
    }

    private IEnumerator _doorStuff()
    {
        if (closedDoor != null && openDoor != null)
        {

            closedDoor.SetActive(false);
            openDoor.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            closedDoor.SetActive(true);
            openDoor.SetActive(false);

        }
        yield return null;
    }
}
