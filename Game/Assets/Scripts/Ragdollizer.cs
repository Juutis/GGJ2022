using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ragdollizer : MonoBehaviour
{
    public GameObject RagdollRoot;

    private Animator anim;
    private List<Collider> ragdollColliders;
    private List<Rigidbody> ragDollRigidbodys;
    public Collider coll;

    private LayerMask ragdollMask;

    public bool DebugTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        ragDollRigidbodys = RagdollRoot.GetComponentsInChildren<Rigidbody>().ToList();
        ragdollColliders = RagdollRoot.GetComponentsInChildren<Collider>().ToList();

        enableRagdoll(false);
        ragdollMask = LayerMask.GetMask("Ragdoll");
    }

    // Update is called once per frame
    void Update()
    {
        if (DebugTrigger) {
            DebugTrigger = false;
            Activate(transform.position + Vector3.up, -transform.forward, 100.0f);
        }
    }

    public void Activate(Vector3 forcePosition, Vector3 forceDirection, float forceAmount)
    {
        anim.enabled = false;
        enableRagdoll(true);

        ApplyForce(forcePosition, forceDirection, forceAmount);
    }

    private void enableRagdoll(bool enable)
    {
        foreach (var rb in ragDollRigidbodys)
        {
            rb.isKinematic = !enable;
            rb.useGravity = enable;
        }
        foreach (var c in ragdollColliders)
        {
            c.enabled = enable;
        }
        if (coll) {
            coll.enabled = !enable;
        }
    }
    
    public void ApplyForce(Vector3 location, Vector3 direction, float amount)
    {
        direction = direction.normalized;
        var rayOrigin = location - direction * 5.0f;
        for (var i = 0; i < 10; i++)
        {
            var rayRadius = i * 0.1f;
            var hits = Physics.SphereCastAll(rayOrigin, rayRadius, direction, 20.0f, ragdollMask);

            var hitsToThisRagdoll = hits.Where(hit => hit.rigidbody != null)
                .Where(hit => ragDollRigidbodys.Contains(hit.rigidbody))
                .ToList();

            if (hitsToThisRagdoll.Count > 0)
            {
                hitsToThisRagdoll.ForEach(hit => hit.rigidbody.AddForceAtPosition(amount * direction, hit.point, ForceMode.Impulse));
                break;
            }
        }
    }
}