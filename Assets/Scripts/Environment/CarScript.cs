using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using static UnityEngine.AI.NavMesh;
using UnityEngine.Windows;

public class CarScript : MonoBehaviour
{
    private Vector3 _destination;
    private Vector3 _velocity;
    private const float Speed = 100;

    private void Start()
    {
        _destination = GetRandomPos();
    }

    private Vector3 GetRandomPos()
    {
        for (int i = 0; i < 1000000; i++)
        {
            var randomSpot = transform.position + Random.insideUnitSphere * 31;

            if (SamplePosition(randomSpot, out var hit, 1, 8))
            {
                Debug.Log($"New position ({hit.position.x}, {hit.position.y})");
                return hit.position;
            }
        }

        Debug.Log("nope");

        return Vector3.zero;
    }

    private void Update()
    {
        if(Vector3.Distance(_destination, transform.position) < 0.0001)
        {
            _destination = GetRandomPos();
        }
        else
        {
            NavMeshHit hit = default;
            Vector2 dir = Vector2.zero;

            if (_destination.x - transform.position.x < .1)
            {
                if (_destination.x < transform.position.x)
                {
                    Debug.Log("test left");
                    //sample position check left
                    SamplePosition(transform.position + Vector3.left, out hit, 1, 8);
                    dir = Vector2.left;
                }
                else if (_destination.x > transform.position.x)
                {
                    Debug.Log("test right");
                    //sample position check right
                    SamplePosition(transform.position + Vector3.right, out hit, 1, 8);
                    dir = Vector2.right;
                }
            }
            else if (_destination.y - transform.position.y < .1)
            {
                if (_destination.y > transform.position.y)
                {
                    Debug.Log("test down");
                    //sample position check down
                    SamplePosition(transform.position + Vector3.down, out hit, 1, 8);
                    dir = Vector2.down;
                }
                else if (_destination.y < transform.position.y)
                {
                    Debug.Log("test up");
                    //sample position check up
                    SamplePosition(transform.position + Vector3.up, out hit, 1, 8);
                    dir = Vector2.up;
                }
            }

            Debug.Log($"dest: ({_destination.x}, {_destination.y})");

            if(hit.hit)
            {
                Debug.Log("hit");
                _velocity.x = dir.x != 0 ?
                            Mathf.MoveTowards(_velocity.x,
                                              dir.x * Speed,
                                              Time.deltaTime)
                            : 0;

                _velocity.y = dir.y != 0 ?
                                Mathf.MoveTowards(_velocity.y,
                                                  dir.y * Speed,
                                                  Time.deltaTime)
                                : 0;

                transform.Translate(_velocity.WithZ(0) * Time.deltaTime);
            }
            // if navmesh.sampleposition velocity.x bit mask is drivable, move (x, 0)
            // else if navmeshsample position veloctiy.x bit mask is drivable, move (y, 0)
        }
    }

    private void OnDrawGizmos()
    {
        if (_destination != Vector3.zero)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _destination);
        }
    }
}
