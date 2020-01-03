using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class ResultManager : MonoBehaviour
{
    public bool enableFloydSteinberg;
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
        int tLen = MainManager.frames[MainManager.selectFrame].colors.Length;
        Color[] thisFrameColors = new Color[tLen];
        Array.Copy(MainManager.frames[MainManager.selectFrame].colors, thisFrameColors, tLen);
        tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Point;
        if (enableFloydSteinberg)
        {
            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    Color originalCol = constrain(thisFrameColors[y * MainManager.width + x]);
                    Color newCol = colorSet[MainManager.FindNearColor(originalCol)];
                    thisFrameColors[y * MainManager.width + x] = newCol;
                    Color diffCol = originalCol - newCol;
                    thisFrameColors[y * MainManager.width + x + 1] += diffCol * 7.0f / 16.0f;
                    thisFrameColors[(y + 1) * MainManager.width + x - 1] += diffCol * 3.0f / 16.0f;
                    thisFrameColors[(y + 1) * MainManager.width + x] += diffCol * 5.0f / 16.0f;
                    thisFrameColors[(y + 1) * MainManager.width + x + 1] += diffCol * 1.0f / 16.0f;
                    tex.SetPixel(x, y, newCol);
                }
            }
        }
        else
        {
            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    tex.SetPixel(x, y, colorSet[MainManager.FindNearColor(thisFrameColors[y * MainManager.width + x])]);

                }
            }
        }
        tex.Apply();
        if(image == null)
        {
            image = GetComponent<Image>();
        }
        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
    }

    public Color constrain(Color color)
    {
        return new Color(Mathf.Clamp(color.r, 0, 1), Mathf.Clamp(color.g, 0, 1), Mathf.Clamp(color.b, 0, 1), 1);
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
        if (enableFloydSteinberg)
        {
            for (int f = 0; f < frame; f++)
            {
                int tLen = MainManager.frames[f].colors.Length;
                Color[] thisFrameColors = new Color[tLen];
                Array.Copy(MainManager.frames[f].colors, thisFrameColors, tLen);
                for (int y = 0; y < height - 1; y++)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        Color originalCol = constrain(thisFrameColors[y * MainManager.width + x]);
                        Color newCol = colorSet[MainManager.FindNearColor(originalCol)];
                        thisFrameColors[y * MainManager.width + x] = newCol;
                        Color diffCol = originalCol - newCol;
                        thisFrameColors[y * MainManager.width + x + 1] += diffCol * 7.0f / 16.0f;
                        thisFrameColors[(y + 1) * MainManager.width + x - 1] += diffCol * 3.0f / 16.0f;
                        thisFrameColors[(y + 1) * MainManager.width + x] += diffCol * 5.0f / 16.0f;
                        thisFrameColors[(y + 1) * MainManager.width + x + 1] += diffCol * 1.0f / 16.0f;
                        tex.SetPixel(x, y, newCol);
                        strOut += "," + MainManager.FindNearColor(originalCol).ToString();
                    }
                }
            }
        }
        else
        {
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
        }
        Debug.Log(strOut);
        Debug.Log(MainManager.exportPath);
        File.WriteAllText(MainManager.exportPath, strOut);
    }
}
