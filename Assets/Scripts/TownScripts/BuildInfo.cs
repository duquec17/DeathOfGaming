using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildInfo : MonoBehaviour
{
    public string message;

    private void OnMouseEnter()
    {
        BuildInfoManager._instance.SetAndShowBuildInfo(message);
    }

    private void OnMouseExit()
    {
        BuildInfoManager._instance.HideBuildInfo();
    }
}
