using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject StartScene;   //start폴더

    public GameObject WorldMapMovingCanvas; //세계지도 캔버스

    [Header("국가별 이미지")]
    public GameObject[] Picture;

    [Header("0~4씬까지 비행기 이동시의 백그라운드 이미지")]
    public GameObject[] WorldMapEachImage;  //나라이동별 백그라운드 세계지도.

    [Header("왼쪽 상단 네비게이션")]
    public GameObject UICanvas; //왼쪽 상단의 캔버스 네비게이션
    public GameObject EnglandArrow;  //처음시작할때 영국부터 시작하는데 클릭없이 단계를 넘어가는 시기이므로 소리를 틀기위한것임.

    public GameObject[] BlueArrow;  //활성화된 Arrow(Navi)
    public GameObject[] GrayArrow;  //미션이 클리어 되면 Image만 교체해 주자.[Gray arrow]

    public GameObject[] NationObject;   //Image Target폴더안의 Start~04Korea게임오브젝트 껐다 켰다 하기위함
    public int NationStageState = 0;        //체험하기를 실행하기 위한 현재 나라의 스테이지를 알려줌.

    public GameObject[] NationImage;    //각 나라마다 소개하는 이미지들
    bool Maximize = false;  //사진보기 버튼 클릭 시 커졌다가 작아졌다가 하게하기.

    float NationObjectLength;
    // Start is called before the first frame update

    public static UIManager Instance = null;

    public Image missionClearedImage;   //미션 클리어된 네이비 Navi이미지
    public Image missionNotClearedImage;    //미션 클리어 안된 이미지 파란색

    CanvasGroup Alpha;
    Coroutine AlphaCoroutine;

    [Header("영국")]
    public Transform EngPolyPants;

    //투니지아
    public Transform PantsPivot;
    public float TunisiaFront = -69.15601f;
    float TunisiaBack = -69.15601f + 180f;

    //한국
    [Header("한국")]
    public Animator krBoxAnim;
    public Animator krPantsAnim;
    public GameObject CelebrationEffect;
    Coroutine EpilogueCoroutine;


    //UI확대 축소시 위치 
    Vector3 initImagePos;
    [Header("확대할 이미지 중앙 위치")]
    public Transform centerPivot;

    [Header("UI-에필로그시 하단 버튼 제어")]
    public GameObject PictureButton;
    public GameObject ExperienceButton;
    public TIMWorldMapManager mapManager;
    public GameObject DeliveryButton;

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


    void Start()
    {
        NationObjectLength = NationObject.Length;
        krBoxAnim.speed = 0;
        krPantsAnim.speed = 0;
    }

    /// <summary>
    /// 미션이 클리어 되면 현재 국가이전의 네비 색상을 바꾼다.
    /// </summary>
    public void ChangeColorClearedNavi(int nation)
    {
        StopCoroutine(AlphaCoroutine);
        //파란색 네비로 리셋
        for(int nationReset = 0; nationReset < 5; nationReset++)
        {
            GrayArrow[nationReset].GetComponent<Image>().sprite = missionNotClearedImage.sprite;
            GrayArrow[nationReset].GetComponent<CanvasGroup>().alpha = 1;
        }

        for (int i = 0; i < nation; i++)
        {
            GrayArrow[i].GetComponent<Image>().sprite = missionClearedImage.sprite;
        }
    }

    /// <summary>
    /// 좌측 상단 네비게이션 바를 클릭할 때의 기능, UI색 제어
    /// </summary>
    /// <param name="NationNum"></param>
    public void NationOnOffCtrlByButton(int NationNum)
    {
        if (mapManager != null) mapManager.StopAllCoroutines(); //튀니지 바느질을 해달라는 코루틴을 정지시킴 중복방지
        if (GameManager.Instance.IndicatorCoroutine != null) StopCoroutine(GameManager.Instance.IndicatorCoroutine);

        switch (NationNum)
            {
                //영국
                case 0:
                if(GameManager.Instance.isEngland == true)
                {
                    MapReset();
                    WorldMapMovingCanvas.SetActive(true);   //WorldMap 캔버스를 킨다.
                    PictureButton.SetActive(true);
                    ExperienceButton.SetActive(true);
                    DeliveryButton.SetActive(false);

                    BlueArrow[NationNum].SetActive(true);   //현재위치의 네비게이션을 교체한다.
                    TIMWorldMapManager.MovePlaneInMap(NationNum);
                    NationStageState = NationNum;   //국가의 넘버링을 재설정한다
                    SoundManager.Instance.PlayTwoAudio(NationNum+5, 2); //해당 국가의 오디오를 튼다.
                    ChangeColorClearedNavi(NationStageState);   //미션 클리어한 네비 색상 바꾸기
                    ETCSoundEffectManager.Instance.PlayEffectSound(0);

                }

                break;

                //베넹
                case 1:
                    if (GameManager.Instance.isEngland == true)
                    {
                        MapReset();
                        WorldMapMovingCanvas.SetActive(true);
                        PictureButton.SetActive(true);
                        ExperienceButton.SetActive(true);
                        DeliveryButton.SetActive(false);

                        BlueArrow[NationNum].SetActive(true);
                        TIMWorldMapManager.MovePlaneInMap(NationNum);
                        NationStageState = NationNum;
                        SoundManager.Instance.PlayTwoAudio(NationNum + 5, 2);
                        ChangeColorClearedNavi(NationStageState);   //미션 클리어한 네비 색상 바꾸기
                        ETCSoundEffectManager.Instance.PlayEffectSound(0);
                    }
                    break;

                //이태리
                case 2:
                    if(GameManager.Instance.isBernin == true)
                    {
                        MapReset();
                        WorldMapMovingCanvas.SetActive(true);
                        PictureButton.SetActive(true);
                        ExperienceButton.SetActive(true);
                        DeliveryButton.SetActive(false);

                        BlueArrow[NationNum].SetActive(true);
                        TIMWorldMapManager.MovePlaneInMap(NationNum);
                        NationStageState = NationNum;
                        SoundManager.Instance.PlayTwoAudio(NationNum + 5, 2);
                        ChangeColorClearedNavi(NationStageState);   //미션 클리어한 네비 색상 바꾸기
                        ETCSoundEffectManager.Instance.PlayEffectSound(0);
                    }
                    break;

                //튀니지
                case 3:
                if(GameManager.Instance.isItaly == true)
                {
                    MapReset();
                    WorldMapMovingCanvas.SetActive(true);
                    PictureButton.SetActive(true);
                    ExperienceButton.SetActive(true);
                    DeliveryButton.SetActive(false);

                    BlueArrow[NationNum].SetActive(true);
                    TIMWorldMapManager.MovePlaneInMap(NationNum);
                    NationStageState = NationNum;
                    SoundManager.Instance.PlayTwoAudio(NationNum + 5, 2);
                    ChangeColorClearedNavi(NationStageState);   //미션 클리어한 네비 색상 바꾸기
                    ETCSoundEffectManager.Instance.PlayEffectSound(0);
                }
                    break;

                //한국
                case 4:
                if(GameManager.Instance.isTunusia == true)
                {
                    MapReset();
                    WorldMapMovingCanvas.SetActive(true);
                    PictureButton.SetActive(false);
                    ExperienceButton.SetActive(false);
                    DeliveryButton.SetActive(true);

                    BlueArrow[NationNum].SetActive(true);
                    TIMWorldMapManager.MovePlaneInMap(NationNum);
                    NationStageState = NationNum;
                    SoundManager.Instance.PlayTwoAudio(NationNum + 5, 16); 
                    ChangeColorClearedNavi(NationStageState);   //미션 클리어한 네비 색상 바꾸기
                    ETCSoundEffectManager.Instance.PlayEffectSound(0);
                }
                    break;
                //에필로그
                case 5:
                    MapReset();
                    WorldMapMovingCanvas.SetActive(true);
                    PictureButton.SetActive(false);
                    ExperienceButton.SetActive(false);
                    DeliveryButton.SetActive(false);
                    SoundManager.Instance.PlayAudio(15);    //에필로그 소리
                    TIMWorldMapManager.MoveWorldArround(0);
                    ChangeColorClearedNavi(5);   // x 미션 클리어한 네비 색상 바꾸기 바꿀 색상이 없으므로 이거는 진행 안해도될듯
                    break;
            }

        

    }

    IEnumerator EpilouGue()
    {
        for(int nation = 0; nation < 5; nation++)
        {
            //사진 초기화(다끄기)
            for(int pic = 0; pic < 5; pic++)
            {
               // WorldMapEachImage[pic].SetActive(false);
                Picture[pic].SetActive(false);
            }
            if(nation !=0)
            //WorldMapEachImage[nation].SetActive(true);  //국가별 사진 키기(이메소드는 사진을 키느게 아니고 지도를 키는 것임 수정할 필요가 있음.)
                Picture[nation].SetActive(true);
            yield return new WaitForSeconds(3f);
            //if (nation == 4)
            //    TIMCommonAssetCtrl.Close(); //2.5초뒤 닫기 안내패널 나오기.
        }
    }

    public void MapReset()
    {
        for (int nation = 0; nation < NationObjectLength; nation++)
        {
            NationObject[nation].SetActive(false);      //체험 국가 오브젝트 끄기
            //WorldMapEachImage[nation].SetActive(false);     //국가이동별 Map끄기
            BlueArrow[nation].SetActive(false);     //안간지역 끄기
            //Picture[nation].SetActive(false);   //사진 끄기
        }
    }

    /// <summary>
    /// 첫 씬에서 바지를 클릭했을 때(프로그램중 딱 1번 등장)
    /// </summary>
    public void ClickPants_AtStart()
    {
        //if(SoundManager.Instance.audiosource.isPlaying == false)
        //{
        ETCSoundEffectManager.Instance.PlayEffectSound(0);
        StartScene.SetActive(false);
        WorldMapMovingCanvas.SetActive(true);
        UICanvas.SetActive(true);
        if(EnglandArrow.activeSelf == true)
        {
            SoundManager.Instance.PlayTwoAudio(5, 2); //England오디오를 튼다.(숫자별로 설명 적어놓을것.)
            TIMWorldMapManager.MovePlaneInMap(0);

        }
    }
    /// <summary>
    /// 체험하기 버튼
    /// </summary>
    public void Experience()
    {
        ETCSoundEffectManager.Instance.PlayEffectSound(0);
        try
        {
            //켜져있는 국가 체험하기 있으면 모두 끄기
            for (int nation = 0; nation < NationObjectLength; nation++)
            {
                NationObject[nation].SetActive(false);
            }
            WorldMapMovingCanvas.SetActive(false);
            NationObject[NationStageState].SetActive(true);
            SoundManager.Instance.PlayAudio(NationStageState + 10);
            if (GameManager.Instance.IndicatorCoroutine != null) StopCoroutine(GameManager.Instance.IndicatorCoroutine);

            switch (NationStageState)
            {
                case 0: //영국 초기화
                    GameManager.Instance.ResetEngland();
                    break;
                case 1: //베넹 초기화
                    GameManager.BerninReset();
                    break;
                case 2: //이태리 초기화
                    break;
                case 3: //튀니지 초기화
                    GameManager.TunuisiaReset();
                    GameManager.Instance.ResetTunuisiaDpDeco();
                    break;
                case 4: //한국 초기화
                    ResetKorea();
                    break;
            }
           
        }
        catch (System.Exception e )
        {
            Debug.Log(e.ToString());
        }
        
    }

    public void KROpenBox()
    {
        krBoxAnim.speed = 1;
        krPantsAnim.speed = 1;
        CelebrationEffect.SetActive(true);
        ETCSoundEffectManager.Instance.PlayEffectSound(4);
        Invoke("SetOffAnim", 2);
    }

    public void ResetKorea()
    {
        krBoxAnim.enabled = true;
        krPantsAnim.enabled = true;
        krBoxAnim.speed = 0;
        krPantsAnim.speed = 0;
        CelebrationEffect.SetActive(false);

    }

    void SetOffAnim()
    {
        krPantsAnim.enabled = false;
        GameManager.Instance.isKorea = true;
        StartCoroutine(startEpilogue());    //에필로그 보여주기
    }

    IEnumerator startEpilogue()
    {
        yield return new WaitForSeconds(2);
        NationOnOffCtrlByButton(5); //에필로그보여주기
    }

    /// <summary>
    /// 켜져있는 국가 있으면 모두 끄기
    /// </summary>
    public void FalseNationObject()
    {
        for (int nation = 0; nation < NationObjectLength; nation++)
        {
            NationObject[nation].SetActive(false);
        }
    }

    /// <summary>
    /// 사진보기 클릭시 크게 키우는 기능(위치 지정해 줘야 할 지도 모름..)
    /// </summary>
    public void MaxmizePhoto()
    {
        ETCSoundEffectManager.Instance.PlayEffectSound(0);
        if (Maximize == false)
        {
            for(int photonum=0; photonum<NationObjectLength; photonum++)
            {
                if (BlueArrow[photonum].activeSelf == true)
                {
                    initImagePos = new Vector3();
                    initImagePos = NationImage[photonum].transform.position;
                    NationImage[photonum].transform.position =centerPivot.position;
                    NationImage[photonum].transform.localScale = new Vector3(2, 2, 2);
                    Maximize = true;
                }
            }
        }
        else
        {
            for (int photonum = 0; photonum < NationObjectLength; photonum++)
            {
                if (BlueArrow[photonum].activeSelf == true)
                {
                    NationImage[photonum].transform.position = initImagePos;
                    NationImage[photonum].transform.localScale = new Vector3(1, 1, 1);
                    Maximize = false;
                }
            }
        }

    }

    public void TunFrontButton()
    {
        ETCSoundEffectManager.Instance.PlayEffectSound(0);
        PantsPivot.localRotation = Quaternion.Euler(TunisiaFront, 90, 90);
    }
    public void TunBackButton()
    {
        ETCSoundEffectManager.Instance.PlayEffectSound(0);
        PantsPivot.localRotation = Quaternion.Euler(TunisiaBack, 90, 90);
    }

    /// <summary>
    /// 돌면서 소리가 나가지고 소리안나는 뒤돌기 메소들 만듬.
    /// </summary>
    public void TunMuteTunBack()
    {
        PantsPivot.localRotation = Quaternion.Euler(TunisiaBack, 90, 90);
    }

    public void EngFrontButton()
    {
        ETCSoundEffectManager.Instance.PlayEffectSound(0);
        EngPolyPants.localRotation = Quaternion.Euler(0, 0, 360);
    }
    public void EngBackButton()
    {
        ETCSoundEffectManager.Instance.PlayEffectSound(0);
        EngPolyPants.localRotation = Quaternion.Euler(0, 0, 180);
    }

    /// <summary>
    /// 미션 클리어 하고 다음 국가 누르라고 하면 위의 유아이가 반짝이게 함.
    /// </summary>
    /// <param name="nownation"></param>
    public void NaviBlink(int nownation)
    {
        nownation += 1;
        Alpha = GrayArrow[nownation].GetComponent<CanvasGroup>();

        if (AlphaCoroutine != null) StopCoroutine(AlphaCoroutine);
        AlphaCoroutine =  StartCoroutine(Blinking(nownation));

    }

    IEnumerator Blinking(int nowNation)
    {
        while(UIManager.Instance.NationStageState == nowNation-1)
        {
            for(float i = 0; i<1; i+=.04f)
            {
                Alpha.alpha+=.04f;
                yield return new WaitForEndOfFrame();
            }
            for (float i = 1; i > 0; i-=.04f)
            {
                Alpha.alpha-=.04f;
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForEndOfFrame();
        }
    }

}
