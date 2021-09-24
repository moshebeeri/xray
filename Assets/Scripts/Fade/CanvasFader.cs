using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasFader : MonoBehaviour
{
    FadeUtils fade = new FadeUtils();
    [SerializeField]
    public Canvas canvas;
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
        fade.Interpolate(duration, 1, 0, precentage => { canvas.GetComponent<CanvasGroup>().alpha = precentage; });
        yield return new WaitForSeconds(waitBetween);
        fade.Interpolate(duration, 0, 1, precentage => { canvas.GetComponent<CanvasGroup>().alpha = precentage; });
    }
}
