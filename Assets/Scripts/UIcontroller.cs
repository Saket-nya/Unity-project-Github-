using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIcontroller : MonoBehaviour

{ // game object is an array 
    public GameObject[] toolbarActivatiorIcons;// what is game object Literally any object in unity is game object for example main grid camera growblock etc and everthing has a tranformator

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static UIcontroller instance;// this is a static variable that will hold a reference to the instance of the UIcontroller script. this allows other scripts to access the UIcontroller script without having to find it in the scene.
    private void Awake()// this is a Unity method that is called when the script instance is being loaded. this is used to set the instance variable to the current instance of the UIcontroller script.
    { if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }   
    }

    public TMP_Text timeText;

    public InventoryController theIC;// inventory controllecter this created bcoz ic is part of the UI

    public Image seedImage;


    void Start()
    {
        SwitchTool(0);// this will set the first tool as active when the game starts
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            theIC.OpenClose();
        }
        
    }

    public void SwitchTool(int selected)// this function will be called when the player clicks on a tool button in the UI and it will take an integer parameter that represents the index of the selected tool in the toolbarActivatiorIcons array
    {
    
    foreach(GameObject icon in toolbarActivatiorIcons)
        {
                        icon.SetActive(false);

        }
        toolbarActivatiorIcons[selected].SetActive(true);

    }

    // Alias to match PlayerController call: switchToolIcon
    public void switchToolIcon(int selected)
    {
        SwitchTool(selected);
    }

    public void UpdateTimeText(float currentTime)// fasta regeler om vad texten current time ska göra 
    {
        if (currentTime < 12)
        {
            timeText.text = Mathf.FloorToInt(currentTime) + "AM";
        }
        else if (currentTime <13)
        {
            timeText.text = "12PM";
        }
        else if (currentTime < 24)
        {
            timeText.text = Mathf.FloorToInt(currentTime - 12) + "PM";
        }
        else if(currentTime < 25)
        { 
        timeText.text = "12AM";
        }
        else
        {
            timeText.text = Mathf.FloorToInt(currentTime - 24) + "PM";
        }
    }

    public void SwitchSeed(CropController.CropType crop)
    {
        seedImage.sprite = CropController.instance.GetCropInfo(crop).seedType;
    }
}
