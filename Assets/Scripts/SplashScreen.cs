using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

    public GameObject studioLogo;
    CanvasGroup studioAlpha;
	// Use this for initialization
	IEnumerator Start () {
        studioAlpha = studioLogo.GetComponent<CanvasGroup>();
        yield return FadeIn(1.5f);
        yield return new WaitForSeconds(1.5f);
        yield return FadeOut(1.5f);
        SceneManager.LoadScene("Menu");
    }

    IEnumerator FadeIn(float duration)
    {

        for (float t = 0f; t < 1; t += Time.deltaTime / duration)
        {
            studioAlpha.alpha = Mathf.Lerp(studioAlpha.alpha, 1, t);
            yield return null;
        }
    }
    IEnumerator FadeOut(float duration)
    {
            for (float t = 0f; t < 1; t += Time.deltaTime / duration)
            {
            studioAlpha.alpha = Mathf.Lerp(studioAlpha.alpha, 0, t);
            yield return null;
            }
    }
}
