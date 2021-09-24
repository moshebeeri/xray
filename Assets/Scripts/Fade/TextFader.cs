using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemporalTextFader : MonoBehaviour
{
    FadeUtils fade = new FadeUtils();
    [SerializeField]
    public Text text;
    public float duration = 1.5f;
    public float waitBefore = 1.0f;
    public float waitBetween = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeInAndOut());
    }

    public IEnumerator FadeInAndOut()
    {
        yield return new WaitForSeconds(waitBefore);
        //fade.Interpolate(duration, 1, 0, precentage => { color.a = precentage; });
        text.CrossFadeAlpha(1.0f, duration, false);
        yield return new WaitForSeconds(waitBetween);
        //fade.Interpolate(duration, 0, 1, precentage => { color.a = precentage; });
        text.CrossFadeAlpha(0.0f, duration, false);
    }
}
