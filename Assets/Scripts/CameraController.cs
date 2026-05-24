using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Transform target;

    public Transform clampMin; // this is the mimúm clamp for the camera so the player doest not go oustide the grass
    public Transform clampMax; // this is the maximum clamp for the camera so the player do

    private Camera cam;
    private float halfwidth, halfheight;// this is the code that calculates the half width and half height of the camera view. this is used to calculate the clamp values for the camera so it does not go outside the bounds of the grass.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //target= FindAnyObjectByType<PlayerController>().transform; FindAnyObjectByType is a new method in Unity 2024 that finds the first object of the specified type in the scene. In this case, it finds the PlayerController and gets its transform to use as the target for the camera.
        target = PlayerController.instance.transform; // this is the code that gets the transform of the player controller instance and sets it as the target for the camera.
        clampMin.SetParent(null); // this basícally meand the min and max shi is not a child of camera it does not move with the cam.
        clampMax.SetParent(null);// Zame as the above but for max mate.

        cam = GetComponent<Camera>();// this is the code that gets the Camera component attached to the same GameObject as this script. this is used to calculate the half width and half height of the camera view.
        halfheight=cam.orthographicSize;
        halfwidth=halfheight*cam.aspect;
        // in unity for 2d you use orthographic for 2d.

    }

    // Update is called once per frame
    void Update()
    {
        transform.position= new Vector3(target.position.x, target.position.y,transform.position.z);// this is the code that sets the position of the camera to the position of the target (the player). this allows the camera to follow the player as they move around the scene mate


        Vector3 clampedpostion = transform.position;// this is the code that clamps the position of the camera to the clampMin and clampMax values. this ensures that the camera does not go outside the bounds of the grass
    
        clampedpostion.x = Mathf.Clamp(clampedpostion.x, clampMin.position.x + halfwidth, clampMax.position.x - halfwidth);// this is the code that clamps the x value of the camera position to the clampMin and clampMax values. this ensures that the camera does not go outside the bounds of the grass.
        clampedpostion.y = Mathf.Clamp(clampedpostion.y, clampMin.position.y + halfheight, clampMax.position.y- halfheight);// this is the code that clamps the y value of the camera position to the clampMin and clampMax values

        transform.position = clampedpostion;// this is the code that sets the position of the camera to the clamped position. this ensures that the camera does not go outside the bounds of the grass.
    }
}
