using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DayEndController : MonoBehaviour

{
    public TMPro.TextMeshProUGUI DayText; // this is the text that will show the current day when the day ends.
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public string WakeUpScene; // this is the name of the scene that will be loaded when the player wakes up

    void Start()
    {
        if(TImeController.instance != null)
        {
            DayText.text = " - Day " + TImeController.instance.currentDay + " -"; // this is the code that sets the text of the day text to the current day when the day ends.
        }
    }
    private void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)

        {
            TImeController.instance.StarDay();// start day after player clicks any button or mouse

            SceneManager.LoadScene(WakeUpScene); // this is the code that loads the wake up scene when the player clicks any button or mouse.
        }
    }
}
