using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private Killable thingToSpawn;

    [SerializeField]
    private int count;

    private List<Killable> spawnedKillables = new List<Killable>();

    private float nextSpawn;

    private int groundLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", 1.0f, 1.0f);
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Spawn() {
        removeKilledKillables();

        if (spawnedKillables.Count < count && timeToSpawn()) {
            RaycastHit hit;
            var didHit = Physics.Raycast(transform.position, Vector3.down, out hit, 1000, groundLayerMask);

            if (didHit) {
                var obj = Instantiate(thingToSpawn);
                obj.transform.position = hit.point + Vector3.up * 1.0f;
                nextSpawn = Time.time + Random.Range(20f, 30f);
                spawnedKillables.Add(obj);
            } else {
                Debug.LogError("CAN'T SPAWN HERE", this);
            }
        }
    }

    private void removeKilledKillables() {
        var killablesToRemove = new List<Killable>();
        foreach(var killable in spawnedKillables) {
            if (!killable.IsAlive()) {
                killablesToRemove.Add(killable);
            }
        }
        foreach(var killable in killablesToRemove) {
            spawnedKillables.Remove(killable);
        }
    }

    private bool timeToSpawn() {
        return Time.time > nextSpawn;
    }
}
