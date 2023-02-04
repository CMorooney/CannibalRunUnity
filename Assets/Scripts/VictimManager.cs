using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static UnityEngine.AI.NavMesh;

public class VictimManager : MonoBehaviour
{
    public GameObject VictimPrefab;

    private const int _startingCount = 10;

    private readonly List<VictimScript> _activeVictims = new List<VictimScript>();

    private void Start()
    {
        SpawnInitial();
    }

    private void SpawnInitial()
    {
        int successCount = 0;
        int failCount = 0;
        while (successCount < _startingCount && failCount < 100)
        {
            var randomOrigin = new Vector3() + Random.insideUnitSphere * 20;

            //TODO: AllAreas seems wrong but I get no hits otherwise (should be bitmask for "Walkable"?)
            if (SamplePosition(randomOrigin, out var hit, 1, AllAreas))
            {
                var obj = Instantiate(VictimPrefab, hit.position.WithZ(0), Quaternion.identity);
                successCount++;
                _activeVictims.Add(obj.GetComponent<VictimScript>());
            }
            else
            {
                failCount++;
            }
        }

        if(successCount < _startingCount - 1)
        {
            throw new Exception("something went wrong spawning initial enemies");
        }

        AssignInitialStates();
    }

    private void AssignInitialStates()
    {
        foreach(var victim in _activeVictims)
        {
            Action action = Random.Range(0, 2) switch
            {
                0 => victim.Wander,
                1 => victim.Wait,
                _ => victim.Wait// shouldn't happen, just covering a confused compiler warning
            };
            action();
        }
    }
}
