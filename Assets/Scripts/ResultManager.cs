using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setupImage()
    {
        int width = MainManager.width;
        int height = MainManager.height;
        float pixelSize = MainManager.pixelSize;
        float offsetX = -width * pixelSize / 2;
        float offsetY = -width * pixelSize / 2;
        Color[] colorSet = MainManager.colorSet;
        int[,] idArray = new int[width*2,height*2];
        string strOut = (width * 2).ToString() + "," + (height * 2).ToString();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                PixelSet pixelSet = MainManager.FindBetterColor(MainManager.imagePixels[y * MainManager.width + x]);
                makePixel(offsetX, offsetY, x, y, 0, 0, pixelSize, colorSet[pixelSet.col[0]]);
                makePixel(offsetX, offsetY, x, y, 0, 1, pixelSize, colorSet[pixelSet.col[1]]);
                makePixel(offsetX, offsetY, x, y, 1, 0, pixelSize, colorSet[pixelSet.col[2]]);
                makePixel(offsetX, offsetY, x, y, 1, 1, pixelSize, colorSet[pixelSet.col[3]]);
                idArray[x * 2, y * 2] = pixelSet.col[0];
                idArray[x * 2, y * 2 + 1] = pixelSet.col[1];
                idArray[x * 2 + 1, y * 2] = pixelSet.col[2];
                idArray[x * 2 + 1, y * 2 + 1] = pixelSet.col[3];
            }
        }
        for (int y = 0; y < height*2; y++)
        {
            for (int x = 0; x < width*2; x++)
            {
                strOut += "," + idArray[x, y].ToString();
            }
        }
        Debug.Log(strOut);
    }

    void makePixel(float offsetX, float offsetY, int x, int y, float xp, float yp, float pixelSize, Color color)
    {
        GameObject pixel = Instantiate(MainManager.pixel, transform, this.gameObject) as GameObject;
        pixel.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetX + (x+xp/2) * pixelSize, offsetY + (y+yp/2) * pixelSize);
        pixel.GetComponent<RectTransform>().sizeDelta = new Vector2(pixelSize/2, pixelSize/2);
        pixel.GetComponent<Image>().color = color;
    }
}
