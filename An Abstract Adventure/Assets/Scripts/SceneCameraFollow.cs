using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class SceneCameraFollow : MonoBehaviour
{
    SceneCameraFollow ()
    {
        //EditorApplication.update += Update;
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying)
        {
            //transform.localPosition = Vector3.zero;
            //transform.rotation = Quaternion.identity;
        }
        else
        {
            //transform.position = SceneView.lastActiveSceneView.camera.transform.position;
            //transform.rotation = SceneView.lastActiveSceneView.camera.transform.rotation;
        }
    }
}
