using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridInfo : MonoBehaviour
{
    public static GridInfo instance; //because we need this to be alwasys active

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool hasGrid; // using info uproppa kod från gridcontroller script
    public List<InfoRow> theGrid = new List<InfoRow>(); // this gonna be showed in unity inspector and this is the list that will hold the information from grid controller script.

    public void CreateGrid()// we are calling this methiód after grid is setup
    {
        hasGrid = true;

        for (int y = 0; y < GridController.instance.blockRows.Count; y++)
        {
            theGrid.Add(new InfoRow());
            for (int x = 0; x < GridController.instance.blockRows[y].blocks.Count; x++)
            {
                theGrid[y].blocks.Add(new BlockInfo());
            }
        }
    }

    public void UpdateInfo(GrowBlock theBlock, int xPos, int yPos)
    {
        theGrid[yPos].blocks[xPos].currentStage = theBlock.currentstage;
        theGrid[yPos].blocks[xPos].isWatered = theBlock.isWatered;
        theGrid[yPos].blocks[xPos].cropType = theBlock.cropType;
        theGrid[yPos].blocks[xPos].growthFailChance = theBlock.growthFailChance;
    }

    public void GrowCrop() // make the plants to change progress like from stage 1 to 2 etc. by using the list. 
    {
        for (int y = 0; y < theGrid.Count; y++)
        {
            for (int x = 0; x < theGrid[y].blocks.Count; x++)
            {
                if (theGrid[y].blocks[x].isWatered == true) // this will check if the block is watered before advancing the growth stage.

                {
                    float growthFailTest = Random.Range(0f, 100f);
                    if (growthFailTest > theGrid[y].blocks[x].growthFailChance)
                    {
                        switch (theGrid[y].blocks[x].currentStage)
                        {

                            case GrowBlock.GrowthStage.planted:
                                theGrid[y].blocks[x].currentStage = GrowBlock.GrowthStage.growing1;
                                break;
                            case GrowBlock.GrowthStage.growing1:
                                theGrid[y].blocks[x].currentStage = GrowBlock.GrowthStage.growing2;
                                break;
                            case GrowBlock.GrowthStage.growing2:
                                theGrid[y].blocks[x].currentStage = GrowBlock.GrowthStage.ripe;
                                break;
                        }

                    }
                    theGrid[y].blocks[x].isWatered = false; // after growing the crop, we set it to not watered, so it needs to be watered again for the next growth stage.
                }
                if (theGrid[y].blocks[x].currentStage==GrowBlock.GrowthStage.ploughed)
                {
                    theGrid[y].blocks[x].currentStage = GrowBlock.GrowthStage.barren;
                }
            }

        }
    }

    //    private void Update()// this was to test the grow crop. 
    //{
    //    if (Keyboard.current.yKey.wasPressedThisFrame)
    //    {
    //        GrowCrop();
    //    }
    //}
}

[System.Serializable]
public class BlockInfo
{
    public bool isWatered;
    public GrowBlock.GrowthStage currentStage; // is replacement of growth blocj script.

    public CropController.CropType cropType;
    public float growthFailChance;
}

[System.Serializable] // this is a class that will hold the information about each row of the grid.
public class InfoRow
{
    public List<BlockInfo> blocks = new List<BlockInfo>();
}