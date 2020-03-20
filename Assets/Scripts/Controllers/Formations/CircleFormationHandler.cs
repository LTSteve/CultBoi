using UnityEngine;
using System.Collections.Generic;

public class CircleFormationHandler : MonoBehaviour, IFormationHandler
{
    public float CircleSpeed = 1f;

    public int DemonsPerCircle = 8;

    public float CircleSpacing = 2f;
    public float CircleCenterOffset = 1f;

    private List<IIntentManager> controlled = new List<IIntentManager>();
    private float radians = 0;

    private bool moving = false;

    void Start()
    {
        var myMover = GetComponent<IMover>();
        
        if(myMover != null)
        {
            myMover.Moving += OnMove;
        }
    }

    public Vector3 GetMyPosition(IIntentManager intent)
    {
        var formationIndex = 0;
        if (!controlled.Contains(intent))
        {
            controlled.Add(intent);
            formationIndex = controlled.Count - 1;
        }
        else
        {
            formationIndex = controlled.IndexOf(intent);
        }

        var tier = formationIndex / DemonsPerCircle;
        var position = formationIndex % DemonsPerCircle;
        var myRadians = radians + ((float)position / (float)DemonsPerCircle) * 2 * Mathf.PI + ((tier / 3f) * Mathf.PI);

        return Quaternion.Euler(0, Mathf.Rad2Deg * myRadians, 0) * Vector3.forward * CircleSpacing * ((CircleCenterOffset / CircleSpacing) + tier) + transform.position;
    }

    void Update()
    {
        if(moving)
            radians = (radians + CircleSpeed * Time.deltaTime) % (2 * Mathf.PI);
    }

    private void OnMove(bool m, Vector3 direction)
    {
        moving = m;
    }
}