using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaSwitcher : MonoBehaviour
{
    public string SceneToLoad; // this is the name of the scene that will be loaded
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform starPoint;

    public string transitionName;

    void Start()
    {
        if (PlayerPrefs.HasKey("Transition"))
        {

            if (PlayerPrefs.GetString("Transition") == transitionName) // this is the code that checks if the transition name stored in the player prefs is the same as the transition name
            {
                PlayerController.instance.transform.position = starPoint.position; // this is the code that sets the position of the player to the star point positio.
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") // imp to set the player tag to player in unity for this to work orginally set to untagge.
        {
            {
                Debug.Log("Player entered");

                SceneManager.LoadScene(SceneToLoad);// this gonna take to inside the house.

                PlayerPrefs.SetString("Transition", transitionName);// a Way to save info of player to set or store the transition t.ex main indoors or outdoors.
            }
        }
    }
}