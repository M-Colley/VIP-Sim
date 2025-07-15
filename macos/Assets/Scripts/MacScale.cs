using UnityEngine;
using TMPro;
using Kirurobo;

public class MacScale : MonoBehaviour
{
    // The fixed aspect ratio for the plane (16:9)
    private float aspectRatio = 16f / 10f;
        // Menu bar height (in pixels) - macOS default is 22px
    private const float menuBarHeight = 22f; 
    // Dock height (in pixels) - can vary but a typical value is 40px
    private const float dockHeight = 40f; 

    public float previousWidth;

    public float previousHeight;

    public float previousXOffset;

    public float previousYOffset;

    private float settingsWidthOffset;

    private float settingsHeightOffset;

    private float settingsXOffset;

    private float settingsYOffset;

    public TMP_InputField width;
    public TMP_InputField height;

    public TMP_InputField xOffset;

    public TMP_InputField yOffset;

    public UniWindowController overlay;

    public GameObject warning;

    public GameObject menu;

    public void Load()
    {
        lastSetting = transform.position;
        menu.SetActive(true);
        overlay.enableFeedbackState();
        previousXOffset = float.Parse(xOffset.text);
        previousYOffset = float.Parse(yOffset.text);
        previousHeight = float.Parse(height.text);
        previousWidth = float.Parse(width.text);
    }

    public void Start()
    {
        lastSetting = transform.position;
    }

    public void Abort()
    {
        overlay.disableFeedbackState();
        warning.SetActive(false);
    }

    public void resetOffset(){
        xOffset.text = previousXOffset.ToString();
        yOffset.text = previousYOffset.ToString();
        height.text = previousHeight.ToString();
        width.text = previousWidth.ToString();
        warning.SetActive(false);
    }


    private Vector3 lastSetting;
    void Update()
    {
        
        float.TryParse(width.text, out settingsWidthOffset);
        float.TryParse(height.text, out settingsHeightOffset);
        float.TryParse(xOffset.text, out settingsXOffset);
        float.TryParse(yOffset.text, out settingsYOffset);

        // Get the full screen resolution
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        //aspectRatio = screenWidth / screenHeight;

        // Since the height of the plane is fixed to 1, calculate the required width based on the aspect ratio
        float requiredWidth = screenHeight * aspectRatio;

        // Scale the plane's x value based on the required width
        Vector3 scale = transform.localScale;
        scale.x = -aspectRatio + settingsWidthOffset; // Scale based on the screen width (normalize to screen)
        scale.z = 1 - 0.1f + settingsHeightOffset; // Keep the y value fixed at 1 (as per your requirement)
        transform.localScale = scale;

        transform.position = lastSetting + new Vector3(settingsXOffset, settingsYOffset, 0);

        // Log the results for debugging
        Debug.Log($"Screen Resolution: {screenWidth}x{screenHeight}");
        Debug.Log($"Calculated Required Width: {requiredWidth}");
    }

    public void CheckForUnsavedAndClose()
    {
        if (float.TryParse(xOffset.text, out float currentXOffset) &&
        float.TryParse(yOffset.text, out float currentYOffset) &&
        float.TryParse(width.text, out float currentWidth) &&
        float.TryParse(height.text, out float currentHeight)){
            if(currentXOffset != previousXOffset || currentYOffset != previousYOffset || currentHeight != previousHeight || currentWidth != previousWidth){
                warning.SetActive(true);
            } else {
                Abort();
            }
        }
    }
}
