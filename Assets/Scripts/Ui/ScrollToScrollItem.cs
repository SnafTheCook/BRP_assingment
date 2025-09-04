using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollToScrollItem : MonoBehaviour, ISelectHandler
{
    public void OnSelect(BaseEventData eventData)
    {
        ScrollToMe();
    }

    private void ScrollToMe()
    {
        ScrollRect scrollRect = GetComponentInParent<ScrollRect>();
        if (scrollRect == null) return;

        RectTransform content = scrollRect.content;
        RectTransform viewport = scrollRect.viewport;
        RectTransform thisRect = GetComponent<RectTransform>();

        Vector3[] itemCorners = new Vector3[4];
        thisRect.GetWorldCorners(itemCorners);

        Vector3[] viewportCorners = new Vector3[4];
        viewport.GetWorldCorners(viewportCorners);

        float itemTop = itemCorners[1].y;
        float itemBottom = itemCorners[0].y;

        float viewportTop = viewportCorners[1].y;
        float viewportBottom = viewportCorners[0].y;

        float contentHeight = content.rect.height;
        float viewportHeight = viewport.rect.height;

        float offset = 0f;

        if (itemTop > viewportTop)
            offset = itemTop - viewportTop;
        else if (itemBottom < viewportBottom)
            offset = itemBottom - viewportBottom;
        else
            return;

        float normalizedOffset = offset / (contentHeight - viewportHeight);
        float targetPos = scrollRect.verticalNormalizedPosition + normalizedOffset;

        targetPos = Mathf.Clamp01(targetPos);

        StopAllCoroutines();
        StartCoroutine(SmoothScroll(scrollRect, targetPos));
    }

    private IEnumerator SmoothScroll(ScrollRect scrollRect, float targetPos)
    {
        float duration = 0.15f;
        float time = 0f;
        float startPos = scrollRect.verticalNormalizedPosition;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / duration;
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(startPos, targetPos, t);
            yield return null;
        }

        scrollRect.verticalNormalizedPosition = targetPos;
    }
}
