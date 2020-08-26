using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackFade : MonoBehaviour
{
    public bool startBlack;
    public float smoothing;

    [HideInInspector] public bool fadeBlack;
    [HideInInspector] public bool fading;

    private Image blackScreen;

    // Start is called before the first frame update
    void Start()
    {
        blackScreen = GetComponent<Image>();
        if (startBlack)
        {
            blackScreen.color = new Color(0, 0, 0, 1);
            fadeBlack = false;
        } else
        {
            blackScreen.color = new Color(0, 0, 0, 0);
            fadeBlack = true;
        }
        fading = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (fading)
        {
            if (fadeBlack)
            {
                blackScreen.color = Color.Lerp(blackScreen.color, new Color(0, 0, 0, 1), smoothing * Time.deltaTime);
                if (blackScreen.color.a >= 0.95f)
                {
                    blackScreen.color = new Color(0, 0, 0, 1);
                    fading = false;
                }
            }
            else
            {
                blackScreen.color = Color.Lerp(blackScreen.color, new Color(0, 0, 0, 0), smoothing * Time.deltaTime);
                if (blackScreen.color.a <= 0.05f)
                {
                    blackScreen.color = new Color(0, 0, 0, 0);
                    fading = false;
                }
            }
        }
    }
}
