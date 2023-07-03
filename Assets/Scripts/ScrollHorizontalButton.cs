using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ScrollHorizontalButton : MonoBehaviour
{
    public RectTransform layout;
    public float animationDuration = 0.5f;
    public float targetOffset = 100f;
    private bool isAnimating = false;
    [SerializeField]
    private int currentTable=1;
    int maxTableCount;
    private void Start()
    {
        maxTableCount = layout.gameObject.transform.childCount-1;
    }
    public void MoveLeft()
    {
        if (isAnimating)
            return;
        if (Mathf.Abs(currentTable) - 1 < 0)
            return;
        float targetPosition = layout.anchoredPosition.x + targetOffset;
        AnimateToPosition(targetPosition);
        currentTable--;
    }

    public void MoveRight()
    {
        if (isAnimating)
            return;
        if (Mathf.Abs(currentTable) + 1>maxTableCount)
            return;
        float targetPosition = layout.anchoredPosition.x - targetOffset;
        AnimateToPosition(targetPosition);
        currentTable++;
    }

    private void AnimateToPosition(float targetPosition)
    {
        isAnimating = true;
        layout.DOAnchorPosX(targetPosition, animationDuration).SetEase(Ease.OutSine).OnComplete(OnAnimationComplete);
    }

    private void OnAnimationComplete()
    {
        isAnimating = false;
    }
}
