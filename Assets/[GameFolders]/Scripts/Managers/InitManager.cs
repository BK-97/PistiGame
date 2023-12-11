using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitManager : Singleton<InitManager>
{
    public Canvas gamePlayCanvas;
    void Start()
    {
        StartCoroutine(WaitCO());
    }
    IEnumerator WaitCO()
    {
        yield return new WaitForSeconds(0.1f);
        gamePlayCanvas.gameObject.SetActive(true);
    }
}
