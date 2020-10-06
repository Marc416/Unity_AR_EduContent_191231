using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIMPantsCtrl : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "DECO")
        {
            TIMDecoManager.Get.ShowPantsDeco(coll.name);
            coll.gameObject.SetActive(false);
        }
    }
}
