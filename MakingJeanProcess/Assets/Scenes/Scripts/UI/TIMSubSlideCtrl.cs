using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TIMSubSlideCtrl : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button Btn_Pocket;
    [SerializeField] Button Btn_Tear;
    [SerializeField] Button Btn_Deco;
    [Header("2nd Postions")]
    [SerializeField] Transform Btn_Tear_2nd;
    [SerializeField] Transform Btn_Deco_2nd;
    [Header("Contents")]
    [SerializeField] Transform Content_Pocket;
    [SerializeField] Transform Content_Tear;
    [SerializeField] Transform Content_Deco;

    TIMSlideButtonData Btn_Tear_Data;
    TIMSlideButtonData Btn_Deco_Data;

    void Start()
    {
        /*
        Btn_Tear_Data = new TIMSlideButtonData
        {
            firstPosition = Btn_Tear.transform.position,
            secondPosition = Btn_Tear_2nd.position
        };
        Btn_Deco_Data = new TIMSlideButtonData
        {
            firstPosition = Btn_Deco.transform.position,
            secondPosition = Btn_Deco_2nd.position
        };
        */
        SetStatus(SLIDE_STATUS.POCKET);
    }

    void Update()
    {
        
    }

    public void OnClick_Pocket()
    {
        SetStatus(SLIDE_STATUS.POCKET);
    }
    public void OnClick_Tear()
    {
        SetStatus(SLIDE_STATUS.TEAR);
    }
    public void OnClick_Deco()
    {
        SetStatus(SLIDE_STATUS.DECO);
    }

    public void SetStatus(SLIDE_STATUS status)
    {
        switch (status)
        {
            case SLIDE_STATUS.POCKET:
                if (Btn_Tear_Data != null) Btn_Tear.transform.position = Btn_Tear_Data.secondPosition;
                if (Btn_Deco_Data != null) Btn_Deco.transform.position = Btn_Deco_Data.secondPosition;
                Content_Pocket.gameObject.SetActive(true);
                Content_Tear.gameObject.SetActive(false);
                Content_Deco.gameObject.SetActive(false);
                break;
            case SLIDE_STATUS.TEAR:
                if (Btn_Tear_Data != null) Btn_Tear.transform.position = Btn_Tear_Data.firstPosition;
                if (Btn_Deco_Data != null) Btn_Deco.transform.position = Btn_Deco_Data.secondPosition;
                Content_Pocket.gameObject.SetActive(false);
                Content_Tear.gameObject.SetActive(true);
                Content_Deco.gameObject.SetActive(false);
                break;
            case SLIDE_STATUS.DECO:
                if (Btn_Tear_Data != null) Btn_Tear.transform.position = Btn_Tear_Data.firstPosition;
                if (Btn_Deco_Data != null) Btn_Deco.transform.position = Btn_Deco_Data.firstPosition;
                Content_Pocket.gameObject.SetActive(false);
                Content_Tear.gameObject.SetActive(false);
                Content_Deco.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
}

[System.Serializable]
public enum SLIDE_STATUS
{
    POCKET,
    TEAR,
    DECO,
}

public class TIMSlideButtonData
{
    public Vector3 firstPosition;
    public Vector3 secondPosition;
}
