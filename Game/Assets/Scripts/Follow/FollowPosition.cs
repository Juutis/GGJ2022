using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPosition : MonoBehaviour
{

    [SerializeField]
    private Transform target;
    [SerializeField]
    private bool FollowX = true;
    [SerializeField]
    private bool FollowY = true;
    [SerializeField]
    private bool FollowZ = true;

    private bool IsEnabled = true;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogWarning($"FollowPosition component of {name} won't work because <b>target</b> is null!");
            IsEnabled = false;
            return;
        }
    }

    void Update()
    {
        if (IsEnabled)
        {
            Vector3 newPos = transform.position;
            if (FollowX) {
                newPos.x = target.position.x;
            }
            if (FollowY) {
                newPos.y = target.position.y;
            }
            if (FollowY) {
                newPos.z = target.position.z;
            }
            transform.position = newPos;
        }
    }
}
