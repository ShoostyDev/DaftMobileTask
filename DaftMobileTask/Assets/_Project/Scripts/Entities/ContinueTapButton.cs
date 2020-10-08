using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueTapButton : TapButton
{
    private Image redFillerImage;

    private void Awake()
    {
        redFillerImage = transform.GetChild(0).GetComponent<Image>();
    }
    protected override void OnButtonTap()
    {
        Deactivate();
    }
}
