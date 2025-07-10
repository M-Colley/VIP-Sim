using TMPro;
using UnityEngine;

public class ZoomSettings : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public TMP_InputField xOffset; // Assign in Inspector
    public TMP_InputField yOffset; // Assign in Inspector
    public TMP_InputField zoom; // Assign in Inspector
    public GameObject WindowManager;

    public GameObject warning;  

    public TransparentWindow transparency;

    private float previousXOffset;
    private float previousYOffset;
    private float previousZoom;

    private bool settingsOpen;

    public void Load()
    {
        settingsOpen = true;
        xOffset.text = WindowManager.transform.position.x.ToString();
        yOffset.text = WindowManager.transform.position.y.ToString();

        float.TryParse(xOffset.text, out previousXOffset);    
        float.TryParse(yOffset.text, out previousYOffset);
        

        // Erstes Kind holen
        Transform firstChild = WindowManager.transform.GetChild(0);

        // Prüfen, ob das erste Kind einen BoxCollider hat
        BoxCollider boxCollider = firstChild.GetComponent<BoxCollider>();

        if (boxCollider != null)
        {
            zoom.text = boxCollider.size.y.ToString();
            float.TryParse(zoom.text, out previousZoom);    
        }
        else
        {
            Debug.LogError("BoxCollider not found on the first child of uWindowCapture!");
        }

    }

    public void Abort()
    {
        settingsOpen = false;
        WindowManager.transform.position = new Vector3(previousXOffset, previousYOffset, WindowManager.transform.position.z);
        // Erstes Kind holen
        Transform firstChild = WindowManager.transform.GetChild(0);

        // Prüfen, ob das erste Kind einen BoxCollider hat
        BoxCollider boxCollider = firstChild.GetComponent<BoxCollider>();

        if (boxCollider != null)
        {
            Vector3 size = boxCollider.size;
            size.y = previousZoom;
            boxCollider.size = size;
            transparency.disableFeedbackState();
        }
        else
        {
            Debug.LogError("BoxCollider not found on the first child of WindowManager!");
        }
    }

    float timer = 0f;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.1f && settingsOpen)
        {
            Save();
            timer = 0f; // Reset the timer
        }
    }

    public void Save()
    {
        // Werte aus den InputFields holen und in Float umwandeln
        if (float.TryParse(xOffset.text, out float x) && float.TryParse(yOffset.text, out float y))
        {
            WindowManager.transform.position = new Vector3(x, y, WindowManager.transform.position.z);
        }
        else
        {
            Debug.LogError("Invalid input for xOffset or yOffset!");
        }

        if (float.TryParse(zoom.text, out float newZoom))
        {
            // Erstes Kind holen
            Transform firstChild = WindowManager.transform.GetChild(0);

            // Prüfen, ob das erste Kind einen BoxCollider hat
            BoxCollider boxCollider = firstChild.GetComponent<BoxCollider>();

            if (boxCollider != null)
            {
                Vector3 size = boxCollider.size;
                size.y = newZoom;
                boxCollider.size = size;
            }
            else
            {
                Debug.LogError("BoxCollider not found on the first child of WindowManager!");
            }
        }
        else
        {
            Debug.LogError("Invalid input for zoom!");
        }
    }

    public void CheckForUnsavedAndClose()
    {
        if (float.TryParse(xOffset.text, out float currentXOffset) &&
        float.TryParse(yOffset.text, out float currentYOffset) &&
        float.TryParse(zoom.text, out float currentZoom))
        {
            Debug.Log($"Checking condition: currentXOffset={currentXOffset}, previousXOffset={previousXOffset}, currentYOffset={currentYOffset}, previousYOffset={previousYOffset}, currentZoom={currentZoom}, previousZoom={previousZoom}");

            if (currentXOffset != previousXOffset || currentYOffset != previousYOffset || currentZoom != previousZoom)
            {
                Debug.Log("Condition met, values changed.");
            }

            if (currentXOffset != previousXOffset || currentYOffset != previousYOffset || currentZoom != previousZoom)
            {
                Debug.Log("One of the values has changed!");
                warning.SetActive(true);

            } else
            {
                Abort();
            }
        }
    }

}
