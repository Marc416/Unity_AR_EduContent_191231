using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ted 20191223
/// </summary>
public class TIMArrowGridCtrl : MonoBehaviour
{
    [Header("시작 지점")]
    [SerializeField] Vector3 startPos;
    [Header("목표 지점")]
    [SerializeField] Vector3 targetPos;
    [Header("화살표 가로 사이즈")]
    [SerializeField] int size_Width;
    [SerializeField] GameObject[] arrows;

    RectTransform rectTr;
    [SerializeField] float distance;
    
    private void Awake()
    {
        rectTr = this.GetComponent<RectTransform>();
    }

    public void SetData(Vector3 s_pos, Vector3 e_pos)
    {
        startPos = s_pos;
        targetPos = e_pos;

        rectTr.anchoredPosition = startPos;
        TIMUtil.LookTarget(this.transform, startPos, targetPos);
        distance = Vector3.Distance(startPos, targetPos);

        int count = (int)(distance / size_Width)-2;
        for (int i = 0; i < arrows.Length; i++)
        {
            if (i < count) arrows[i].SetActive(true);
            else arrows[i].SetActive(false);
        }
    }

}
