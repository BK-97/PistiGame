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
        // T�klama veya dokunma konumu
        Vector2 clickPosition = Input.mousePosition;

        // Yeni panelin RectTransform bile�eni
        RectTransform newPanelRect = GetComponent<RectTransform>();

        // T�klama veya dokunma konumu, yeni panelin alan�n�n d���nda m� kontrol edin
        if (!RectTransformUtility.RectangleContainsScreenPoint(newPanelRect, clickPosition))
        {
            gameObject.SetActive(false);
        }
    }
}
