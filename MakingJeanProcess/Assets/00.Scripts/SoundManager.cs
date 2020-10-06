using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance = null;
    [Header("이거는 추가하지 말것")]
    public AudioSource audiosource;

    public AudioClip[] narationNeffect; //연속적으로 사용할 이펙트 음향

    [Header("0:첫씬, 1:영국")]
    public GameObject[] clickHandBySound;

    bool interrutp;

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
    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    private void Update()
    {
    }
    /// <summary>
    /// 0:인트로, 1:터치해주세요, 3:다음국가터치, 4:염색해주세요. 5~9:국가네비, 10~14:체험하기 클릭시 소리
    /// </summary>
    /// <param name="i"></param>
    public void PlayAudio(int i)        //그냥 플레이하고 끄는거임.
    {
        if (c != null) StopCoroutine(c);
        audiosource.Stop();
        audiosource.clip = narationNeffect[i];
        audiosource.Play();
    }

    /// <summary>
    /// 첫번째 오디오 실행, TrackingTarget스크립트에서 실행 yellowpanel사라지면 실행
    /// </summary>
    public void PlayFirstAudio()
    {
        if (c != null) StopCoroutine(c);
        //PlayAudio(0);   //첫씬 나레이션
        c = StartCoroutine(PlayAudioNext(0,1)); //나래이션 후 클릭 하라는거 듣기
    }

    /// <summary>
    /// 0: intro, 1:터치, 2:경험, 3:네비 다음, 4:색이바낌 염색해주세요, 5:영국, 6:버넹, 7:이태리, 8:투니지아 9:한국
    /// 5~9: 네비게이션 클릭 시나레이션.
    /// 10:
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public void PlayTwoAudio(int a, int b)
    {

        if (c != null) StopCoroutine(c);
        
         c = StartCoroutine(PlayAudioNext(a, b));
    }
    int checkA;
    Coroutine c;
    /// <summary>
    /// a: 네비게이션 눌렀을 때 나오는 국가별 오디오
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    IEnumerator PlayAudioNext(int a, int b)
    {
        int[] indexArray = { a, b};
        switch (a)
            {
                case 0:
                    clickHandBySound[0].SetActive(true);
                    break;
                case 5:
                    if(GameManager.Instance.isEngland == false)
                    {
                        clickHandBySound[1].SetActive(true);
                    }
                    break;
            }
        for (int i = 0; i < indexArray.Length; i++)
        {
            float c_length = narationNeffect[indexArray[i]].length;
            PlayIEnumAudio(indexArray[i]);
            yield return new WaitForSeconds(c_length);
        }

        /*

        yield return StartCoroutine(PlayIEnumAudio(a));
        //앞에 있는 오디오 실행 후 다음 오디오 실행
        yield return new WaitForSeconds(narationNeffect[a].length);
        if(SoundManager.Instance.audiosource.isPlaying == false)
        {
            yield return StartCoroutine(PlayIEnumAudio(b));
            yield return new WaitForSeconds(narationNeffect[b].length);   //오디오가 끝난 후 a가 무엇이냐에 따라 
        
        }
        else
        {
            if(c !=null)StopCoroutine(c);
        }*/
    }

    void  PlayIEnumAudio(int i)
    {
        audiosource.Stop();
        audiosource.clip = narationNeffect[i];
        audiosource.Play();
    }
}
