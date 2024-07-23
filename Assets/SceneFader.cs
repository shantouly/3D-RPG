using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class SceneFader : MonoBehaviour
{
    CanvasGroup canvasGroup;
    public float FadeOutDuration;
    public float FadeInDuration;    

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator FadeOutIn()
    {
        yield return FadeOut(FadeOutDuration);
        yield return FadeIn(FadeInDuration);
    }

    public IEnumerator FadeOut(float time)
    {
        while(canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }     
    }

    public IEnumerator FadeIn(float time)
    {
        while(canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }      
    }

   
}
