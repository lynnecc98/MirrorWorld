
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    

    public static IEnumerator In(GameObject g, float fadePeriod)
    {
        CanvasGroup cg = g.GetComponent<CanvasGroup>();

        float alphaPerSeconds = 1 / fadePeriod;

        while (cg.alpha < 1)
        {
            cg.alpha += alphaPerSeconds * Time.deltaTime;
            yield return null;
        }

        cg.alpha = 1;
    }


    public static IEnumerator Out(GameObject g, float fadePeriod)
    {
        CanvasGroup cg = g.GetComponent<CanvasGroup>();


        float alphaPerSeconds = 1 / fadePeriod;

        while (cg.alpha > 0)
        {
            cg.alpha -= alphaPerSeconds * Time.deltaTime;
            yield return null;
        }

        cg.alpha = 0;
    }
}
