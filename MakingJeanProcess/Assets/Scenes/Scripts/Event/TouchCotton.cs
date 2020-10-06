using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchCotton : MonoBehaviour
{
    public Image sytheImg;  //낫이미지로 교체

    [Header("Basket 오브젝트 안의 Cotton-껐다켰다할거임")]
    public GameObject[] BasketCtnObj;
    public GameObject[] GardenCtnObj;   //밭에 있는 목화들

    public static int cottonCount;    //20개 모두 수확 한지 안한지 쳌용

    private void OnEnable()
    {
        GameManager.BerninReset += ResetBernin;
    }

    private void OnDisable()
    {
        GameManager.BerninReset -= ResetBernin;
        
    }

    private void Start()
    {
        cottonCount = 0; // 초기화
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            //수확 후 낫을 계속 둘지 말지 에 따라서 기능변경하기
            if (cottonCount < 20)
            {
                sytheImg.gameObject.SetActive(true);
            }
            else
            {
                sytheImg.gameObject.SetActive(false);
            }
            Touch touch = Input.GetTouch(0);    //터치값 읽어옴

            Vector3 ScreenPos = new Vector3(touch.position.x, touch.position.y, 100);   //스크린터치셋팅
            Vector3 TouchPos = Camera.main.ScreenToWorldPoint(ScreenPos);               //3d터치로 바꿈
            Ray ray = Camera.main.ScreenPointToRay(ScreenPos);                          //레이터치로 바꿈
            RaycastHit hit;

            sytheImg.transform.position = ScreenPos;
            if (Physics.Raycast(ray, out hit, 50000))
            {
                if (hit.collider.tag == "Cotton")
                {
                    hit.collider.transform.Translate(Vector3.down * 10f * Time.deltaTime, Space.Self);
                    hit.collider.gameObject.GetComponent<MeshRenderer>().material.color =
                        new Color(hit.collider.gameObject.GetComponent<MeshRenderer>().material.color.r,
                        hit.collider.gameObject.GetComponent<MeshRenderer>().material.color.g,
                        hit.collider.gameObject.GetComponent<MeshRenderer>().material.color.b, 0.2f);
                    //hit.collider.tag = ""; // 태그 초기화
                    hit.collider.GetComponent<BoxCollider>().enabled = false;

                    //Destroy(hit.collider.gameObject, 0.25f); // 목화 삭제
                    //목화를 바구니에 이동 시키기(목화의 Pivot이 코드로 이동시키기 적절치 않아서 복사한 오브젝트들 껐다 켰다를 해야 함.)
                    //이동하려면 여기서 메소드 다시 짜기
                    hit.collider.gameObject.SetActive(false);
                    BasketCtnObj[cottonCount].SetActive(true);

                    ETCSoundEffectManager.Instance.PlayEffectSound(3);
                    ETCSoundEffectManager.Instance.EffectAudioSource.volume = .2f;

                    if(cottonCount == 19)
                    {
                        //미션클리어
                            BerninMission();
                    }
                    cottonCount++;
                }
            }
        }
    }

    public void BerninMission()
    {
        //베넹 미션 클리어
        SoundManager.Instance.PlayAudio(3);
        GameManager.Instance.isBernin = true;
        UIManager.Instance.NaviBlink(UIManager.Instance.NationStageState);  //다음 국가 반짝임
    }

    public void ResetBernin()
    {
        cottonCount = 0;
        for (int i = 0; i <GardenCtnObj.Length; i++)
        {
            GardenCtnObj[i].SetActive(true);
            GardenCtnObj[i].GetComponent<BoxCollider>().enabled = true;
            BasketCtnObj[i].SetActive(false);
        }
    }
}
