using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TIMEnt.Util
{
    public class TIMDirectionArrow : MonoBehaviour
    {
        private static TIMDirectionArrow instance;
        public static TIMDirectionArrow Get
        {
            get { return instance; }
        }

        [SerializeField] bool is3D = false;
        [SerializeField] RectTransform TargetObject;
        [SerializeField] GameObject Arrow;

        Vector3 mp = Vector3.zero; // My Position
        Vector3 tp = Vector3.zero; // Target Position
        Vector3 newAngle;
        Vector3 tp_temp;
        void Awake()
        {
            instance = this;
            newAngle = Vector3.zero;
            tp_temp = Vector3.zero;
            newAngle.x = 180;
        }

        void Update()
        {
            if (TargetObject == null)
            {
                Arrow.SetActive(false);
            }
            else
            {
                if (Arrow.activeSelf == false) Arrow.SetActive(true);
                mp = GetScreenPoint(transform.position);
                Vector3[] v = new Vector3[4];
                TargetObject.GetWorldCorners(v);
                tp_temp = (v[0] + v[2]) / 2;

                tp = GetScreenPoint(tp_temp);
                tp.y = Screen.height - tp.y;

                if (is3D)
                {
                    transform.LookAt(TargetObject.transform);
                }
                else
                {
                    LookTarget(TargetObject.position);// GetTargetScreenPosition(TargetObject));
                }
            }
        }

        void LookTarget(Vector3 targetPos)
        {
            newAngle.z = GetDegree(mp, tp);
            transform.localEulerAngles = newAngle;
        }

        Vector3 GetScreenPoint(Vector3 p)
        {
            return Camera.main.WorldToScreenPoint(p);
        }

        public void SetTargetObject(RectTransform t)
        {
            TargetObject = t;
        }

        float GetDegree(Vector3 m, Vector3 t)
        {
            float ang = Mathf.Atan2(t.y - m.y, Mathf.Abs(t.x) - m.x) * 180 / Mathf.PI;
            if (ang < 0) ang += 360;

            return ang;
        }

        //#if UNITY_EDITOR
        //        void OnGUI()
        //        {
        //            GUI.Box(new Rect(0, 0, 100, 100), "0.0");
        //            GUI.Box(new Rect(Screen.width - 50, Screen.height - 50, 100, 100), "end");
        //            GUI.Box(new Rect(tp.x, tp.y, 100, 100), tp.ToString());
        //        }
        //    }
        //#endif
    }
}

