using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RotateScaleProps : MonoBehaviour
{
    List<RotateAndScale> things;

    // Start is called before the first frame update
    void Start()
    {
        things = FindObjectsOfType<RotateAndScale>().ToList();
        foreach (RotateAndScale thing in things)
        {
            Transform t = thing.transform;
            float scale = Random.Range(thing.minScale, thing.maxScale);
            t.localScale = new Vector3(scale, scale, scale);
            Vector3 rotate = Vector3.zero;
            if (thing.rotateX)
            {
                rotate.x = Random.Range(-180, 180);
            }
            if (thing.rotateY)
            {
                rotate.y = Random.Range(-180, 180);
            }
            if (thing.rotateZ)
            {
                rotate.z = Random.Range(-180, 180);
            }

            t.localRotation = Quaternion.Euler(rotate);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
