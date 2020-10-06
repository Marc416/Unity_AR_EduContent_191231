using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    public delegate void Reset();
    public static Reset EnglandReset;
    public static Reset BerninReset;
    public static Reset ItalyReset;
    public static Reset TunuisiaReset;
    public static Reset KoreaReset;
    public GameObject CommonUI;
    //Mission제어
    public bool isEngland;
    public bool isBernin;
    public bool isItaly;
    public bool isTunusia;
    public bool isKorea;

    //영국관련 변수
    [Header("영국:탭하기 전의 주머니들")]
    public GameObject[] InitPantsPocket;

    [Header("영국:탭 한 후 바지에 붙은 주머니들")]
    public GameObject[] AttachedPantsPocket;

    //이태리관련 변수
    [Header("이태리관련변수")]
    public GameObject B0Pants;
    public GameObject Italy;
    public GameObject DyeWater; //[이동 목적지]염색통 안
    public GameObject InitPants; //[염색하기 전의 위치]바지의 처음 위치

    Quaternion rotX;
    bool isDying = false;    //염색중일 때는 움직이는거 못하게

    int initPantsPosX;
    int initPantsPosY;
    int initPantsPosZ;

    int arrivePantsPos;

    Vector3 initPos, midPos, DyePos, arrivePos;
    float speed;    //바지가 움직이는 속도

    //투니지아 관련변수
    [Header("투니지아")]
    public Transform[] dpDecoTr;    //DP창에서 움직이게 해서 바지에 붙게 하기
    public Transform[] pantsDecoTr;  //붙었을 때 키게 하기
    public bool[] isDecoAttached;  //붙으면 더이상 이동 안하게 하기
    Vector3[] dpDecoPos;
    public GameObject[] Indicator;  //바느질 할 곳 손가락으로 표시하기
    public Coroutine BlinkingIndicator;    //인디케이터 코루틴
    public static bool isCanMove;
    public Coroutine IndicatorCoroutine;


    private void Start()
    {
        CommonUI.SetActive(true);
        initPantsPosX = (int)InitPants.transform.localPosition.x;
        initPantsPosY = (int)InitPants.transform.localPosition.y;
        initPantsPosZ = (int)InitPants.transform.localPosition.z;
        isDecoAttached = new bool[dpDecoTr.Length];
        dpDecoPos = new Vector3[dpDecoTr.Length];


        for (int i = 0; i < dpDecoTr.Length; i++)
        {
            //바지에 달린 장식 초기화
            pantsDecoTr[i].gameObject.SetActive(false);
            dpDecoPos[i] = new Vector3(dpDecoTr[i].position.x, dpDecoTr[i].position.y, dpDecoTr[i].position.z);
        }

    }

    public void ResetTunuisiaDpDeco()
    {
        DecoTriggerEvent.DecoNum = 0;
        for (int i = 0; i < dpDecoTr.Length; i++)
        {
            //바지에 달린 장식 초기화
            pantsDecoTr[i].gameObject.SetActive(false);
            isDecoAttached[i] = false;
            dpDecoTr[i].gameObject.SetActive(true);
            dpDecoTr[i].position = dpDecoPos[i];
        }
    }

    /// <summary>
    /// 0: 뒷주머니, 1: 앞주머니
    /// </summary>
    /// <param name="pocketNum"></param>
    public void ENGAttachPantsPocket(int pocketNum)
    {
        switch (pocketNum)
        {
            case 0:
                InitPantsPocket[pocketNum].SetActive(false);
                AttachedPantsPocket[pocketNum].SetActive(true);
                break;

            case 1:
                InitPantsPocket[pocketNum].SetActive(false);
                AttachedPantsPocket[pocketNum].SetActive(true);
                break;
        }
    }

    public void ResetEngland()
    {
        SoundManager.Instance.clickHandBySound[1].SetActive(true);
        RayCast.isPocketFront = false;
        RayCast.isPocketBack = false;
        UIManager.Instance.EngPolyPants.localRotation = Quaternion.Euler(0, 0, 0);
        for(int pocket=0; pocket<InitPantsPocket.Length; pocket++)
        {
            InitPantsPocket[pocket].SetActive(true);
            AttachedPantsPocket[pocket].SetActive(false);
        }
    }

    /// <summary>
    /// Italy에서 바지 염색 후 건조과정 메소드.(Tag활용)_RayCast스크립트에서 사용중
    /// </summary>
    /// <param name="Pants"></param>
    public void ItlyDyPants(GameObject Pants)   //메소드 실행중에 못만지도록 collider disable 시켜야함.
    {
        //Pants.GetComponent<BoxCollider>().enabled = false; 이걸 쓰면 처음 색이 안바뀌니까 불값으로 가자
        if(isDying == false)
            StartCoroutine(PantsMove(Pants));

    }

    IEnumerator PantsMove(GameObject Pants)
    {
        yield return StartCoroutine(InitPantsMove(Pants));
        yield return StartCoroutine(ArrivePantsMove(Pants));
        yield return StartCoroutine(IsDyingFalse());
    }

    IEnumerator InitPantsMove(GameObject Pants)
    {
        isDying = true;  //염색중
        speed = 0;
        initPos = new Vector3(Pants.transform.position.x, Pants.transform.position.y, Pants.transform.position.z);
        DyePos = new Vector3(DyeWater.transform.position.x, DyeWater.transform.position.y, DyeWater.transform.position.z);
        midPos = new Vector3((initPos.x + DyePos.x) / 2, 960, (initPos.z + DyePos.z)/2);
        rotX = Pants.transform.rotation;

        Vector3 newPos = new Vector3();

        while((int)Pants.transform.position.x != (int)DyePos.x && (int)Pants.transform.position.y != (int)DyePos.y && (int)Pants.transform.position.z != (int)DyePos.z)
        {
            //(int)Pants.transform.position.x != (int)DyePos.x && (int)Pants.transform.position.y != (int)DyePos.y && (int)Pants.transform.position.z != (int)DyePos.z
            
            //Pants.transform.position != DyePos
            //Vector3.Distance(Pants.transform.position, DyePos)
            speed += Time.deltaTime;
            Vector3 startPos = Vector3.Lerp(initPos, midPos, speed);
            
            //마지막 목표지점의 우치가 AR위치가 계속 바뀌었기 때문에 transform으로 실시간 위치를 계속 불러와야함.
            //그렇지않으면 목표지점이 계속 달라지는 수가 있음.
            Vector3 endPos = Vector3.Lerp(midPos, DyeWater.transform.position, speed);
            Vector3 startBegiPos = Vector3.Lerp(startPos, endPos, speed);
            newPos = new Vector3(startBegiPos.x, startBegiPos.y, startBegiPos.z);

            //바지 회전
            rotX = Quaternion.Lerp(rotX, Quaternion.Euler(-90f, rotX.y, rotX.z), speed);

            Pants.transform.position = newPos;
            Pants.transform.rotation = rotX;
            yield return new WaitForEndOfFrame();
        }
        //Pants.transform.SetParent(DyeWater.transform);
    }

    IEnumerator ArrivePantsMove(GameObject Pants)
    {
        yield return new WaitForSeconds(2f);
        speed = 0;  //속도 초기화

        Vector3 newPos = new Vector3();
        while (Pants.transform.position != InitPants.transform.position)
        {
            //(int)Pants.transform.position.x != initPantsPosX && (int)Pants.transform.position.y != initPantsPosY && (int)Pants.transform.position.z != initPantsPosZ
            
            speed += Time.deltaTime;
            Vector3 startPos = Vector3.Lerp(DyePos, midPos, speed);
            Vector3 endPos = Vector3.Lerp(midPos, initPos, speed);
            Vector3 ReturnBegiPos = Vector3.Lerp(startPos, endPos, speed);
            newPos = new Vector3(ReturnBegiPos.x, ReturnBegiPos.y, ReturnBegiPos.z);

            rotX = Quaternion.Lerp(rotX, Quaternion.Euler(-65.31001f, rotX.y, rotX.z), speed);

            Pants.transform.position = newPos;
            Pants.transform.rotation = rotX;
            yield return new WaitForEndOfFrame();
        }
        //Pants.transform.SetParent(Italy.transform);
    }
    IEnumerator IsDyingFalse()
    {
        isDying = false;
        yield return null;
    }

    /// <summary>
    /// 바지장식움직이는것을 다른 코드에서 사용
    /// </summary>
    /// <param name="num"></param>
    public void PantsDecoTunisia(int num)
    {
        dpDecoTr[num].GetComponent<MonoBehaviour>().StartCoroutine(DecoToPants(num));
        //
        StartCoroutine(DecoToPants(num));
    }
    
    /// <summary>
    /// 바지 장식 움직여서 바지에 붙게하기
    /// </summary>
    /// <param name="decoNum"></param>
    /// <returns></returns>
    IEnumerator DecoToPants(int decoNum)
    {
        speed = 0;
        initPos = new Vector3(dpDecoTr[decoNum].position.x, dpDecoTr[decoNum].position.y, dpDecoTr[decoNum].position.z);
        arrivePos = new Vector3(pantsDecoTr[decoNum].position.x, pantsDecoTr[decoNum].position.y, pantsDecoTr[decoNum].position.z);
        midPos = new Vector3((initPos.x + arrivePos.x) / 2, (initPos.y + arrivePos.y) / 2 + 60, (initPos.z + arrivePos.z) / 2);
        Vector3 newPos;
        while (isDecoAttached[decoNum] == false)
        {
            speed += Time.deltaTime;

            Vector3 startPos = Vector3.Lerp(initPos, midPos, speed);
            Vector3 endPos = Vector3.Lerp(midPos, arrivePos, speed);
            Vector3 BegiPos = Vector3.Lerp(startPos, endPos, speed);
            newPos = new Vector3(BegiPos.x, BegiPos.y, BegiPos.z);
            dpDecoTr[decoNum].position = newPos;
            yield return new WaitForEndOfFrame();
        }
    }

    public void AnimCtrl(Animator animator, bool issewing, int num)
    {
        if (IndicatorCoroutine != null) StopCoroutine(IndicatorCoroutine);
        IndicatorCoroutine = StartCoroutine(AnimationEnd(animator, issewing, num));
    }

    IEnumerator AnimationEnd(Animator animator, bool issewing, int num)  //여기서 말하는 레이어는 애니메이션 종류를 이야기하는 듯 하다.
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        if(animator.gameObject.name == "Needle_New")
        {
            TIMNiddleCtrl.isSewing  = false;

            //바느질 인디케이터 키는 구간
            if (BlinkingIndicator != null) StopCoroutine(BlinkingIndicator);
            if(num < 4)
            {
                BlinkingIndicator = StartCoroutine(IndicatorBlink(num + 1));
                SoundManager.Instance.PlayAudio(13);
            }

        }
    }
    Coroutine InitBlinkingIndicator;
    public void InitIndicator()
    {
        if (InitBlinkingIndicator != null) StopCoroutine(InitBlinkingIndicator);
        if (BlinkingIndicator != null) StopCoroutine(BlinkingIndicator);
        InitBlinkingIndicator = StartCoroutine(IndicatorBlink(0));
    }

    IEnumerator IndicatorBlink(int next)
    {
        for(int i = 0; i < Indicator.Length; i++)
        {
            Indicator[i].SetActive(false);
        }

        while(TIMNiddleCtrl.isSewing == false)
        {
            Indicator[next].SetActive(true);
            yield return new WaitForSecondsRealtime(.5f);
            Indicator[next].SetActive(false);
            yield return new WaitForSecondsRealtime(.5f);
        }
    }


   

    /// <summary>
    /// 앱종료
    /// </summary>
    public void OnApplicationQuit()
    {
        Debug.Log("종료");
        Application.Quit();
    }

    

}
