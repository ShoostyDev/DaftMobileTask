using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueTapButton : TapButton
{
    private Image redFillerImage;
    protected RippleEffect ripples;

    protected override void Awake()
    {
        base.Awake();
        redFillerImage = transform.GetChild(0).GetComponent<Image>();
        ripples = Camera.main.GetComponent<RippleEffect>();
    }

    protected override IEnumerator LifeTimeCounter()
    {
        while (Timer < LifeTime)
        {
            Timer += Time.deltaTime;
            redFillerImage.fillAmount += 1.0f / LifeTime * Time.deltaTime;
            yield return null;
        }

        GameOver();
    }

    protected override void OnButtonTap()
    {
        ripples.Emit(new Vector2(transform.position.x / Screen.width, transform.position.y / Screen.height));
        Deactivate();
    }

    public override void Activate(Vector2 pos, float lifeTime)
    {
        redFillerImage.fillAmount = 0;
        base.Activate(pos, lifeTime);
    }
}
