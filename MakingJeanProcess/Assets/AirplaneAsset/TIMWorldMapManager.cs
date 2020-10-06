using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ted 20191223
/// </summary>
public class TIMWorldMapManager : MonoBehaviour
{
    public int routeIndex;
    [Header("비행기")]
    [SerializeField] TIMAirplaneCtrl airplane;
    [Header("Arrow Grid")]
    [SerializeField] TIMArrowGridCtrl arrowGrid;

    [SerializeField] RectTransform Image_WorldMap;

    [Header("마커")]
    [SerializeField] RectTransform Pos_England;
    [SerializeField] RectTransform Pos_Italy;
    [SerializeField] RectTransform Pos_Benin;
    [SerializeField] RectTransform Pos_Tunisi;
    [SerializeField] RectTransform Pos_Korea;

    [Header("사진")]
    [SerializeField] List<GameObject> pic_small = new List<GameObject>();
    [SerializeField] List<GameObject> pic_large = new List<GameObject>();
    [SerializeField] GameObject Pic_Large_bg;

    [Header("국가플래그")]
    public GameObject[] Nation_Flag;

    //델리게이트
    public delegate void PlaneDel (int nation);
    public static PlaneDel MovePlaneInMap;
    /// <summary>
    /// from 인자로 0을 넣으면됌. 처음 부터 시작한다는 뜻
    /// </summary>
    public static PlaneDel MoveWorldArround;



    Coroutine PlaneMove;
    Coroutine Epilogue;         //에필로그 코루틴 비행기 이동


    List<TripRoute> routes = new List<TripRoute>();

    int currentPicIndex;

    private void OnEnable()
    {
        MovePlaneInMap += MoveAirplane;
        MoveWorldArround += EpilogueMoveAirPlane;
    }

    private void OnDisable()
    {
        MovePlaneInMap -= MoveAirplane;
        MoveWorldArround -= EpilogueMoveAirPlane;
    }

    void Start()
    {
        InitData();
        airplane.gameObject.SetActive(false);
        //StartCoroutine(PlayRoute(0));
    }

    void InitData()
    {
        //두번째 파라미터는 지도 맵이미지의 recttransform, 세번째 파라미터는 스케일
        routes.Add(new TripRoute(false, Pos_England, Pos_England,   new Vector2(331, -141),   new Vector2(1.4f, 1.4f),  0));
        routes.Add(new TripRoute(true, Pos_England, Pos_Benin,      new Vector2(331, -141), new Vector2(1.4f, 1.4f),    1));
        routes.Add(new TripRoute(true, Pos_Benin, Pos_Italy,        new Vector2(331, -141),  new Vector2(1.4f, 1.4f),      2));
        routes.Add(new TripRoute(true, Pos_Italy, Pos_Tunisi,       new Vector2(331, -141),  new Vector2(1.4f, 1.4f),     3));
        routes.Add(new TripRoute(true, Pos_Tunisi, Pos_Korea,       new Vector2(331, -141),  new Vector2(1.4f, 1.4f),     4));
    }

    public void MoveAirplane(int index)
    {
        airplane.gameObject.SetActive(true);
        Pic_Large_bg.SetActive(false);
        for (int i = 0; i <5; i++)
        {
            //초기화
            Nation_Flag[i].SetActive(false);
            pic_large[i].SetActive(false);
        }
        //if (index > 0)
        //{
        //    //선택부분 키기
        //    Nation_Flag[index - 1].SetActive(true);
        //    Nation_Flag[index].SetActive(true);
        //}
        //else if( index == 0)
        //{
        //    Nation_Flag[index].SetActive(true);
        //}

        for(int showNation = 0; showNation<index+1; showNation++)
        {
            Nation_Flag[showNation].SetActive(true);
        }


        if (PlaneMove != null) StopCoroutine(PlaneMove);
        PlaneMove = StartCoroutine(PlayRoute(index));

    }

    public void EpilogueMoveAirPlane(int from)  //굳이 인자를 받을 필요는 없지만 위의 델리게이트를 재사용 하기 위함.
    {
        if (Epilogue != null) StopCoroutine(Epilogue);
        Epilogue = StartCoroutine(EpilogueEnum());
    }

    IEnumerator EpilogueEnum()
    {
        for(int nationNum=0; nationNum < 5; nationNum++)
        {
            //yield return StartCoroutine(PlayRoute(nationNum));
            MoveAirplane(nationNum);
            yield return new WaitForSeconds(3f);
            if (nationNum == 4 )
            {
                TIMCommonAssetCtrl.Close();
            }


        }
    }

 

    IEnumerator PlayRoute(int index)
    {
        yield return null;
        TripRoute data = routes[index];

        Image_WorldMap.anchoredPosition = data.mapPosition;
        Image_WorldMap.localScale = data.mapScale;

        if (data.needMove == false)
        {
            arrowGrid.gameObject.SetActive(false);
            airplane.gameObject.SetActive(false);
        }
        else
        {
            arrowGrid.gameObject.SetActive(true);
            airplane.gameObject.SetActive(true);
            Vector3 s_pos = data.startPos;
            Vector3 e_pos = data.endPos;
            arrowGrid.SetData(s_pos, e_pos);
            airplane.MovePlane(s_pos, e_pos);
        }
        currentPicIndex = data.Image_Index;
        for (int i = 0; i < pic_small.Count; i++)
        {
            if(i == currentPicIndex)
            {
                pic_small[i].SetActive(true);
            }
            else
                pic_small[i].SetActive(false);
        }
    }

    public void OnClick_ShowPicture()
    {
        //확대사진을 클릭했을 때만 클릭사운드
        if(Pic_Large_bg.activeSelf == false)
        {
            ETCSoundEffectManager.Instance.PlayEffectSound(0);
        }

        Pic_Large_bg.SetActive(true);
        pic_large[currentPicIndex].SetActive(true);
        pic_small[currentPicIndex].SetActive(false);
    }

    public void OnClick_ClosePicture()
    {
        ETCSoundEffectManager.Instance.PlayEffectSound(0);
        Pic_Large_bg.SetActive(false);
        pic_large[currentPicIndex].SetActive(false);
        pic_small[currentPicIndex].SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            MoveAirplane(routeIndex);
            routeIndex++;
        }
    }
}

public class TripRoute
{
    public bool needMove;
    public Vector3 startPos;
    public Vector3 endPos;
    public Vector2 mapPosition;
    public Vector2 mapScale;
    public int Image_Index;
    public TripRoute(bool move, RectTransform s_tr, RectTransform e_tr, Vector2 mappos, Vector2 map_s, int  img_s)
    {
        needMove = move;
        startPos = s_tr.anchoredPosition;
        endPos = e_tr.anchoredPosition;
        mapPosition = mappos;
        mapScale = map_s;
        Image_Index = img_s;
    }
}

