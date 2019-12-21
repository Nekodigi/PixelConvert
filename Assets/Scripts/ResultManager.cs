using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ResultManager : MonoBehaviour
{
    public bool enable4cdiff;
    Texture2D tex;
    Color[] colorSet;
    Image image;
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
        tex = new Texture2D(width*2, height*2, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Point;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                PixelSet pixelSet = new PixelSet();
                if (enable4cdiff)
                {
                    pixelSet = MainManager.FourCdiff(MainManager.frames[MainManager.selectFrame].colors[y * MainManager.width + x]);
                }
                else
                {
                    pixelSet = MainManager.FindBetterColor(MainManager.frames[MainManager.selectFrame].colors[y * MainManager.width + x]);
                }
                tex.SetPixel(x * 2, y * 2, colorSet[pixelSet.col[0]]);
                set4pixel(x, y, pixelSet);
            }
        }
        tex.Apply();
        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
    }

    public void set4pixel(int x, int y, PixelSet pixelSet)
    {
        tex.SetPixel(x * 2, y * 2, colorSet[pixelSet.col[0]]);
        tex.SetPixel(x * 2 + 1, y * 2, colorSet[pixelSet.col[1]]);
        tex.SetPixel(x * 2, y * 2 + 1, colorSet[pixelSet.col[2]]);
        tex.SetPixel(x * 2 + 1, y * 2 + 1, colorSet[pixelSet.col[3]]);
    }

    public void export()
    {
        int width = MainManager.width;
        int height = MainManager.height;
        int frame = MainManager.frame;
        colorSet = MainManager.colorSet;
        int[,] idArray = new int[width * 2, height * 2];
        string strOut = (width * 2).ToString() + "," + (height * 2).ToString() + "," + frame.ToString();
        Debug.Log("Exporting"+width);
        for (int f = 0; f < frame; f++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    PixelSet pixelSet = new PixelSet();
                    if (enable4cdiff)
                    {
                        pixelSet = MainManager.FourCdiff(MainManager.frames[f].colors[y * MainManager.width + x]);
                    }
                    else
                    {
                        pixelSet = MainManager.FindBetterColor(MainManager.frames[f].colors[y * MainManager.width + x]);
                    }
                    idArray[x * 2, y * 2] = pixelSet.col[0];
                    idArray[x * 2, y * 2 + 1] = pixelSet.col[1];
                    idArray[x * 2 + 1, y * 2] = pixelSet.col[2];
                    idArray[x * 2 + 1, y * 2 + 1] = pixelSet.col[3];
                }
            }
            for (int y = 0; y < height * 2; y++)
            {
                for (int x = 0; x < width * 2; x++)
                {
                    strOut += "," + idArray[x, y].ToString();
                }
            }
        }
        Debug.Log(strOut);
        Debug.Log(MainManager.exportPath);
        File.WriteAllText(MainManager.exportPath, strOut);
    }
}
