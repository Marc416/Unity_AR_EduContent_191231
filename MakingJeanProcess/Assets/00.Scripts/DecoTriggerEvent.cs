using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoTriggerEvent : MonoBehaviour
{
    public static int DecoNum=0;

    private void Start()
    {
        //DecoNum = GameManager.Instance.isDecoAttached.Length;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "DECO")
        {
            GameManager.Instance.isDecoAttached[DecoNum] = true;    //장식이 더이상 이동 못하게하기
            GameManager.Instance.pantsDecoTr[DecoNum].gameObject.SetActive(true);
            GameManager.Instance.dpDecoTr[DecoNum].gameObject.SetActive(false);
            if (DecoNum == 4)
            {
                UIManager.Instance.TunMuteTunBack();
            }
            DecoNum++;
        }
    }
}
