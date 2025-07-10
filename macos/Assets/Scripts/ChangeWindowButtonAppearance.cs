using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeWindowButtonAppearance : MonoBehaviour
{
    [SerializeField] private Sprite spriteOff; // First sprite
    [SerializeField] private Sprite spriteOn; // Second sprite
    [SerializeField] private TextMeshProUGUI buttonText; // Reference to the button's text
    [SerializeField] private Color colorOff = Color.white; // First color
    [SerializeField] private Color colorOn = Color.black; // Second color

    
    private Image buttonImage;
    public bool offSpriteActive = true;

    void Start()
    {
        // Get the Image component of the button
        buttonImage = GetComponent<Image>();
    }
    
    public void SwapSpritesAndTextColor()
    {
        // Find all buttons with the "Settings" tag in the scene
        GameObject[] settingsButtons = GameObject.FindGameObjectsWithTag("WindowCapture");

        foreach (GameObject buttonObj in settingsButtons)
        {
            if(buttonObj != this.gameObject)
            buttonObj.GetComponent<ChangeWindowButtonAppearance>().turnOff();
        }
        PerformSwap();
    }

    public void turnOff(){
        buttonImage.sprite = spriteOff;
        buttonText.color = colorOff;
        offSpriteActive = true;
    }

    private void PerformSwap()
    {
        if (offSpriteActive)
        {
            buttonImage.sprite = spriteOn;
            if(buttonText != null)  
            buttonText.color = colorOn;
        }
        else
        {
            buttonImage.sprite = spriteOff;
            if(buttonText != null)  
            buttonText.color = colorOff;
        }

        // Toggle the state
        offSpriteActive = !offSpriteActive;
    }
}
