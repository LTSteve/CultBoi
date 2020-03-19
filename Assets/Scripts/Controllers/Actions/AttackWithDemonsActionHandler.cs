using UnityEngine;
using System.Linq;

public class AttackWithDemonsActionHandler : CommandDemonsActionHandler
{
    protected override Command BuildCommand(Vector3 commandLocation)
    {
        return new Command
        {
            Type = CommandType.Action1,
            Location = commandLocation,
            From = transform
        };
    }
}