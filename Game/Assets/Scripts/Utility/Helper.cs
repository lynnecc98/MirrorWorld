using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CS4455.Utility {
    public class Helper {



        public static void DrawRay(Ray ray, float rayLength, bool hitFound, RaycastHit hit, Color rayColor, Color hitColor) {
            
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * rayLength, rayColor); 
                       
            if (hitFound)
            {
                //draw an X that denotes where ray hit
                const float ZBufFix = 0.01f;
                const float edgeSize = 0.2f;
                Color col = hitColor;

                Debug.DrawRay(hit.point + Vector3.up * ZBufFix, Vector3.forward * edgeSize, col);
                Debug.DrawRay(hit.point + Vector3.up * ZBufFix, Vector3.left * edgeSize, col);
                Debug.DrawRay(hit.point + Vector3.up * ZBufFix, Vector3.right * edgeSize, col);
                Debug.DrawRay(hit.point + Vector3.up * ZBufFix, Vector3.back * edgeSize, col);
            }
        }

    }

}