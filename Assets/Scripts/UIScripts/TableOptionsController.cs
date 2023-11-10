using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableOptionsController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnOutsideClick();
        }
    }

    public void OnOutsideClick()
    {
        Vector2 clickPosition = Input.mousePosition;

        RectTransform newPanelRect = GetComponent<RectTransform>();

        if (!RectTransformUtility.RectangleContainsScreenPoint(newPanelRect, clickPosition))
        {
            gameObject.SetActive(false);
        }
    }
}
