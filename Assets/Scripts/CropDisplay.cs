using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CropDisplay : MonoBehaviour
{
    public CropController.CropType crop;

    public Image CropImage;
    public TMP_Text amountText;

    public void UpdateDisplay()
    {
        CropInfo info = CropController.instance.GetCropInfo(crop);

        CropImage.sprite = info.finalCrop;
        amountText.text = "x" + info.cropAmount;
    }
}
