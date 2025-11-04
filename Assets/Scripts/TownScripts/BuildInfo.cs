using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildInfo : MonoBehaviour
{
    public string message;

    private void OnMouseDown()
    {
        BuildInfoManager._instance.SetAndShowBuildInfo(message);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void DeleteInfo()
    {
        BuildInfoManager._instance.HideBuildInfo();
        GetComponent<BoxCollider2D>().enabled = true;
    }

}
