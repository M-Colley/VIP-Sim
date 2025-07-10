using UnityEngine;
using UnityEngine.UI;
using uWindowCapture;

public class HideImpairmentSelection : MonoBehaviour
{
    // Serielles Field für das GameObject, das gesetzt werden soll
    [SerializeField] private GameObject targetGameObject;
    [SerializeField] Slider enableToggle;
    [SerializeField] Image settingWheel;

    // Update is called once per frame
    void Update()
    {
        // Setzt das target GameObject je nach Rückgabewert
        if (UwcWindowList.thereIsActiveWindow)
        {
            if (enableToggle.value > 0.9)
            {
                targetGameObject.SetActive(true);
            } else
            {
                targetGameObject.SetActive(false);
            }
            if(settingWheel != null)
            settingWheel.enabled = true;
        }
        else
        {
            targetGameObject.SetActive(false);
            if(settingWheel != null)
            settingWheel.enabled = false;    
        }
    }
}
