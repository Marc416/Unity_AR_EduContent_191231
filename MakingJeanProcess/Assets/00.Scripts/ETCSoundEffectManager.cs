using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETCSoundEffectManager : MonoBehaviour
{
    public static ETCSoundEffectManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    [Header("0:")]
    public AudioClip[] AudioSource;

    [Header("넣지말것- 소리 크기 제어하기위함")]
    public AudioSource EffectAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        EffectAudioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 0: UI버튼터치, 1: 청바지,주머니터치, 이태리 바지터치(염색적용이후), 2:비행기 3:목화 아주작게, 튀니지바지 보통사운드크기, 4: 폭죽소리
    /// </summary>
    /// <param name="num"></param>
    public void PlayEffectSound(int num)
    {
        EffectAudioSource.volume = 1;
        EffectAudioSource.Stop();
        EffectAudioSource.clip = AudioSource[num];
        EffectAudioSource.Play();
    }
}
