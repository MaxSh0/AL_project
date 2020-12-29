using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHeal : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 100, 0) * Time.fixedDeltaTime);
    }
}
