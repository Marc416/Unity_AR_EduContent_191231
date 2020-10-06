using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{
    [Header("활대할것")]
    public Transform BerninStuff;
    public Transform PantsPivotTunisiaPants;
    public Transform PantsPivotKorea;
    Vector3 maxSize;
    Vector3 minSize;

    Ray ray;
    RaycastHit hit;
    Camera arcamera;

    Touch touch;
    Vector3 touchPosVector3;
    Vector3 touchPos;
    string Name;
    string Tag;

    Transform target;
    float targetY;
       //두손가락 확대시 손가락의 거리

    float initTouchDist;
    float lastTouchDIst;

    //영국 : 포켓 디자인을 한번 하면 터치손가락 사라지고 다 터치하면 다음 유아이로 이동 가이드해줌
    public static bool isPocketFront;
    public static bool isPocketBack;


    private void OnEnable()
    {
        GameManager.BerninReset += returnSizeNRot;
        GameManager.TunuisiaReset += returnSizeNRot;
        GameManager.KoreaReset += returnSizeNRot;
    }

    private void OnDisable()
    {
        GameManager.BerninReset -= returnSizeNRot;
        GameManager.TunuisiaReset -= returnSizeNRot;
        GameManager.KoreaReset -= returnSizeNRot;
    }

    // Start is called before the first frame update
    void Start()
    {
        arcamera = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (SoundManager.Instance.audiosource.isPlaying == false)
        //{
        if (Input.touchCount > 0)   //오디오 소리 나오는 동안은 레이케스트 막아놓음 근데 여기서 에러가 뜨는거 같음..
        {
            touch = Input.GetTouch(0);
            
            //레이를 쏘기위한 목표지점 위치값설정
            touchPosVector3 = new Vector3(touch.position.x, touch.position.y, 3.03f);
            ray = Camera.main.ScreenPointToRay(touchPosVector3);

            Debug.DrawLine(ray.origin, ray.direction, Color.red, 1f);

            if(Physics.Raycast(ray, out hit))
            {
                //콜라이더를 움직이기위해 콜라이더의 위치값을 받기.(x,y는 터치값 z값은 콜라이더의 현재 z값으로 위아래로 못움직이게 고정.)
                Vector3 hitCollider = new Vector3(touchPosVector3.x, touchPosVector3.y, hit.collider.gameObject.transform.position.z);
                touchPos = Camera.main.ScreenToWorldPoint(hitCollider);

                //Debug.DrawLine(ray.origin, ray.direction, Color.red, 0.5f);
                Name = hit.collider.gameObject.name;
                Tag = hit.collider.gameObject.tag;
                target = hit.collider.gameObject.transform;
               
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (Input.touchCount > 0 )//&& SoundManager.Instance.audiosource.isPlaying == false
                        {
                            switch (Name)
                            {
                                //영국에서 뒷주머니 디자인할 때(터치하면 바지에 붙게하기)
                                //바지 데코레이션은 숫자로 매겨놨음
                                //0 : 뒷주머니, 1: 앞주머니
                                case "DpBackPocket":    //뒷주머니
                                    GameManager.Instance.ENGAttachPantsPocket(0);
                                    ETCSoundEffectManager.Instance.PlayEffectSound(1);
                                    isPocketBack = true;
                                    EngMission();
                                    break;
                                case "DpFrontPocket":    //앞주머니
                                    GameManager.Instance.ENGAttachPantsPocket(1);
                                    ETCSoundEffectManager.Instance.PlayEffectSound(1);
                                    isPocketFront = true;
                                    EngMission();

                                    break;
                                case "Courier_Box":    //한국 장면에서 박스 터치
                                    UIManager.Instance.KROpenBox();
                                    break;
                            }
                        }

                        switch (Tag)
                        {
                            case "DyeWater":
                                GameManager.Instance.ItlyDyPants(target.gameObject);
                                    ETCSoundEffectManager.Instance.PlayEffectSound(1);
                                break;

                            case "Parts":   //투니지아 바느질 할 때 사용. 콜라이더 터치하면 바느질 시작
                                //if (Input.touchCount > 0 && SoundManager.Instance.audiosource.isPlaying == false)   //소리나면 실행 못하게 막기
                                //{
                                //}
                                    TIMNiddleCtrl.NiddleSewing();
                                break;

                        }
                        break;

                    case TouchPhase.Moved:
                        switch (Name)
                        {
                            case "Cube":
                                //target.position= new Vector3(0, 0, 0);
                                target.position = Vector3.MoveTowards(target.position, touchPos, 1000*Time.deltaTime);
                                break;
                        }
                        switch (Tag)
                        {
                            case "PANTS":
                                target.Rotate(0, -touch.deltaPosition.x, 0);
                                break;

                            case "TunuisiaPants":
                                if(GameManager.isCanMove == true)
                                {
                                    target.Rotate(0, -touch.deltaPosition.x, 0);
                                }
                                break;

                            case "POLYGON":
                                target.Rotate(0, 0, -touch.deltaPosition.x);
                                break;
                        }
                        break;
                    case TouchPhase.Ended:
                        //손뗗을 때

                        break;
                    #region 안쓰는 기능
                    case TouchPhase.Stationary:
                        //기능 : 터치유지, 안쓸거임
                        break;
                        #endregion
                }
            }
        }
               
        //손가락 두개일 때
        if (Input.touchCount == 2)
        {
            lastTouchDIst = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
            //float maxORmini = Mathf.Abs(lastTouchDIst - initTouchDist);
            float maxORmini = .05f;
            if (lastTouchDIst > initTouchDist)
            {
                maxSize = new Vector3(1.3f, 1.3f, 1.3f);
                minSize = new Vector3(.5f, .5f, .5f);
                    //확대
                if(BerninStuff.gameObject.activeSelf == true)
                {
                    if(BerninStuff.localScale.x < 1f)   //확대 크기 제어하기
                        BerninStuff.localScale += new Vector3(maxORmini, maxORmini, maxORmini);
                }
                if(PantsPivotTunisiaPants.gameObject.activeSelf == true && GameManager.isCanMove ==true)
                {
                    if (PantsPivotTunisiaPants.localScale.x < maxSize.x)   //확대 크기 제어하기
                        PantsPivotTunisiaPants.localScale += new Vector3(maxORmini, maxORmini, maxORmini);
                }
                if (PantsPivotKorea.gameObject.activeSelf == true)
                {
                    if (PantsPivotKorea.localScale.x < maxSize.x)   //확대 크기 제어하기
                        PantsPivotKorea.localScale += new Vector3(maxORmini, maxORmini, maxORmini);
                }

            }
            else if(initTouchDist > lastTouchDIst)
            {
                //축소시키기
                if (BerninStuff.gameObject.activeSelf == true)
                {
                    if (BerninStuff.localScale.x > minSize.x)   //축소 크기 제어하기
                        BerninStuff.localScale -= new Vector3(maxORmini, maxORmini, maxORmini);
                }
                if (PantsPivotTunisiaPants.gameObject.activeSelf == true && GameManager.isCanMove == true)
                {
                    if (PantsPivotTunisiaPants.localScale.x > 1)   //축소 크기 제어하기
                        PantsPivotTunisiaPants.localScale -= new Vector3(maxORmini, maxORmini, maxORmini);
                }
                if (PantsPivotKorea.gameObject.activeSelf == true)
                {
                    if (PantsPivotKorea.localScale.x > 1)   //축소 크기 제어하기
                        PantsPivotKorea.localScale -= new Vector3(maxORmini, maxORmini, maxORmini);
                }
            }

        }
            initTouchDist = lastTouchDIst;
    }

    /// <summary>
    /// 영국 미션, 단추를 모두 달았을 때
    /// </summary>
    public void EngMission()
    {
        if (isPocketFront == true || isPocketBack == true)
        {
            //if(SoundManager.Instance.clickHandBySound[1].activeSelf == true)
            //{
                SoundManager.Instance.clickHandBySound[1].SetActive(false); //손가락 아이콘이 있으면 꺼라는 것이고.
            //}
        }

        if (isPocketFront == true && isPocketBack == true)
        {
            SoundManager.Instance.PlayAudio(3);
            //영국미션 클리어 한 곳.
            StartCoroutine(EnglandMission());
        }
    }

    IEnumerator EnglandMission()
    {
        //yield return new WaitForSeconds(SoundManager.Instance.audiosource.clip.length); //소리다들어야 미션클리어하게하기
        yield return null;
        GameManager.Instance.isEngland = true;  //영국 미션 클리어
        UIManager.Instance.NaviBlink(UIManager.Instance.NationStageState);  //다음 국가 반짝임
    }

    public void returnSizeNRot()
    {
        BerninStuff.localScale = new Vector3(.5f, .5f, .5f);
        PantsPivotTunisiaPants.localScale = new Vector3(1,1,1);
        PantsPivotTunisiaPants.localRotation = Quaternion.Euler(UIManager.Instance.TunisiaFront, 90, 90);
        PantsPivotKorea.localScale = new Vector3(1, 1, 1);
        PantsPivotKorea.localRotation = Quaternion.Euler(90, 0, 0);
    }

}
