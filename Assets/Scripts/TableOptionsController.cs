using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableOptionsController : MonoBehaviour
{
    public void StartNewGame()
    {

    }
    public void BackToLobby()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnOutsideClick();
        }
    }

    public void OnOutsideClick()
    {
        // Týklama veya dokunma konumu
        Vector2 clickPosition = Input.mousePosition;

        // Yeni panelin RectTransform bileþeni
        RectTransform newPanelRect = GetComponent<RectTransform>();

        // Týklama veya dokunma konumu, yeni panelin alanýnýn dýþýnda mý kontrol edin
        if (!RectTransformUtility.RectangleContainsScreenPoint(newPanelRect, clickPosition))
        {
            gameObject.SetActive(false);
        }
    }
}
