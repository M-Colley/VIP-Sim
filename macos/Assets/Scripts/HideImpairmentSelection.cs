using UnityEngine;
using UnityEngine.UI;

public class HideImpairmentSelection : MonoBehaviour
{
    // Serielles Field f�r das GameObject, das gesetzt werden soll
    [SerializeField] private GameObject targetGameObject;
    [SerializeField] Slider enableToggle;
    [SerializeField] Image settingWheel;

    [SerializeField] MacCapture macCapture;

    // Update is called once per frame
    void Update()
    {
        // Setzt das target GameObject je nach R�ckgabewert
        if (macCapture.isRunning)
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
