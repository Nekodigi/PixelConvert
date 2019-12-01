using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ResultTwoManager : MonoBehaviour
{
    Texture2D tex;
    Image image;
    Color[] colorSet;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawImage()
    {
        int width = MainManager.width;
        int height = MainManager.height;
        colorSet = MainManager.colorSet;
        tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Point;
        for (int y = 0; y < MainManager.height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                tex.SetPixel(x, y, colorSet[MainManager.FindNearColor(MainManager.frames[MainManager.selectFrame].colors[y * MainManager.width + x])]);

            }
        }
        tex.Apply();
        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
    }

    public void export()
    {
        int width = MainManager.width;
        int height = MainManager.height;
        int frame = MainManager.frame;
        colorSet = MainManager.colorSet;
        int[,] idArray = new int[width * 2, height * 2];
        string strOut = (width).ToString() + "," + (height).ToString() + "," + frame.ToString();
        Debug.Log("Exporting" + width);
        for (int f = 0; f < frame; f++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    strOut += "," + MainManager.FindNearColor(MainManager.frames[f].colors[y * width + x]).ToString();
                }
            }
        }
        Debug.Log(strOut);
        Debug.Log(MainManager.exportPath);
        File.WriteAllText(MainManager.exportPath, strOut);
    }
}
