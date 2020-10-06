using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIMUtil
{
    public static void LookTarget(Transform tr, Vector3 myPos, Vector3 targetPos)
    {
        Vector3 degree = Vector3.zero;
        degree.z = GetDegree(myPos, targetPos);
        tr.localEulerAngles = degree;
    }

    static float GetDegree(Vector3 m, Vector3 t)
    {
        float ang = Mathf.Atan2(t.y - m.y, t.x - m.x) * 180 / Mathf.PI;
        if (ang < 0) ang += 360;

        return ang;
    }
}
