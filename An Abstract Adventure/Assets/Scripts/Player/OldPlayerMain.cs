using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPlayerMain : MonoBehaviour
{
    private OldCircleMain circleMain;
    private OldSquareMain squareMain;

    // Start is called before the first frame update
    void Start()
    {
        circleMain = GetComponent<OldCircleMain>();
        squareMain = GetComponent<OldSquareMain>();
    }

    void OnEnable()
    {
        if (circleMain)
        {
            circleMain.enabled = true;
        }
        else if (squareMain)
        {
            squareMain.enabled = true;
        }
    }

    void OnDisable()
    {
        if (circleMain)
        {
            circleMain.enabled = false;
        }
        else if (squareMain)
        {
            squareMain.enabled = false;
        }
    }
}
