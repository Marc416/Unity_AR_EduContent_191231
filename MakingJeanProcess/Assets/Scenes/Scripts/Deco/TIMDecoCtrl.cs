using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIMDecoCtrl : MonoBehaviour
{
    public float dis;
    bool isMove = false;
    Vector3 lastTouchPos;
    Transform target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isMove)
        {
            float step = 1.5f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }
    }

    public void MoveTo(Transform t)
    {
        target = t;
        isMove = true;
    }

    private void OnDisable()
    {
        isMove = false;
    }

    Vector3 mousePosition;
    void OnMouseDrag()
    {
        return;

        print("Drag!!");
        mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dis);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = objPosition;
        /*
        print("OnMouseDrag : " + ableDrag);
        Vector3 delta = Vector3.zero;
        Vector3 currentTouchPos = GetMouseRayPosition();
        if (Vector3.Distance(lastTouchPos, currentTouchPos) > 0)
        {
            delta = currentTouchPos - lastTouchPos;
            lastTouchPos = currentTouchPos;
        }
        else lastTouchPos = currentTouchPos;
        CheckPosition(delta);*/
    }

    private void OnMouseDown()
    {
        print("OnMouseDown : " + this.name);
        //TIMFieldManager.Get.SelectedObject = this.transform;
        //TIMUIManager.Get.ShowEditUI(true);
    }

    private void OnMouseUp()
    {
    }

    public void CheckPosition(Vector3 delta)
    {
        print("CheckPosition : " + delta.ToString());
        Vector3 pos = delta;
        //pos.y = 10;
        this.transform.position = Vector3.Lerp(this.transform.position, pos, 1.0f);

        Ray ray = new Ray(pos, - transform.up);//Camera.main.po(this.transform.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))//, 1 << Constants.LAYERMASK_FIELD))
        {
            Vector3 v = hit.transform.position + delta;
            this.transform.position = Vector3.Lerp(this.transform.position, v, 1.0f);
        }
    }
    Vector3 GetMouseRayPosition()
    {
        Vector3 result = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))//, 1 << Constants.LAYERMASK_FIELD))
        {
            result = hit.transform.position;
        }
        return result;
    }
}
