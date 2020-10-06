using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnColorChange : MonoBehaviour
{
    public GameObject incompletion;

    private Material mDyeWater;

    private Material mIncompletion;
    public MeshRenderer[] changedPantsMaterial;

    string waterColor ="Black";

    bool isColorChanged;


    private void Awake()
    {
        mDyeWater = GetComponent<MeshRenderer>().material;
        mIncompletion = incompletion.GetComponent<MeshRenderer>().material;

        //물색깔 초기화
        mDyeWater.color = Color.black;
    }

    //private void OnEnable()
    //{
    //    TouchDyeWater.ChangeColor += ColorChange;
    //}

    //private void OnDisable()
    //{
    //    TouchDyeWater.ChangeColor -= ColorChange;
    //}


    // 머티리얼 색 설정
    public void SetColor(string color)
    {
        //사운드 끝난다음 실행 하도록 함.
        //if(SoundManager.Instance.audiosource.isPlaying == false)
        //{
            waterColor = color;
        
            //바지를 클릭해 염색해 주세요.
            SoundManager.Instance.PlayAudio(4);

            switch(color)
            {
                case "Red":
                    mDyeWater.color = Color.red + new Color(1, 0, 0, 0.1f)  ; 
                    break;
                case "Yellow":
                    mDyeWater.color = Color.yellow + new Color(0.8f, 0.5f, 0, 0.2f)  ;
                    break;
                case "Green":
                    mDyeWater.color = Color.green + new Color(0, 1, 0, 0.1f) ; 
                    break;
                case "Blue":
                    mDyeWater.color = Color.blue + new Color(0, 0, 1, 0.1f)  ; 
                    break;
                case "Black":
                    mDyeWater.color = Color.black;
                    break;
            }
        //}
    }

    // 머티리얼 색 변경
    public void ColorChange()
    {

        if (waterColor == "Red")
        {
            mIncompletion.color = mDyeWater.color + new Color(-1, -1, -1);
            PantsColorAllChange();
        }
        else if (waterColor == "Yellow")
        {
            mIncompletion.color = mDyeWater.color + new Color(-1.1f, -1, -1);
            PantsColorAllChange();
        }
        else if (waterColor == "Green")
        {
            mIncompletion.color = mDyeWater.color + new Color(-1, -1, -1);
            PantsColorAllChange();
        }
        else if (waterColor == "Blue")
        {
            mIncompletion.color = mDyeWater.color + new Color(-1f, -1f, -1f);
            PantsColorAllChange();
        }
        else if (waterColor == "Black")
        {
            mIncompletion.color = mDyeWater.color + new Color(0.3f, 0.3f, 0.3f);
            PantsColorAllChange();
        }
        // JoonGameManager.isPantsColorChanged = true;       //에러 생겨서 나중에
        //MissionSuccess;
    
    }

    void PantsColorAllChange()
    {
        for(int i = 0; i < changedPantsMaterial.Length; i++)
        {
            changedPantsMaterial[i].material.color = mIncompletion.color;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DyeWater")
        {
            ColorChange();
            ItalyMission(); //미션 완료 메서드
        }
    }

    public void ItalyMission()
    {
        //바지 색상을 바꾼 후
        if(GameManager.Instance.isItaly == false)
        {
            StartCoroutine(ColorChangedSound());
        }
        else if(GameManager.Instance.isItaly == true)
        {
            StartCoroutine(ColorChangedSound());
        }
    }

    IEnumerator ColorChangedSound()
    {
        yield return new WaitForSeconds(2f);  //바지가 한번 담겨진 후 다시 돌아오는 시간(알면 좋겠지만 지금은 예상으로만)
        yield return StartCoroutine(NextNation());
    }

    IEnumerator NextNation()
    {
        //Italy미션완료구간
        SoundManager.Instance.PlayAudio(3); 
        GameManager.Instance.isItaly = true;
        UIManager.Instance.NaviBlink(UIManager.Instance.NationStageState);  //다음 국가 반짝임
        yield return new WaitForSeconds(SoundManager.Instance.audiosource.clip.length);
    }

}
