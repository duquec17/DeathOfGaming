using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildInfo : MonoBehaviour
{
    public string message;

    // When building clicked on will display information
    private void OnMouseDown()
    {
        BuildInfoManager._instance.SetAndShowBuildInfo(message);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    // Will hide info when clicked outside of box
    public void DeleteInfo()
    {
        BuildInfoManager._instance.HideBuildInfo();
        GetComponent<BoxCollider2D>().enabled = true;
    }

}
