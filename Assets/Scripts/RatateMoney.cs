using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatateMoney : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(150, 0, 0) * Time.fixedDeltaTime);
    }
}
