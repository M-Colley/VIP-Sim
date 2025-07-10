using Christina.UI;
using UnityEngine;
using uWindowCapture;

public class HideMenu : MonoBehaviour
{

    // Serielles Field für das GameObject, das gesetzt werden soll
    [SerializeField] private GameObject targetGameObject;
    [SerializeField] private ToggleSwitch toggle;

    void Update()
    {
        // Setzt das target GameObject je nach Rückgabewert
        if (UwcWindowList.thereIsActiveWindow)
        {
            targetGameObject.SetActive(true);
        }
        else
        {
            targetGameObject.SetActive(false);
            if(toggle.CurrentValue)
            toggle.Toggle();

        }    
    }
}
