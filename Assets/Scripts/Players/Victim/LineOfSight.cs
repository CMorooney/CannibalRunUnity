using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Alert(Transform source);

public class LineOfSight : MonoBehaviour
{
    // which way we're "facing," set by parent
    // but we'll start with a random direction until things get moving
    public Vector3 SightVector;

    public event Alert Alert;

    private readonly Dictionary<int, GameObject> _nearbyFolk = new Dictionary<int, GameObject>();

    private void Awake()
    {
        SightVector = Random.insideUnitSphere.WithZ(0);
    }

    public GameObject NearbyPolicePole =>
        _nearbyFolk.Values.FirstOrDefault(f => f.CompareTag("policepole"));

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // be careful to only track the things we care about
        if(collision.gameObject.CompareTagsOR("cannibal", "victim", "policepole"))
        {
            _nearbyFolk.Add(collision.gameObject.GetInstanceID(), collision.gameObject);
        }
    }

    private void Update()
    {
        foreach(var gameObject in _nearbyFolk.Values)
        {
            var headingToObject = gameObject.transform.position - transform.position;
            var dot = Vector3.Dot(SightVector, headingToObject);

            //gameObject should be in line if sight (within 90deg of sight vector)
            if(dot > 0)
            {
                if (gameObject.CompareTagsOR("alertedvictim", "cannibal"))
                {
                    Alert?.Invoke(gameObject.transform);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) => _nearbyFolk.Remove(collision.gameObject.GetInstanceID());

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, (transform.position + SightVector.normalized));

        Gizmos.color = Color.blue;
        foreach (var gameObject in _nearbyFolk.Values)
        {
            var dot = Vector3.Dot(SightVector, gameObject.transform.position - transform.position);
            if (dot > 0)
            {
                Gizmos.DrawLine(transform.position, gameObject.transform.position);
            }
        }
    }
}
