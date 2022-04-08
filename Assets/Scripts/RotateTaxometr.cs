using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTaxometr : MonoBehaviour
{
    public void rotateStrelka(float rpm)
    {
        float angle = (rpm - 5661)/ -33.3f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
