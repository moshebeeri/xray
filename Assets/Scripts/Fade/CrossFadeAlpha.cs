using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossFadeAlpha : MonoBehaviour
{
    public float duration = 1.5f;
    public float waitBefore = 1.0f;
    public bool autoFade = true;

    // Start is called before the first frame update
    void Start()
    {
        if(autoFade)
            StartCoroutine(Fader());
    }

    public IEnumerator Fader()
    {
        yield return new WaitForSeconds(waitBefore);
		Image alphaChannel = GetComponent<Image> ();
		alphaChannel.CrossFadeAlpha (0, duration, true);
		Destroy (this.gameObject, 0);
    }
}
