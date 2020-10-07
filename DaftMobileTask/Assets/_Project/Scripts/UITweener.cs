﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class UITweener
{
    public static IEnumerator FadeImage(Image img, float startAlpha, float endAlpha, float duration)
    {
        Color currentColor = img.color;
        currentColor.a = startAlpha;
        Color targetColor = img.color;
        targetColor.a = endAlpha;
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            img.color = Color.Lerp(currentColor, targetColor, timer / duration);
            yield return null;
        }
    }
}
