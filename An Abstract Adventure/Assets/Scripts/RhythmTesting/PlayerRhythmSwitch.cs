using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRhythmSwitch : MonoBehaviour
{
    public float switchTime;
    public Collider cubeCollider;
    public Collider sphereCollider;
    public Camera cubeCamera;
    public Camera sphereCamera;

    private PlayerMain playerMain;

    // Start is called before the first frame update
    void Start()
    {
        playerMain = GetComponent<PlayerMain>();
        if (playerMain.activePlayer == ActivePlayer.Cube)
        {
            sphereCollider.enabled = false;
            cubeCollider.enabled = true;
            sphereCamera.enabled = false;
            cubeCamera.enabled = true;
        }
        else
        {
            cubeCollider.enabled = false;
            sphereCollider.enabled = true;
            cubeCamera.enabled = false;
            sphereCamera.enabled = true;
        }
        StartCoroutine(WaitToSwitch());
    }

    IEnumerator WaitToSwitch()
    {
        yield return new WaitForSeconds(switchTime);
        Switch();
    }

    void Switch()
    {
        if (playerMain.activePlayer == ActivePlayer.Cube)
        {
            cubeCollider.enabled = false;
            sphereCollider.enabled = true;
            cubeCamera.enabled = false;
            sphereCamera.enabled = true;
            playerMain.activePlayer = ActivePlayer.Sphere;
            playerMain.cubeDash.StopDashEarly();
            playerMain.playerGroundDetection.SwitchGroundCheck();
        }
        else
        {
            sphereCollider.enabled = false;
            cubeCollider.enabled = true;
            sphereCamera.enabled = false;
            cubeCamera.enabled = true;
            playerMain.activePlayer = ActivePlayer.Cube;
            playerMain.sphereSlam.StopSlamEarly();
            playerMain.playerGroundDetection.SwitchGroundCheck();
        }
        StartCoroutine(WaitToSwitch());
    }
}
