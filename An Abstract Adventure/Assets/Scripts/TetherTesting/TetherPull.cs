using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherPull : MonoBehaviour
{
    public float pullSlowDistance;
    public float pullFastDistance;
    public float pullSlowSpeed;
    public float pullFastSpeed;
    public float pullSlowSmoothing;
    public float pullFastSmoothing;
    public float pullStopSmoothing;
    public Rigidbody cubeRb;
    public Rigidbody sphereRb;

    private Vector3 cubeForceToApply = Vector3.zero;
    private Vector3 sphereForceToApply = Vector3.zero;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 dirSToC = cubeRb.position - sphereRb.position;
        dirSToC.z = 0;
        if (dirSToC.magnitude > pullFastDistance)
        {
            cubeForceToApply = Vector3.Lerp(cubeForceToApply, -dirSToC * pullFastSpeed, pullFastSmoothing);
            sphereForceToApply = Vector3.Lerp(sphereForceToApply, dirSToC * pullFastSpeed, pullFastSmoothing);
        }
        else if (dirSToC.magnitude > pullSlowDistance)
        {
            cubeForceToApply = Vector3.Lerp(cubeForceToApply, -dirSToC * pullSlowSpeed, pullSlowSmoothing);
            sphereForceToApply = Vector3.Lerp(sphereForceToApply, dirSToC * pullSlowSpeed, pullSlowSmoothing);
        }
        else
        {
            cubeForceToApply = Vector3.Lerp(cubeForceToApply, Vector3.zero, pullStopSmoothing);
            sphereForceToApply = Vector3.Lerp(sphereForceToApply, Vector3.zero, pullStopSmoothing);
        }
        cubeRb.AddForce(cubeForceToApply);
        sphereRb.AddForce(sphereForceToApply);
    }
}
