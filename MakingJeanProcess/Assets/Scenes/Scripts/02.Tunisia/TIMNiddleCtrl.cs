using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIMNiddleCtrl : MonoBehaviour
{
    public int num;
    public Animator[] animator;   //Part아래에 있는 NeedleNew
    public GameObject[] needle;   //위의 NeedleNew와 같음
    public GameObject[] thread;   //Part아래에 있는 BO_Sin1_Thread02
    public SphereCollider[] part;   //콜라이더 제어용

    [Header("앞뒤버튼")]
    public GameObject[] FrontBackButtons;

    int test = 0;
    int needlePart = 0;
    public delegate void NiddleDel();
    public static NiddleDel NiddleSewing;
    public static bool isSewing;  //바느질 중일때는 다음 바느질 못하게 막기.

    private void OnEnable()
    {
        NiddleSewing += Sewing;
        GameManager.TunuisiaReset += ResetTunuisia;
    }

    private void OnDisable()
    {
        NiddleSewing -= Sewing;
        GameManager.TunuisiaReset -= ResetTunuisia;
    }

    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 0; i<needle.Length; i++)
        {
            needle[i].SetActive(false);
            thread[i].SetActive(false);
            part[i].enabled = false;
        }

    }
    private void Start()
    {
        //소리 나오고 바느질 하는건가..?
        part[0].enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        //테스트코드
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //PlayUp(1);
            //PlayDown(1);

            //StartCoroutine(startSewing());
        }
    }

    /// <summary>
    /// 오른쪽에서부터 왼쪽 까지 순서대로 바느질!
    /// </summary>
    public void Sewing()
    {
        if(isSewing == false)   //바느질 중에는 한번더 터치 못하게 막기
        {
            isSewing = true;
            ETCSoundEffectManager.Instance.PlayEffectSound(3);

            //if(needlePart ==4)
            //    UIManager.Instance.TunBackButton();
            part[needlePart].enabled = false;
            if (needlePart < part.Length)
            {
                PlayUp(needlePart);
                PlayDown(needlePart);
                //디피된 장식물들 움직이게 하기
                GameManager.Instance.PantsDecoTunisia(needlePart);
                needlePart++;
                if(needlePart<part.Length)
                    part[needlePart].enabled = true;
            }

        }
    }

    //터치 카운터에 따라 니들의 수가
    public void PlayUp(int needlenum)
    {
        //바느질 인디케이터를 끄는 구간 from 0번
        needle[needlenum].SetActive(true);
        animator[needlenum].speed = 1;
    }
    public void PlayDown(int needlnum)
    {
        animator[needlnum].SetBool("isDown", true);
        GameManager.Instance.AnimCtrl(animator[needlnum], isSewing, needlnum);   //애니메이션 끝나야  isSewingfalse
        if(needlnum == needle.Length - 1)
        {
            StartCoroutine(TunuisiaMission());
        }
    }

    public void ResetTunuisia()
    {
        //스탑코루틴자리
        //StopCoroutine(GameManager.Instance.BlinkingIndicator);
        if (GameManager.Instance.BlinkingIndicator != null) StopCoroutine(GameManager.Instance.BlinkingIndicator);
        GameManager.isCanMove = false;
        for(int i = 0;  i<2; i++)
        {
            FrontBackButtons[i].SetActive(false);   //앞뒤버튼 없애기
        }

        for (int reset = 0; reset < part.Length; reset++)
        {
            needle[reset].SetActive(false);
            animator[reset].enabled = true;
            animator[reset].speed = 0;
            //손가락 다끈다음
            GameManager.Instance.Indicator[reset].SetActive(false);
        }
        GameManager.Instance.InitIndicator();
        isSewing = false;
        needlePart = 0;
        part[0].enabled = true;
        //손가락 0번째꺼 실행하도록 하기.
    }


    /// <summary>
    /// 투니지아 미션 성공
    /// </summary>
    /// <returns></returns>
    IEnumerator TunuisiaMission()
    {
        for (int i = 0; i < 2; i++)
        {
            FrontBackButtons[i].SetActive(true);    //앞뒤버튼 생기기
        }
        yield return new WaitForSeconds(1.5f);
        SoundManager.Instance.PlayAudio(3);
        //yield return new WaitForSeconds(SoundManager.Instance.audiosource.clip.length);
        GameManager.Instance.isTunusia = true;      //미션 석세스 한 구간
        
        //바지 못움직이게하는 불값자리
        GameManager.isCanMove = true;

        UIManager.Instance.NaviBlink(UIManager.Instance.NationStageState);  //다음 국가 반짝임
    }

}
