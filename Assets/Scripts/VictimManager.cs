using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.AI.NavMesh;

public class VictimManager : MonoBehaviour
{
    public GameObject VictimPrefab;

    private const int _startingCount = 4;
    private const int _maxCount = 10;

    private void Start()
    {
        SpawnInitial();
    }

    private void SpawnInitial()
    {
        int successCount = 0;
        while (successCount < _startingCount)
        {
            var randomOrigin = new Vector3() + Random.insideUnitSphere * 20;
            if (SamplePosition(randomOrigin, out var hit, 1, AllAreas))
            {
                successCount++;
                Instantiate(VictimPrefab, hit.position.WithZ(0), Quaternion.identity);
            }
        }
    }
}
