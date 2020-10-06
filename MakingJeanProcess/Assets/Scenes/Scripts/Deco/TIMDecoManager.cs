using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIMDecoManager : MonoBehaviour
{
    private static TIMDecoManager instance;
    public static TIMDecoManager Get
    {
        get { return instance; }
    }
    public Transform Pants;
    public Transform CopyPos;
    public Transform Canvas_Sub;

    public List<Transform> decoList_Pants;
    public List<Transform> decoList_UI;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        CheckDragObject();
        
    }
    bool isDrag = false;
    TIMDecoCtrl seletedObject;
    Vector3 lastTouch;
    void CheckDragObject()
    {

    }

    void DragObject(RaycastHit hit)
    {
        Vector3 dir = hit.point - hit.collider.transform.position;
        dir.y = 0;

        float speed = 0.1f;
        if (dir.magnitude > speed)
        {
            dir.Normalize();
            hit.collider.transform.Translate(dir * speed);
        }
    }

    public void ShowPantsDeco(string name)
    {
        for (int i = 0; i < decoList_Pants.Count; i++)
        {
            if(name == decoList_Pants[i].name)
            {
                decoList_Pants[i].gameObject.SetActive(true);
                continue;
            }
        }
    }

    public void ShowFront()
    {
        Pants.localRotation = Quaternion.Euler(-90, 0, 0);
    }

    public void ShowBack()
    {
        Pants.localRotation = Quaternion.Euler(-90, 180, 0);
    }

    public void Submit()
    {
        ShowFront();
        Transform o = Instantiate(Pants, CopyPos.position, Quaternion.identity);
        o.parent = Pants.parent;
        o.localRotation = Quaternion.Euler(-90, 180, 0);
        o.localScale = Pants.localScale;
        //Vector3 p = Pants.position;
        //o.position = new Vector3(p.x - 1, p.y, p.z);
        Canvas_Sub.gameObject.SetActive(false);
    }
}
