using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectablesUI : MonoBehaviour
{
    public PlayerCollect playerCollect;
    public Text amount;
    public Text amountBackshadow;

    private UISlide uiSlide;

    // Start is called before the first frame update
    void Start()
    {
        amount.text = playerCollect.numCollectables.ToString();
        amountBackshadow.text = playerCollect.numCollectables.ToString();
        uiSlide = GetComponentInParent<UISlide>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCollect.collected)
        {
            amount.text = playerCollect.numCollectables.ToString();
            amountBackshadow.text = playerCollect.numCollectables.ToString();
            playerCollect.collected = false;
            uiSlide.StopAllCoroutines();
            uiSlide.active = true;
        }
    }
}
