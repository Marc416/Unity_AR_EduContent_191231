using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDyeWater : MonoBehaviour
{
    public GameObject incompletion;
    public Transform dryingRackMidPoint;

    public static bool dryingRack;

    //by준희, 1127, 내부변수로 있던 변수를 외부변수로 변경하였습니다.
    Vector3 touchPosToVector3;
    Vector3 touchPos;
    Ray ray;
    RaycastHit hit;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            // 싱글 터치.
            Touch touch = Input.GetTouch(0);
      
            touchPosToVector3 = new Vector3(touch.position.x, touch.position.y, 3.03f);
            touchPos = Camera.main.ScreenToWorldPoint(touchPosToVector3);
            ray = Camera.main.ScreenPointToRay(touchPosToVector3);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // 터치 시작 시.
                    Debug.Log("터치 시작");

                    if (Physics.Raycast(ray, out hit, 500))
                    {
                        Debug.DrawLine(ray.origin, hit.point, Color.red, 1.5f);
                        if (hit.collider.tag == "DyeWater")
                        {
                            Debug.Log("바지를 잡았다.");
                            incompletion.transform.position = touchPos;
                        }
                    }
                    break;
                case TouchPhase.Moved:
                    // 터치 이동 시.
                    Debug.Log("터치 이동");
                    if (Physics.Raycast(ray, out hit, 500))
                    {
                        Debug.DrawLine(ray.origin, hit.point, Color.red, 1.5f);

                        if (hit.collider.tag == "DyeWater")
                        {
                            Debug.Log("드래그로 바지를 잡았다.");
                            incompletion.transform.position = touchPos;
                        }
                    }

                    break;

                case TouchPhase.Stationary:
                    // 터치 고정 시.
                    // Debug.Log("터치 고정");
                    break;

                case TouchPhase.Ended:
                    // 터치 종료 시. ( 손을 뗐을 시 )
                    if (dryingRack)
                    {
                        AttachDryingRack();
                    }

                     Debug.Log("터치 종료");
                    break;

                case TouchPhase.Canceled:
                    // 터치 취소 시. ( 시스템에 의해서 터치가 취소된 경우 (ex: 전화가 왔을 경우 등) )
                    // Debug.Log("터치 취소");
                    break;
            }
        }
    }

    //by준, 건조대 중앙으로 올 수 있게 하는 메소드,
    //건조대 근처에서 드래그한손을 떼면 건조대의 중앙으로 위치할 수 있도록 함.
    void AttachDryingRack()
    {
        incompletion.transform.position = dryingRackMidPoint.position + new Vector3(0,0,0.156f);
    }

}