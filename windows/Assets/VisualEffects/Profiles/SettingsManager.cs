using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json; // F�r JSON (installiere NewtonSoft.Json aus dem Unity Asset Store oder per NuGet)
using System.Reflection;
using MathNet.Numerics.Interpolation;
using System.Threading.Tasks;
using VisSim;
using static VisSim.myRecolour;
using static VisSim.myFloaters;
using SimpleFileBrowser; // F�r Reflection, um alle Public Variablen zu aktualisieren

public class SettingsManager : MonoBehaviour
{
    // Referenzen zu den Buttons
    public Button loadButton;
    public Button saveButton;

    // Referenz zu den aktuellen Einstellungen
    public AppSettings appSettings = new AppSettings();

    [SerializeField]
    public myFieldLoss myFieldLoss;

    [SerializeField]
    public myBlur myBlur;

    [SerializeField]
    public myRecolour myRecolour;

    [SerializeField]
    public myBrightnessContrastGamma myBrightnessContrastGamma;

    [SerializeField]
    public myDistortionMap myDistortionMap;

    [SerializeField]
    public myNystagmus myNystagmus;

    [SerializeField]
    public myFloaters myFloaters;

    [SerializeField]
    public myTeichopsia myTeichopsia;

    [SerializeField]
    public myWiggle myWiggle;

    [SerializeField]
    public myBloom myBloom;

    [SerializeField]
    public myFieldLossInverted myFieldLossInverted;

    [SerializeField]
    public myCataract myCataract;

    [SerializeField]
    public myInpainter2 myInpainter2;

    [SerializeField]
    public DoubleVisionEffect myDoubleVision;

    [SerializeField]
    public VortexEffect myVortexEffect;

    [SerializeField]
    public FovealDarkness myFovealDarkness;

    [SerializeField]
    public FlickeringStars myFlickeringStars;

    [SerializeField]
    public PixelationEffect myPixelationEffect;

    private void Start()
    {
        // Event Listener f�r die Buttons hinzuf�gen
        loadButton.onClick.AddListener(OpenFileBrowser);
        saveButton.onClick.AddListener(SaveSettings);
        InvokeRepeating("SyncSettings", 5.0f, 5.0f);
    }

    // �ffnet einen Dateibrowser, um die Datei zu laden
    private async void OpenFileBrowser()
    {
        string path = await OpenFilePanelAsync();
        if (!string.IsNullOrEmpty(path))
        {
            LoadSettings(path);
        }
    }

    // Speichert die aktuellen Einstellungen in eine Datei
    private async void SaveSettings()
    {
        string path = await SaveFilePanelAsync();
        if (!string.IsNullOrEmpty(path))
        {
            string json = JsonConvert.SerializeObject(appSettings, Formatting.Indented);
            File.WriteAllText(path, json);
            Debug.Log("Settings saved to: " + path);
        }
    }

    // L�dt Einstellungen aus einer JSON-Datei
    private void LoadSettings(string path)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            appSettings = JsonConvert.DeserializeObject<AppSettings>(json);
            ApplySettings(); // �bernimmt die Einstellungen ins Spiel
            Debug.Log("Settings loaded from: " + path);
        }
        else
        {
            Debug.LogError("File not found: " + path);
        }
    }

    // �bernimmt die geladenen Einstellungen und aktualisiert alle relevanten Public Variablen in anderen Skripten
    private void ApplySettings()
    {
        // Aktualisiere alle Objekte mit Public Variablen
        MonoBehaviour[] allBehaviours = FindObjectsOfType<MonoBehaviour>();
        foreach (var behaviour in allBehaviours)
        {
            UpdatePublicFields();
        }

        // Beispiel: wende Licht-Intensit�t an
        Light[] lights = FindObjectsOfType<Light>();
        foreach (var light in lights)
        {
            light.intensity = appSettings.intensity;
        }
    }

    // Aktualisiert die Public Variablen eines MonoBehaviour anhand der aktuellen AppSettings
    private void UpdatePublicFields()
    {
        // vision loss central
        myFieldLoss.overlayScale = appSettings.overlayscale;

        // Hyperopia
        myBlur.maxCPD = appSettings.maxCPD;
        // Color Vision Deficiency
        myRecolour.anomType = appSettings.anomType;
        myRecolour.severityIndex = appSettings.severityIndex;
        //  Contrast Sensitivty
        myBrightnessContrastGamma.Contrast = appSettings.contrast;
        myBrightnessContrastGamma.Brightness = appSettings.brightness;
        myBrightnessContrastGamma.Gamma = appSettings.gamma;
        //Nystagmus
        myNystagmus.foveat_d = appSettings.foveat_d;
        myNystagmus.rise_d = appSettings.rise_d;
        myNystagmus.rise_exp = appSettings.rise_exp;
        myNystagmus.amp_deg = appSettings.amp_deg;
        //Retinopathy
        myFloaters.floaterType = appSettings.type;
        myFloaters.intensity = appSettings.intensity;
        myFloaters.floaterSize = appSettings.floaterSize;
        myFloaters.floaterDensity =  appSettings.floaterDensity;
        myFloaters.circleRadius = appSettings.circleRadius;
        myFloaters.center = appSettings.center;
        myFloaters.Speed = appSettings.r_speed; //!!!!
        //Teichopisa
        myTeichopsia.Strength = appSettings.strength;
        myTeichopsia.LumContribution = appSettings.lumContribution;
        myTeichopsia.gazeContingent = appSettings.gazeContingent;
        //Metamorphosia
        myWiggle.Timer = appSettings.timer;
        myWiggle.Speed = appSettings.speed;
        myWiggle.Frequency = appSettings.frequency;
        myWiggle.Amplitude = appSettings.amplitute;
        //Photophobia
        myBloom.intensity = appSettings.p_intensity; //!!!!
        myBloom.threshold = appSettings.threshold;
        myBloom.blurSize = appSettings.blurSize;
        //Vision Loss peripheral
        myFieldLossInverted.overlayScale = appSettings.overlayscale;
        //Cataract
        myCataract.severityIndex = appSettings.c_severityIndex; //!!!!
        myCataract.useFrosting = appSettings.useFrosting;
        myCataract.Gamma = appSettings.c_gamma; //!!!!
        //In-Filling
        myInpainter2.threshold = appSettings.i_threshold; //!!!!
        //Double Vision
        myDoubleVision.displacementAmount = appSettings.displacementAmount;
        //Distortion
        myVortexEffect.vortexRadius = appSettings.vortexRadius;
        myVortexEffect.suctionStrength = appSettings.suctionStrenght;
        myVortexEffect.innerCircleRadius = appSettings.innerCircleRadius;
        myVortexEffect.noiseAmount = appSettings.noiseAmount;
        //Foveal Darkness
        myFovealDarkness.innerCircleRadius = appSettings.innnerCircleRadius;
        myFovealDarkness.fadeWidth = appSettings.fadWidth;
        myFovealDarkness.opacity = appSettings.opacity;
        //Flickering Stars
        myFlickeringStars.radius = appSettings.radius;
        myFlickeringStars.starRadius = appSettings.starRadius;
        myFlickeringStars.fadeInDuration = appSettings.fadeInDuration;
        myFlickeringStars.fadeOutDuration = appSettings.fadeOutDuration;
        //Detail Loss
        myPixelationEffect.pixelRadius = appSettings.pixelRadius;

}

    // Synchronisiert die AppSettings, wenn sich eine Public Variable �ndert
    public void SyncSettings()
    {
        // vision loss central
        appSettings.overlayscale = myFieldLoss.overlayScale;

        // Hyperopia
        appSettings.maxCPD = myBlur.maxCPD;
        // Color Vision Deficiency
        appSettings.anomType = myRecolour.anomType;
        appSettings.severityIndex = myRecolour.severityIndex;
        // Contrast Sensitivity
        appSettings.contrast = myBrightnessContrastGamma.Contrast;
        appSettings.brightness = myBrightnessContrastGamma.Brightness;
        appSettings.gamma = myBrightnessContrastGamma.Gamma;
        // Nystagmus
        appSettings.foveat_d = myNystagmus.foveat_d;
        appSettings.rise_d = myNystagmus.rise_d;
        appSettings.rise_exp = myNystagmus.rise_exp;
        appSettings.amp_deg = myNystagmus.amp_deg;
        // Retinopathy
        appSettings.type = myFloaters.floaterType;
        appSettings.intensity = myFloaters.intensity;
        appSettings.floaterSize = myFloaters.floaterSize;
        appSettings.floaterDensity = myFloaters.floaterDensity;
        appSettings.circleRadius = myFloaters.circleRadius;
        appSettings.center = myFloaters.center;
        appSettings.r_speed = myFloaters.Speed;
        // Teichopisa
        appSettings.strength = myTeichopsia.Strength;
        appSettings.lumContribution = myTeichopsia.LumContribution;
        appSettings.gazeContingent = myTeichopsia.gazeContingent;
        // Metamorphosia
        appSettings.timer = myWiggle.Timer;
        appSettings.speed = myWiggle.Speed;
        appSettings.frequency = myWiggle.Frequency;
        appSettings.amplitute = myWiggle.Amplitude;
        // Photophobia
        appSettings.p_intensity = myBloom.intensity;
        appSettings.threshold = myBloom.threshold;
        appSettings.blurSize = myBloom.blurSize;
        // Vision Loss peripheral
        appSettings.overlayscale = myFieldLossInverted.overlayScale;
        // Cataract
        appSettings.c_severityIndex = myCataract.severityIndex;
        appSettings.useFrosting = myCataract.useFrosting;
        appSettings.c_gamma = myCataract.Gamma;
        // In-Filling
        appSettings.i_threshold = myInpainter2.threshold;
        // Double Vision
        appSettings.displacementAmount = myDoubleVision.displacementAmount;
        // Distortion
        appSettings.vortexRadius = myVortexEffect.vortexRadius;
        appSettings.suctionStrenght = myVortexEffect.suctionStrength;
        appSettings.innerCircleRadius = myVortexEffect.innerCircleRadius;
        appSettings.noiseAmount = myVortexEffect.noiseAmount;
        // Foveal Darkness
        appSettings.innnerCircleRadius = myFovealDarkness.innerCircleRadius;
        appSettings.fadWidth = myFovealDarkness.fadeWidth;
        appSettings.opacity = myFovealDarkness.opacity;
        // Flickering Stars
        appSettings.radius = myFlickeringStars.radius;
        appSettings.starRadius = myFlickeringStars.starRadius;
        appSettings.fadeInDuration = myFlickeringStars.fadeInDuration;
        appSettings.fadeOutDuration = myFlickeringStars.fadeOutDuration;
        // Detail Loss
        appSettings.pixelRadius = myPixelationEffect.pixelRadius;

    }

    // Methoden f�r den Standalone File Browser
    /*
    private string OpenFilePanel()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Select a Settings File", "", "profile", false);
        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            return paths[0];
        }
        return null;
    }

    private string SaveFilePanel()
    {
        string path = StandaloneFileBrowser.SaveFilePanel("Save Settings File", "", "settings", "profile");
        if (!string.IsNullOrEmpty(path))
        {
            return path;
        }
        return null;
    }
    */

    public async Task<string> OpenFilePanelAsync()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Profile Files", ".profile"));
        FileBrowser.SetDefaultFilter(".profile");

        FileBrowser.WaitForLoadDialog(
            FileBrowser.PickMode.Files,
            false,
            null,
            null,
            "Select a Settings File",
            "Select");

        return (FileBrowser.Success && FileBrowser.Result.Length > 0) ? FileBrowser.Result[0] : null;
    }

    public async Task<string> SaveFilePanelAsync()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Profile Files", ".profile"));
        FileBrowser.SetDefaultFilter(".profile");

        FileBrowser.WaitForSaveDialog(
            FileBrowser.PickMode.Files,
            false,
            null,
            "settings.profile",
            "Save Settings File",
            "Save");

        return FileBrowser.Success ? FileBrowser.Result[0] : null;
    }
}

// Klasse, die alle Einstellungen enth�lt
[System.Serializable]
public class AppSettings
{
    
    public string playerName = "Player";
    public int resolutionWidth = 1920;
    public int resolutionHeight = 1080;
    public bool fullscreen = true;
    public float volume = 0.5f;

    // F�ge hier weitere Einstellungen hinzu, die gespeichert werden sollen

    // vision loss central
    public float overlay_scale;
    // Hyperopia
    public float maxCPD;
    // Color Vision Deficiency
    public AnomolyType anomType;
    // Contrast Sensitivty
    public float severityIndex;
    //Metamorphosia
    public float brightness;
    public float contrast;
    public float gamma;
    //Nystagmus
    public float foveat_d;
    public float rise_d;
    public float rise_exp;
    public float amp_deg;
    //Retinopathy
    public FloaterType type;
    public float intensity;
    public float floaterSize;
    public float floaterDensity;
    public float circleRadius;
    public bool center;
    public float r_speed; //!!!!
    //Teichopisa
    public float strength;
    public float lumContribution;
    public bool gazeContingent;
    //Metamorphosia
    public float timer;
    public float speed;
    public float frequency;
    public float amplitute;
    //Photophobia
    public float p_intensity; //!!!!
    public float threshold;
    public float blurSize;
    //Vision Loss peripheral
    public float overlayscale;
    //Cataract
    public float c_severityIndex; //!!!!
    public bool useFrosting;
    public float c_gamma; //!!!!
    //In-Filling
    public float i_threshold; //!!!!
    //Double Vision
    public float displacementAmount;
    //Distortion
    public float vortexRadius;
    public float suctionStrenght;
    public float innerCircleRadius;
    public float noiseAmount;
    //Foveal Darkness
    public float innnerCircleRadius;
    public float fadWidth;
    public float opacity;
    //Flickering Stars
    public float radius;
    public float starRadius;
    public float fadeInDuration;
    public float fadeOutDuration;
    //Detail Loss
    public float pixelRadius;
}
