using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IconMaker : MonoBehaviour
{
    int EmptyIconSlotIndex = 0;
    public List<Image> Icons;
    public List<Camera> cams;

    public Sprite GetIcon(Camera cam)
    {
        int resX = cam.pixelWidth;
        int resY = cam.pixelHeight;

        int clipX = 0;
        int clipY = 0;
        if (resX > resY)
            clipX = resX - resY;
        else if (resY > resX)
            clipY = resY - resX;

        Texture2D tex = new Texture2D(resX - clipX, resY - clipY, TextureFormat.RGB24, false);
        RenderTexture rt = new RenderTexture(resX, resY, 24);
        cam.targetTexture = rt;
        RenderTexture.active = rt;

        cam.Render();

        tex.ReadPixels(new Rect(clipX/2, clipY/2, resX, resY), 0, 0);
        tex.Apply();

        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector3(0,0));
    }

    private void Start()
    {
        for(int i = 0; i < Icons.Count; i++)
        {
            Icons[i].sprite = GetIcon(cams[i]);
        }
    }

    public void AddIcon(GameObject newItem)
    {
        if (EmptyIconSlotIndex > Icons.Count - 1)
            return;

        newItem.transform.position = new Vector3(cams[EmptyIconSlotIndex].transform.position.x - .3f, cams[EmptyIconSlotIndex].transform.position.y, cams[EmptyIconSlotIndex].transform.position.z + 2);

        Icons[EmptyIconSlotIndex].sprite = GetIcon(cams[EmptyIconSlotIndex]);
        EmptyIconSlotIndex++;
    }

    /// <summary>
    /// If we remove an item from the inventory, update the corresponding image in the inventory.
    /// </summary>
    /// <param name="item"></param>
    public void RemoveIcon(GameObject item)
    {
        for(int i = 0; i < Icons.Count; i++)
        {
            float diffX = Mathf.Abs(cams[i].transform.position.x - item.transform.position.x);
            float diffY = Mathf.Abs(cams[i].transform.position.y - item.transform.position.y);
            if(diffX < 1 && diffY < 1)
            {
                Icons[i].sprite = GetIcon(cams[i]);
                EmptyIconSlotIndex--;
                return;
            }
        }
    }
}
