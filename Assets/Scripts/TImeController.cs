using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TImeController : MonoBehaviour
{
    public static TImeController instance;// you want to keep track of the time controller and this is gonna be used to advance the time when the player sleeps in the bed. 
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else// othwerwise destroy the game. 
        {
            Destroy(gameObject);
        }
    }
    public float currentTime;

    public float dayStart, dayEnd;// i set it as start for 7 and end for 26 in unity script 

    public float timespeed = .25f; // time speed set to this if i want to i can change "rember"

    private bool timeactive;

    public int currentDay = 1; // this is the current day that the player is on

    public string DayEndScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = dayStart;// when opned the game it is always the start of the day.

        timeactive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeactive==true)
        {
            currentTime += Time.deltaTime * timespeed;// this is the code that advances the current time by the time that has passed since the last frame. 

            if (currentTime > dayEnd)
            {
                {
                    currentTime= dayEnd;// if the current time is greater than the end of the day then we set it to the end of the day and end the day
                    EndDay();
                }
            }
            if (UIcontroller.instance != null)
            {
                UIcontroller.instance.UpdateTimeText(currentTime);
            }
        }
    }

    public void EndDay()
    {
        timeactive = false;

        currentDay++;

        GridInfo.instance.GrowCrop();// when we end the day the crop gonna grow.

        //StarDay();

        SceneManager.LoadScene(DayEndScene); // this gonna load this scene at the end of a the day.
    }

    public void StarDay()
    {
        timeactive = true;

        currentTime = dayStart;
    }
}
