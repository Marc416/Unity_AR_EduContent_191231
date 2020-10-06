using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve : MonoBehaviour
{

    //테스트중
    [Header("비행기변수")]
    public RectTransform planeTR;
    //비행기 위치
    float planeX;
    float planeY;

    [Header("타겟변수, 나라")]
    public RectTransform redcube;
    float targetposx;
    float targetposy;

    Vector3 target;
    Vector3 plane;
    Vector3 mid;
    float Speed =0;

    private Vector2 newPos = new Vector2();

    private void Start()
    {
        planeX = planeTR.position.x;
        planeY = planeTR.position.y;

        targetposx = redcube.position.x;
        targetposy = redcube.position.y;

        target = new Vector3(targetposx, targetposy, 0);
        plane = new Vector3(planeX, planeY, 0);
        mid = new Vector3((planeX + targetposx)/2 , ((planeY + targetposy)/2) + 600, 0);

    }

    // Use this for initialization
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(IPlaneMoving());
        }
    }

    IEnumerator IPlaneMoving()
    {
        while(planeTR.position != target)
        {
            Speed += 0.3f* Time.deltaTime;
            Vector3 a = Vector3.Lerp(plane, mid, Speed);
            Vector3 b = Vector3.Lerp(mid, target, Speed);
            Vector3 ab = Vector3.Lerp(a, b, Speed);

            newPos = new Vector2(ab.x, ab.y);
            planeTR.position = newPos;

            yield return new WaitForSeconds(0.01f);
        }

    }


}


