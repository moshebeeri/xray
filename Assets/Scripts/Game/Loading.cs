using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    public Vector3 startPos;
    public float rotateSpeed = 32f;

    // Update is called once per frame
    void Update()
    {
        float r = rotateSpeed * Time.deltaTime;
        transform.Rotate(r, r, r);
    }
}
