using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    public ChangeButtonApperance associatedButton;
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleSettings()
    {
        if (!associatedButton.isSprite1Active)
        {
            Debug.Log(associatedButton.isSprite1Active);
            GetComponent<ChangeButtonApperance>().SwapSpritesAndTextColor();
        }
    }
}
