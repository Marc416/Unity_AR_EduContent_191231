using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TIMNaviArrowManager : MonoBehaviour
{
    private static TIMNaviArrowManager instance;
    public static TIMNaviArrowManager Get
    {
        get { return instance; }
    }

    [SerializeField] bool isDontDestroy = false;
    [SerializeField] int totalStep;

    [SerializeField] int CurrentStep;

    [Header("Arrows")]
    [SerializeField] List<TIMUIButton> arrowList = new List<TIMUIButton>();

    void Start()
    {
        instance = this;
        if(isDontDestroy) DontDestroyOnLoad(this);

        init();
    }

    void init()
    {
        for (int i = 0; i < arrowList.Count; i++)
        {
            bool isShow = (i < totalStep) ? true : false;
            arrowList[i].gameObject.SetActive(isShow);
        }
        UpdateStep();
    }

    public void SetCurrentStep(int step)
    {
        CurrentStep = step;
        for (int i = 0; i < arrowList.Count; i++)
        {
            arrowList[i].SetButtonStatus((i == step - 1));
        }
    }

    public void UpdateStep()
    {
        SetCurrentStep(CurrentStep);
    }

    public void GoNextStep()
    {
        CurrentStep++;
        if (CurrentStep > totalStep) return;

        SetCurrentStep(CurrentStep);
    }

}
