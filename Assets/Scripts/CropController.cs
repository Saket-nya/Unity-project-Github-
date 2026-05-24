using System.Collections.Generic;
using UnityEngine;

public class CropController : MonoBehaviour
{
    public static CropController instance;// this is the crop controller that will be used to keep track of the crops and their growth stages.
    void Awake()
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
    public enum CropType

    {
        pumpkin,
        lettuce,
        carrot,
        hay,
        potato,
        strawberry,
        tomato,
        avocado

    }


    public List<CropInfo> cropList = new List<CropInfo>(); // this is the list that will hold the information about the crops and their growth stages.

    public CropInfo GetCropInfo(CropType cropToGet) // this is the function that will return the crop info for a specific crop type, we will use this to get the sprites for the different growth stages of the crops.
    {
        int position = -1;

        for (int i = 0; i < cropList.Count; i++)
        {

            if (cropList[i].cropType == cropToGet)
            {
                position = i;

            }

        }
        if (position >= 0)
        {
            return cropList[position];
        }
        else
        {
            return null;
        }
    }
    public void UseSeed(CropType seedToUse)

    { 
        foreach(CropInfo info in cropList)
        {
            if (info.cropType == seedToUse)
            {
                info.seedAmount--;
            
            }

        }
    }

    public void AddCrop(CropType cropToAdd)
    {
        foreach (CropInfo info in cropList)
        {
            if (info.cropType == cropToAdd)
            {
                info.cropAmount++;

            }

        }
    }
}



[System.Serializable]
public class CropInfo
{
    public CropController.CropType cropType;
    public Sprite finalCrop, seedType, planted, growStage1, growStage2, ripe;

    public int seedAmount, cropAmount;

    [Range(0f, 100f)]
    public float growthFailChance;

    
}