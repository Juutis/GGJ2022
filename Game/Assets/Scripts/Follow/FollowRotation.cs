using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRotation : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private bool IsEnabled = true;
    void Start() {
        if (target == null) {
            IsEnabled = false;
            Debug.LogWarning($"FollowRotation component of {name} won't work because <b>target</b> is null!");
            return;
        }
    }

    private void Update() {
        if (IsEnabled) {
            transform.rotation = target.rotation;
        }
    }
}
