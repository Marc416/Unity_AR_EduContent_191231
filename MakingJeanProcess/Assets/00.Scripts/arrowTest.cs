using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowTest : MonoBehaviour
{
    

    [Header("시작점")]
    public GameObject init;
    RectTransform initTR;

    [Header("도착점")]
    public GameObject target;
    RectTransform targetTR;

    [Header("사용할 화살 표")]
    public GameObject arrowTranz;

    [Header("캔버스")]
    public Transform canvas;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(IArrowFollow());
        }
    }

    IEnumerator IArrowFollow()
    {
        float speed =10;
        speed += Time.deltaTime;
        
        //시작점과 끝점 변수 설정
        initTR = init.GetComponent<RectTransform>();
        targetTR = target.GetComponent<RectTransform>();
        
        //거리가 얼마나 되나책정
        float dist = Vector3.Distance(targetTR.anchoredPosition, initTR.anchoredPosition);
        Debug.Log(dist);

        //스폰할 플래그 위치 의 개수(비행기와 타겟거리를 150로 나눔)
        Vector3[] arrowSpawn = new Vector3[(int)dist / 150];
        Debug.Log(arrowSpawn.Length);

        //기울기 (y = ax+b) 중 a, b
        float a = (targetTR.position.y - initTR.position.y) / (targetTR.position.x - initTR.position.x);
        float b = targetTR.position.y - a * targetTR.position.x;
        float posX = initTR.position.x;
        Quaternion ang = Quaternion.FromToRotation(initTR.position, targetTR.position);

        //각 스펀 포인트를 생성하고 위치하기.
        for (int i = 0; i < arrowSpawn.Length; i++)
        {
            float posY = a * posX + b; ;
            arrowSpawn[i] = new Vector3(posX, posY, 0);

            //도착지점이 시작지점보다 y축 기준 왼쪽에 가있으면 x좌표를 줄여야 하므로 if문으로 검출
            if (targetTR.position.x > initTR.position.x)
            {
                posX += 150;
            }
            else 
            {
                posX -= 150;
            }
            Debug.Log(arrowSpawn[i]);
            GameObject arrowObject;

            //방향 돌리는거 하고 싶다...
            arrowObject = Instantiate(arrowTranz, arrowSpawn[i], ang, canvas);
            arrowTranz.transform.LookAt(targetTR);
            yield return new WaitForSeconds(.5f);
            //Debug.LogFormat("arrowSpawn({0}):{1}", i, arrowSpawn[i]);
        }
    }
}
