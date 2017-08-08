using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image ItemImage;
    public void UpdateItem(Sprite s)
    {
        ItemImage.sprite = s;
    }
}
