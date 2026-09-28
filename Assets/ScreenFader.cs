using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    // Making it static means able to call this script from any other script W/O passing it in by making it instance
    public static ScreenFader Instance;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float fadeDuration = 0.5f;
    [SerializeField] CinemachineVirtualCamera vcam;

    CinemachineFramingTransposer transposer;
    Vector3 originalDamping;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        transposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        originalDamping = new Vector3(transposer.m_XDamping, transposer.m_YDamping, transposer.m_ZDamping);
    }

    async Task Fade(float targetTransparency)
    {
        float start = canvasGroup.alpha, t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, targetTransparency, t / fadeDuration);
            await Task.Yield();
        }

        canvasGroup.alpha = targetTransparency;
    }

    public async Task FadeOut() // When entering buildings
    {
        await Fade(1); // Fade to transparent
        SetDamping(Vector3.zero); // Turn off damping
    }

    public async Task FadeIn() // When leading buildings
    {
        await Fade(0); // Fade to transparent
        SetDamping(originalDamping);
    }

    void SetDamping(Vector3 d)
    {
        if (!transposer) return;
        transposer.m_XDamping = d.x;
        transposer.m_YDamping = d.y;
        transposer.m_ZDamping = d.z;
    }
}
