using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class GridController : MonoBehaviour
{
    public static GridController instance;// to get info of grid and this to get the pos of grid to make it relavent. 
    public void Awake()
    {
        instance = this;
    }

    public Transform minpoint, maxpoint; // these are the minimum and maximum points for the grid. they will be used to calculate the size of the grid and the position of the grid cells.
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GrowBlock baseGridBlock; // this is the base grid block that will be used to instantiate the grid cells. this is a prefab that we will create unity.

    private Vector2Int gridSize; // this is the size of the grid in terms of the number of cells in the x and y directions.
   
    public List<BlockRow> blockRows = new List<BlockRow>();

    public LayerMask gridBlockers; // this is the layer mask that will be used to check for blockers when placing th

    //public List<GrowBlock> Blocks = new List<GrowBlock>();// we dont want to display this. 
    void Start()
    {
        GenrateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenrateGrid()
    {
        minpoint.position= new Vector3(Mathf.Round(minpoint.position.x), Mathf.Round(minpoint.position.y),0f);
        maxpoint.position = new Vector3(Mathf.Round(maxpoint.position.x), Mathf.Round(maxpoint.position.y), 0f);

        Vector3 starpoint = minpoint.position + new Vector3 (.5f, .5f, 0f); // this is the code that calculates the starting point for the grid.

        // Instantiate(baseGridBlock, starpoint, Quaternion.identity); // vector 3 will not work bxoz it all the things ar on 0 in xyz axis// this will crate a copy of the block ch

        gridSize = new Vector2Int(Mathf.RoundToInt(maxpoint.position.x - minpoint.position.x),
            Mathf.RoundToInt(maxpoint.position.y - minpoint.position.y  )); // this is gonna take away the x value from min an max point example; Min x value - max x value.
       
     for(int y = 0; y< gridSize.y; y++)// this loop is for to establish and create the grid
        {
             blockRows.Add(new BlockRow()); // this will add a new block row to the list of block rows.
             
            for (int x = 0; x < gridSize.x; x++)
            {
                GrowBlock newBlock = Instantiate(baseGridBlock, starpoint + new Vector3(x, y, 0f), Quaternion.identity);

                newBlock.transform.SetParent(transform);
                newBlock.theSR.sprite = null; // this will make the block invisible in the scene.

                newBlock.SetGridPosition(x,y);


                blockRows[y].blocks.Add(newBlock);

                if (Physics2D.OverlapBox(newBlock.transform.position, new Vector2(.9f, .9f), 0f, gridBlockers)) // this will check if there is a blocker in the way of the block being placed. it will check for a box collider with a size of .9 by .9 and a rotation of 0 degrees on the grid blockers layer.
                {
                    newBlock.theSR.sprite= null; // this will remove the blocks from the things that use fysiks like house, pound etc.
                    newBlock.preventuse = true;
                }

                if (GridInfo.instance.hasGrid == true) // we will destroy the grid and create a new one with the same info if there is already a grid in the scene.wil help to save inf
                {
                    BlockInfo storedblock = GridInfo.instance.theGrid[y].blocks[x];

                    newBlock.currentstage = storedblock.currentStage;
                    newBlock.isWatered = storedblock.isWatered;
                    newBlock.cropType =storedblock.cropType;
                    newBlock.growthFailChance = storedblock.growthFailChance;

                    newBlock.SetSoilSprite();
                        newBlock.UpdateCropSprite();


                }
            }

        }


        if(GridInfo.instance.hasGrid==false)

        {
            GridInfo.instance.CreateGrid();
        }
       baseGridBlock.gameObject.SetActive(false); // this will make the base grid block invisible in the scene, we only need it to instantiate the other blocks, 
     
    }

    public GrowBlock GetBlock(float x, float y )
    {
        x = Mathf.RoundToInt(x);
        y = Mathf.RoundToInt(y);

        x -= minpoint.position.x;
        y -= minpoint.position.y;
        

        int intx = Mathf.RoundToInt(x);
        int inty = Mathf.RoundToInt(y);

        if(intx < gridSize.x &&  inty < gridSize.y)
        {
            return blockRows[inty].blocks[intx];
        }

        return null;
    
    }

}
[System.Serializable]
public class BlockRow
{

    public List<GrowBlock> blocks = new List<GrowBlock>();
}
