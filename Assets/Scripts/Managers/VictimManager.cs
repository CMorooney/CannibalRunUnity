using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static UnityEngine.AI.NavMesh;
using UnityEngine.AI;

public class VictimManager : MonoBehaviour
{
    public GameObject VictimPrefab;
    public GameObject PolicePrefab;
    public GameObject[] PoliceSpawns;

    private const int _startingCount = 20;

    private readonly List<VictimScript> _activeVictims = new List<VictimScript>();

    private void Start() => SpawnInitial();

    private void SpawnInitial()
    {
        int successCount = 0;
        int failCount = 0;
        while (successCount < _startingCount && failCount < _startingCount * 20)
        {
            var randomOrigin = transform.position + Random.insideUnitSphere * 20;

            if (SamplePosition(randomOrigin, out var hit, 1, 1))
            {
                var obj = Instantiate(VictimPrefab, hit.position.WithZ(0), Quaternion.identity);
                successCount++;

                var victimScript = obj.GetComponent<VictimScript>();
                victimScript.RequestPolicePresence += PolicePresenceRequested;
                victimScript.Died += VictimDied;
                _activeVictims.Add(victimScript);
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

    private void VictimDied(GameObject victim, VictimScript script)
    {
        script.Died -= VictimDied;
        script.RequestPolicePresence -= PolicePresenceRequested;
        Destroy(victim);
    }

    private void PolicePresenceRequested(Transform location)
    {
        Debug.Log("COP ALERT");
        var nearestSpawn = PoliceSpawns.OrderBy(s =>
                                                    Vector3.Distance(s.transform.position,
                                                                     location.position))
                                       .First();

        var police = Instantiate(PolicePrefab,
                              nearestSpawn.transform.position.WithZ(0),
                              Quaternion.identity);

        var policeNavMeshAgent = police.GetComponent<NavMeshAgent>();
        policeNavMeshAgent.updateRotation = false;
        policeNavMeshAgent.updateUpAxis = false;
        policeNavMeshAgent.destination = location.position;
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
