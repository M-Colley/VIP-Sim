using UnityEngine;
using UnityEngine.UI;
using TMPro; // Correct namespace for TextMeshPro
using mcDesktopCapture;
using System.Linq;

public class MacCapture : MonoBehaviour
{
    [SerializeField]
    private ScrollRect scrollView; // The ScrollView that holds the window list.
    [SerializeField]
    private GameObject buttonPrefab; // The prefab for a single Button (not Toggle).

    private WindowProperty[] list = { };

    public bool isRunning = false;
    private bool setTexture = false;
    private bool isInit = false;

    [SerializeField]
    public Material transparentMaterial;

    public Renderer planeRenderer;

    public FirestoreRESTManager logger;

    public void Init()
    {
        Application.targetFrameRate = 60;

        DesktopCapture2.Init();

        list = DesktopCapture2.WindowList;
        var non = new WindowProperty
        {
            windowID = -999,
            owningApplication = new WindowProperty.RunningApplication
            {
                applicationName = "Stop"
            },
            frame = new WindowProperty.Frame(),
            isOnScreen = true
        };

        // Append Property for Stop
        list = list.Append(non).ToArray();

        // Clear previous buttons before adding new ones
        foreach (Transform child in scrollView.content)
        {
            Destroy(child.gameObject);
        }

        int i = 0;

        foreach (var window in list)
        {

            // Make button interactable based on window status
            if(!window.isOnScreen || window.owningApplication.applicationName == "Stop" || window.owningApplication.applicationName == "" || window.owningApplication.applicationName.ToLower().Replace("_","").Contains("vipsim")) {
                continue;
            }


            var buttonObj = Instantiate(buttonPrefab, scrollView.content);
            RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();

            if (buttonRect == null)
            {
                Debug.LogError("RectTransform not found on buttonPrefab!");
                continue;
            }

            // Set height and spacing for the prefab
            //buttonRect.sizeDelta = new Vector2(buttonRect.sizeDelta.x, 70);  // Set height (adjust value)

            // Adjust the position of the instantiated buttons to add spacing
        buttonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(buttonObj.GetComponent<RectTransform>().anchoredPosition.x, - ((i * buttonRect.sizeDelta.y)));

            // Get the Button component inside the prefab
            Button button = buttonObj.GetComponentInChildren<Button>();
            if (button == null)
            {
                Debug.LogError("Button component not found in prefab!");
                continue;
            }

            // Find TMP_Text inside the button component
            TMP_Text tmpText = button.GetComponentInChildren<TMP_Text>();
            if (tmpText == null)
            {
                Debug.LogError("TMP_Text component not found inside the Button!");
                continue;
            }


            tmpText.text = $"{window.owningApplication.applicationName}";

            // Set up Button action when clicked
            button.onClick.AddListener(() => OnButtonClicked(window));
            button.onClick.AddListener(() => logger.OnProgramClick(window.owningApplication.applicationName));
            

            i++;
        }



        isInit = true;
        this.enabled = true;
    }
    private static int lastWindowID = -1;
    // Called when a button is clicked
    private void OnButtonClicked(WindowProperty window)
    {
        if (isRunning)
        {
            DesktopCapture2.StopCapture();
            setTexture = false;
            planeRenderer.enabled = false;
            isRunning = false;
            Debug.Log("test1");
        }
        if(window.windowID == lastWindowID){
            lastWindowID = -1;
            return;
        }
            planeRenderer.enabled = true;
            // Start capture for selected window
            isRunning = true;
            StartCapture(window);
            lastWindowID = window.windowID;
            Debug.Log("test2");
        
        
    }

    // Start capture for a selected window
    private void StartCapture(WindowProperty window)
    {
        DesktopCapture2.StartCaptureWithWindowID(window.windowID, window.frame.width, window.frame.height, true);
        isRunning = true;
        Debug.Log($"Started capture for window {window.windowID} ({window.owningApplication.applicationName})");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInit || !isRunning){
            planeRenderer.enabled = false;
            return;
        } 

        if (setTexture) return;

        var texture = DesktopCapture2.GetTexture2D();
        if (texture == null) return;

        setTexture = true;
        planeRenderer.material.mainTexture = texture;
    }

    void OnDisable()
    {
        Debug.Log("OnDisable");
        DesktopCapture2.StopCapture();
        DesktopCapture2.Destroy();
    }

    // Stop capture manually
    public void StopCapture()
    {
        DesktopCapture2.StopCapture();
        setTexture = false;
        isRunning = false;
        planeRenderer.material = transparentMaterial;
    }
}
