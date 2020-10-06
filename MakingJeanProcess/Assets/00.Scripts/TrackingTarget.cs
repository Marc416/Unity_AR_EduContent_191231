using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TrackingTarget : MonoBehaviour, ITrackableEventHandler
{
    //카메라 Depth바꾸기 위함
    public Camera ARCamera;
    public Camera camera;   //메인카메라
    public GameObject NationStart;  //마커 입력되면 Start오브젝트 실행
    public GameObject Floor;    //바닥 모델 키기

    // 트래킹 이벤트를 등록할 클래스
    private TrackableBehaviour track;
    public GameObject YellowPanel;
    bool targetDetected = false;
    // 활성화하거나 비활성화할 FollowCamera 스크립트를 저장할 배열
    [SerializeField]
    //private FollowCamera[] scripts;

    void Start()
    {
        // 모든 FollowCamera 스크립트를 검색해 저장
       // scripts = GameObject.FindObjectsOfType<FollowCamera>();

        track = GetComponent<TrackableBehaviour>();

        if (track)
        {
            // 트래킹 이벤트가 발생하면 이 클래스로 이벤트를 전달받을 수 있게 등록
            track.RegisterTrackableEventHandler(this);
        }
    }

    void OnDisable()
    {
        track.UnregisterTrackableEventHandler(this);
    }

    // 트래킹의 상태가 변경될 때 발생하는 이벤트(Refactor 기능을 이용해 작성)
    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
    {
        // 트래킹이 감지/진행/확장 트래킹 상태로 변경되는 경우를 판단
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED || 
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            // //ScriptEnable(true);
            // if (!GameManager1.Inst.gArrow.activeSelf)
            // {
            //     GameManager1.Inst.gArrow.SetActive(true);
            //     //if(SoundManager.Inst != null) SoundManager.Inst.Play("EF_Sin1_Narrtion_M01");
            // }
            
            //한번만 실행하게 하기 위함.
            if (targetDetected == false)
            {
                targetDetected = true;
                YellowPanel.SetActive(false);
                SoundManager.Instance.PlayFirstAudio();
                ARCamera.depth = 0;
                camera.depth = 1;
                NationStart.SetActive(true);    //첫 인트로 시작(바지를 클릭하세요.)
                Floor.SetActive(true);

            }
            //GameManager1.Inst.isSound++;

        }
        else
        {
            //ScriptEnable(false);
        }
    }

    // 스크립트의 활성화 비활성화 상태를 결정
    void ScriptEnable(bool _enabled)
    {
        //foreach (var script in scripts)
        //{
        //    script.enabled = _enabled;

        //    if (!_enabled)
        //    {
        //        //script.gameObject.transform.parent = Camera.main.transform;
        //    }
        //}
    }
}
