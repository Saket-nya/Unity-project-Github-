using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GrowBlock : MonoBehaviour
{
    // keep track of the current size of the block
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public enum GrowthStage
    {

        barren,
        ploughed,
        planted,
        growing1,
        growing2,
        ripe
    }

    public GrowthStage currentstage;

    public SpriteRenderer theSR;
    public Sprite soilTilled, soilWatered;

    public SpriteRenderer cropSR; // this is the sprite renderer for the crop, we will use this to change the sprite of the crop as it grows.
    public Sprite cropPlanted, cropGrowing1, cropGrowing2, cropRipe; // these are the sprites for the different stages of the crop

    public bool isWatered;  // this is connected to watersoil 

    public bool preventuse;// this is used to prevent the player from using a tool on the block while it is in a transition state.

    private Vector2Int gridPosition; // this is the position of the block in the grid, we will use this to update the grid info when the block changes. conttect to setgridpostion scroll down.

    public CropController.CropType cropType; // this is the type of crop that is planted on this block.
    public float growthFailChance;//This is to save the chance of fail for crops.

    void Start()
    {
        AdvanceStage(); // Start at ploughed

        AdvanceStage(); // Move to planted
    }

 
    // Update is called once per frame
    void Update()
    {
        /* if(Keyboard.current.eKey.wasPressedThisFrame)
         {
            AdvanceStage();

            setSoilSprite();
         }
        */
#if UNITY_EDITOR
        if (Keyboard.current.nKey.wasPressedThisFrame)// you dont want to be left with the game when the player enters the game so i am suing # unity editor only works while making the game it doesnt work.
        {
            AdvancedCrop();
        }
#endif

    }

    public void AdvanceStage()
    {
       // currentstage = currentstage + 1; // Advance to the next growth stage

        if ((int)currentstage >= 6)// Check if the current stage exceeds the last defined stage (ripe)
        {
            currentstage = GrowthStage.barren; // Reset to barren after reaching ripe
        }


    }


    public void SetSoilSprite() // This method can be called to change the sprite to the tilled soil sprite

    {
        if (currentstage == GrowthStage.barren)
        {
            theSR.sprite = null; // Set to no sprite for barren stage
        }
        else
        {
            if (isWatered == true)
            {
                theSR.sprite = soilWatered;
            }
            else
            {
                theSR.sprite = soilTilled; // Set to the tilled soil sprite for all other stages
            }        
        }
        UpdateGridInfo(); // we need this because it will update the grid infromation when ever we change the souldsprite.
    }

    public void PloughSoil()
    {
        if (currentstage == GrowthStage.barren && preventuse==false)
        {
            currentstage = GrowthStage.ploughed; // Change to ploughed stage if currently barren
            SetSoilSprite(); // as of nbw we are not using advance stage to change the sprite, so we need to call SetSoilSprite() here to update the sprite to the tilled soil sprite
                            //note to change that.
        }
    }

    public void WaterSoil()// this was reffered in player tools 
    {
        if (preventuse == false)
        {
            isWatered = true;

            SetSoilSprite(); // this is needed because if is barren or plough we need to know.. it doesnt matter at what stage is it right now...
        }
    }


    public void PlantCrop(CropController.CropType cropToPlant)
    {
        if (currentstage == GrowthStage.ploughed && isWatered== true && preventuse==false)
        {
            currentstage = GrowthStage.planted; // Change to planted stage if currently ploughed

            cropType = cropToPlant;

            growthFailChance= CropController.instance.GetCropInfo(cropType).growthFailChance; // Set the growth fail chance based on the crop info for the planted crop type

            CropController.instance.UseSeed(cropToPlant);

            UpdateCropSprite(); // Update the crop sprite to the planted stage sprite
        }

    }

    public void UpdateCropSprite()
    {
        CropInfo activeCrop = CropController.instance.GetCropInfo(cropType); // Get the crop info for the current crop type

        switch (currentstage)
        {
            case GrowthStage.planted:
                //cropSR.sprite = cropPlanted;
                cropSR.sprite = activeCrop.planted; // Set the crop sprite to the planted stage sprite from the crop info
                break;

            case GrowthStage.growing1:
                //cropSR.sprite = cropGrowing1;
                cropSR.sprite = activeCrop.growStage1;// Set the crop sprite to the growing stage 1 sprite from the crop info and the same for the rest of the stages which tou see

                break;
            case GrowthStage.growing2:
                //cropSR.sprite = cropGrowing2;
                cropSR.sprite = activeCrop.growStage2;
                break;
            case GrowthStage.ripe:
                //cropSR.sprite = cropRipe;
                cropSR.sprite = activeCrop.ripe;
                break;
            
        }
        UpdateGridInfo(); // to change the state when we update this 
    }



    public void AdvancedCrop()

    {

        if (isWatered == true&& preventuse == false)

        {
            if (currentstage == GrowthStage.planted || currentstage == GrowthStage.growing1 || currentstage == GrowthStage.growing2)
            {
                currentstage++; // Advance to the next growth stage if currently planted, growing1,

                isWatered = false;
                SetSoilSprite(); // Update the soil sprite to reflect that it is no longer watered
                UpdateCropSprite(); // So that it shows the correct crop. 
            }
        }
    }

    public void HarvestCrop()
    {
        if (currentstage == GrowthStage.ripe & preventuse == false)
        {
            currentstage = GrowthStage.ploughed; // Reset to ploughed after harvesting
            SetSoilSprite(); // Update the soil sprite to reflect that it is now barren

            cropSR.sprite = null; // Remove the crop sprite after harvesting

            CropController.instance.AddCrop(cropType); // Add the harvested crop to the player's inventory using the CropController
        }
    }

    public void SetGridPosition(int x, int y)
    {
        gridPosition = new Vector2Int(x, y); // Set the grid position based on the provided x and y values
    }

     
    void UpdateGridInfo()
    { 
     GridInfo.instance.UpdateInfo(this, gridPosition.x, gridPosition.y); // Update the grid info with the current state of this block

    }

}



