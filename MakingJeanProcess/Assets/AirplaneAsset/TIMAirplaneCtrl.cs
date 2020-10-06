using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ted 20191223
/// </summary>
public class TIMAirplaneCtrl : MonoBehaviour
{
    [Header("시작/끝 지점(RectTrasform.Position)")]
    [SerializeField] Vector2 startPos;
    [SerializeField] Vector2 endPos;
    RectTransform tempPlane;
    void Start()
    {
        tempPlane = this.GetComponent<RectTransform>();
    }

    public void MovePlane(Vector2 s_pos, Vector2 e_pos)
    {
        startPos = s_pos;
        endPos = e_pos;
        tempPlane.anchoredPosition = startPos;
        TweenObjectToPostion(endPos, 2.0f);
    }

    /// <summary>
    /// Ted, 191223
    /// iTween 으로 Rectransform 이동시키기
    /// </summary>
    /// <param name="target">RectTrasform 을 가진 움직일 대상 오브젝트</param>
    /// <param name="targetPosition">이동시킬 위치</param>
    /// <param name="time">이동 시간</param>
    public void TweenObjectToPostion(Vector2 targetPosition, float time)
    {
        TIMUtil.LookTarget(this.transform, tempPlane.anchoredPosition, targetPosition);

        iTween.ValueTo(tempPlane.gameObject, iTween.Hash(
        "from", tempPlane.anchoredPosition,
        "to", targetPosition,
        "time", time,
        "onupdatetarget", this.gameObject,
        "onupdate", "OnUpdateTween",
        "oncompletetarget", gameObject,
        "oncomplete", "OnCompleteTween"
        ));

        Vector2 scale = new Vector3(3, 3, 1);
        SetScale(scale, time);
        StartCoroutine(SetScaleDown(Vector3.one, time));
    }

    /// <summary>
    /// 이동 시 프레임 단위로 호출 되는 Callback 함수
    /// </summary>
    /// <param name="position"></param>
    public void OnUpdateTween(Vector2 position)
    {
        //Debug.Log("OnUpdateTween : " + position.ToString());
        tempPlane.anchoredPosition = position;
    }

    /// <summary>
    /// 이동이 종료되면 호출 되는 Callback 함수
    /// </summary>
    public void OnCompleteTween() 
    {

    }

    /// <summary>
    /// Scale 조정
    /// </summary>
    /// <param name="scale"></param>
    /// <param name="time"></param>
    public void SetScale(Vector3 scale, float time)
    {
        

        iTween.ValueTo(tempPlane.gameObject, iTween.Hash(
           "from", tempPlane.localScale,
           "to", scale,
           "time", time / 2,
           "onupdatetarget", this.gameObject,
           "onupdate", "OnUpdateScale"
           ));
    }

    public void OnUpdateScale(Vector3 s)
    { 
        //Debug.Log("OnUpdateScale : " + s.ToString());
        tempPlane.localScale = s;
    }

    IEnumerator SetScaleDown(Vector3 scale, float time)
    {
        yield return new WaitForSeconds(time / 2);
        SetScale(scale, time);
    }
}

