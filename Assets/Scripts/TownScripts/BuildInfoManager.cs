using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildInfoManager : MonoBehaviour
{
    public static BuildInfoManager _instance;

    public TextMeshProUGUI textComponent;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void SetAndShowBuildInfo(string message)
    {
        gameObject.SetActive(true);
        textComponent.text = message;
    }

    public void HideBuildInfo()
    {
        gameObject.SetActive(false);
        textComponent.text = string.Empty;
    }
}
