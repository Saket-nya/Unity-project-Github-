using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private void Awake()// can be acessed any time 
    {
        if (instance == null)// so that the player dummy does not create
        {
            instance = this;
            DontDestroyOnLoad(gameObject);// Normally when loaded in to a new scence everything in the previous gets destroyed but it doesnät now.
        }
        else
        {
            Destroy(gameObject);// if there is already an instance of the PlayerController, we destroy this new one to ensure that there is only one instance in the scene.
        }
    }

    public Rigidbody2D theRB;
    public float moveSpeed;// broski look at the unity app the speed was set to 8 mate.

    public InputActionReference moveInput, actionInput;// this is the input action reference for the player movement.


    public Animator anim;

    public enum ToolType
    {
        plough,
        wateringCan,
        seeds,
        basket,
    }
    public ToolType currentTool;// at this point you cant move the tool but down in the code i will put a if statment to change the tool on tap

    public float toolWaitTime = .5f; //THis much time to wait every time we use a tool, it to make the transion of graphics smoother.

    public float toolWaitCounter; // Toolwaitecounter for counting how much time has passed. Imp to note ; 

    public Transform toolIndicator;// this is the transform for the tool indicator.
    public float toolRange = 3f; // I set 3 float as range but can be changed to your own liking.

    public CropController.CropType seedCropType;// this is the current crop type that the player is planting, we will use this to determine which sprites to use for the different growth stages of the crops.


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIcontroller.instance.switchToolIcon((int)currentTool);
    }

    // Update is called once per frame
    void Update()
    {
        if (UIcontroller.instance != null)
        {
            if (UIcontroller.instance.theIC != null)
            {
                if (UIcontroller.instance.theIC.gameObject.activeSelf == true)// basscially when the inventory is open the player ain't moving.
                {
                    theRB.linearVelocity = Vector2.zero;

                    return;// bcoz this is void funktion doesnät return the remaining code.
                }
            }
        }

        if (toolWaitCounter > 0)
        {
            toolWaitCounter -= Time.deltaTime; // this is the code that decreases the tool wait counter by the time that has passed since the last frame.
            theRB.linearVelocity = Vector2.zero;
        }
        else
        {
            //theRB.linearVelocity = new Vector2(moveSpeed,0f);// new vector that stores x and y values for the player movement.
            theRB.linearVelocity = moveInput.action.ReadValue<Vector2>().normalized * moveSpeed;// this is the code that allows the player to move using the input action reference and the move speed variable. moreover normulized is used to ensure that the player moves at the same speed in all directions.


            if (theRB.linearVelocity.x < 0f)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f); //if the player is moving left, the local scale of the player is set to -1 on the x axis to flip the sprite. also make sure that mate your player has a x and y value of 0,0,0
            }

            else if (theRB.linearVelocity.x > 0f)
            {
                transform.localScale = Vector3.one; // if the player is moving right
            }
        }
        bool hasSwitchedTool = false;

        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {

            currentTool++;
            hasSwitchedTool = true;
        }

        if ((int)currentTool >= 4)//4 bacuse in tooltype we have 4 tools ex: plough, wateringcan etc. so if the current tool is greater than or equal to 4, we reset it
        {
            currentTool = ToolType.plough;

            hasSwitchedTool = true;
        }
        if (Keyboard.current.digit1Key.wasPressedThisFrame)// this if statement is to change the tools while pressed num 1 to 4 och note: vikgit att kolla om att alla har rätt tool.
        {
            currentTool = ToolType.plough;

            hasSwitchedTool = true;
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)// note the graphic 
        {
            currentTool = ToolType.wateringCan;
            hasSwitchedTool = true;
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            currentTool = ToolType.seeds;
            hasSwitchedTool = true;
        }
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            currentTool = ToolType.basket;
            hasSwitchedTool = true;
        }

        if (hasSwitchedTool == true)
        {
            FindFirstObjectByType<UIcontroller>().switchToolIcon((int)currentTool);
            UIcontroller.instance.switchToolIcon((int)currentTool);
        }

        anim.SetFloat("speed", theRB.linearVelocity.magnitude);

        if (GridController.instance != null)
        {


            if (actionInput.action.WasPressedThisFrame()) // this is the code that checks if the action input was pressed this frame and if so, it will print "Action button pressed" to the console.
            {
                UseTool();
            }
            // if action previosely wenread the value but this time we are gonna check if it was pressed this frame. this is useful for actions that should only happen once when the button is pressed, such as using a tool or jumping.

           // dont need this inside the statment he walks the whole time if ther
           // -anim.SetFloat("speed", theRB.linearVelocity.magnitude);// this is the code that sets the moveX parameter in the animator to the x value of the player's velocity. magntidue is used to get the speed of the player regardless of the direction they are moving in.

            toolIndicator.position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()); // This makes the tool indi follow the mouse pos on screen.
            toolIndicator.position = new Vector3(toolIndicator.position.x, toolIndicator.position.y, 0f); // This gonna set the Z value to zero as we know the game aura farming is not a 3d game. // note check if i have done this for player as weel

            if (Vector3.Distance(toolIndicator.position, transform.position) > toolRange)// This will adjust the indicator. so the indicator doesnt go beyond the range of the tool.
            {
                Vector2 direction = toolIndicator.position - transform.position;
                direction = direction.normalized * toolRange;
                toolIndicator.position = transform.position + new Vector3(direction.x, direction.y, 0f);
            }
            toolIndicator.position = new Vector3(Mathf.FloorToInt(toolIndicator.position.x) + .5f,
                Mathf.FloorToInt(toolIndicator.position.y) + .5f,
                0f); // This is to snap the tool indicator to the grid. and make sure the z is 0.
        }
        else
        {
            toolIndicator.position= new Vector3(0f, 0f, -20f);
        }
    }

    void UseTool()// this is code used to diffrent things till exempel we can use it for changing the click and choose the weapons or tools for the player or even others. 
    {
        GrowBlock block = null;

        /*block = FindFirstObjectByType<GrowBlock>();*/// this is the code that finds the first object of type GrowBlock in the scene and assigns it to the block variable. this is just an example and you can change it to find the specific block that the player is interacting with.

        block = GridController.instance.GetBlock(toolIndicator.position.x - 0.5f, toolIndicator.position.y - 0.5f);

        toolWaitCounter= toolWaitTime; // i set to .05 evertime i change my tool.

        // block.PloughSoil();
        //if (Block != null)
        //{
        //    Block.AdvanceStage();
        //}
        if (block != null)// beacuse we have enum vi will use switch case
        {
            switch (currentTool)
            {
                case ToolType.plough:

                    block.PloughSoil();// this switch case only allows to plough when the tool is plough for example.

                    anim.SetTrigger("usePlough");// this is the code that sets the useTool trigger in the animator to play the ploughing animation when the player uses the plough tool.  


                    break;
            
            
                case ToolType.wateringCan:
                     
                    block.WaterSoil();

                    anim.SetTrigger("useWateringCan");// This it mate to plat the watering can animation when the player uses the water can tool by clicking 2.

                    break;
           
           
            
                case ToolType.seeds:
                    if (CropController.instance.GetCropInfo(seedCropType).seedAmount > 0)
                    {

                        block.PlantCrop(seedCropType);

                        //CropController.instance.UseSeed(seedCropType);
                    }

                    break;
            
            
                case ToolType.basket:
                    block.HarvestCrop();

                    break; 
                                
            }
            
        }
    }
    public void SwitchSeed(CropController.CropType newseed)// this is for selecting the seed inventory
    {
        seedCropType= newseed;
    }
}