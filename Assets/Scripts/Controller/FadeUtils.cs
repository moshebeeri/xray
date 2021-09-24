// https://stackoverflow.com/questions/45032579/editing-a-cubemap-skybox-from-remote-image
// https://forum.unity.com/threads/how-to-set-a-skybox-from-an-image-url.420476/using System;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class FadeUtils
{

    public IEnumerator Interpolate(float targetTime, float startValue, float endValue, UnityAction<float> action)
    {
        float lerpTime = 0.0f;
        while(lerpTime < targetTime)
        {
            lerpTime += Time.deltaTime;
            float percentage = lerpTime / targetTime;
            float finalValue = Mathf.Lerp(startValue, endValue, percentage);
            if(action != null)
                action.Invoke(finalValue);
            yield return null;
        }
    }
}
