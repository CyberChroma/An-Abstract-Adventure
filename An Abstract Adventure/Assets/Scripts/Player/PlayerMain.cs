using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    private CircleMain circleMain;
    private SquareMain squareMain;

    // Start is called before the first frame update
    void Start()
    {
        circleMain = GetComponent<CircleMain>();
        squareMain = GetComponent<SquareMain>();
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
