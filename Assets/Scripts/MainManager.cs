using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExtendMath;


public class MainManager : MonoBehaviour
{
    public float whiteOfset;
    public bool testMode;
    public static int colorCount = 16;
    public GameObject inPixel;
    public float inPixelSize;
    public int inWidth;
    public int inHeight;
    public static GameObject pixel;
    public static float pixelSize;
    public static int width;
    public static int height;
    public Color requestColor;
    public Color[] colorPreset = new Color[16];
    public static Color[] colorSet;
    public static Color[,,,] colorEncoded = new Color[16,16,16,16];
    public static Color[] imagePixels;
    public Texture2D tex;
    //public Color[] colorEncodedForTest = new Color[4096];
    Transform canvasTrans;
    Image requestImg;
    Image[] colorSourcesImg = new Image[4];
    Image resultImg;
    Image result2Img;
    OriginalManager originalManager;
    ResultManager resultManager;
    ResultTwoManager resultTwoManager;


    // Start is called before the first frame update
    void Start()
    {
        Defining();
    }

    public static int FindNearColor(Color goalColor)
    {
        float minDiff = Mathf.Infinity;
        int id = 0;
        for (int i = 0; i < MainManager.colorCount; i++)
        {
            float diff = colorSqrtDiff(goalColor, colorSet[i]);
            if (minDiff > diff)
            {
                minDiff = diff;
                id = i;
            }

        }
        return id;
    }

    public static PixelSet FindBetterColor(Color goalColor)
    {
        float minDiff = Mathf.Infinity;
        int colorA = 0;
        int colorB = 0;
        int colorC = 0;
        int colorD = 0;
        for (int a = 0; a< MainManager.colorCount; a++)
        {
            for (int b = 0; b< MainManager.colorCount; b++)
            {
                for (int c = 0; c< MainManager.colorCount; c++)
                {
                    for (int d = 0; d< MainManager.colorCount; d++)
                    {
                        float diff = colorSqrtDiff(goalColor, colorEncoded[a, b, c, d]);
                        if (minDiff > diff)
                        {
                            minDiff = diff;
                            colorA = a;
                            colorB = b;
                            colorC = c;
                            colorD = d;
                        }
                       
                    }
                }
            }
        }
        PixelSet pixelSet = new PixelSet();
        pixelSet.col[0] = colorA;
        pixelSet.col[1] = colorB;
        pixelSet.col[2] = colorC;
        pixelSet.col[3] = colorD;
        return pixelSet;
    }

    void Defining()
    {
        canvasTrans = GameObject.Find("Canvas").transform;
        colorSet = colorPreset;
        for (int a = 0; a < colorCount; a++)
        {
            for (int b = 0; b < colorCount; b++)
            {
                for (int c = 0; c < colorCount; c++)
                {
                    for (int d = 0; d < colorCount; d++)
                    {
                        colorEncoded[a, b, c, d] = avg4color(a, b, c, d);
                    }
                }
            }
        }
        if (!testMode)
        {
            pixel = inPixel;
            pixelSize = inPixelSize;
            width = inWidth;
            height = inHeight;
            imagePixels = tex.GetPixels();
            for (int i = 0; i < width * height; i++)
            {
                imagePixels[i] += new Color(whiteOfset, whiteOfset, whiteOfset);
            }
            originalManager = canvasTrans.Find("Original").GetComponent<OriginalManager>();
            originalManager.setupImage();
            resultManager = canvasTrans.Find("Result").GetComponent<ResultManager>();
            resultManager.setupImage();
            resultTwoManager = canvasTrans.Find("Result2").GetComponent<ResultTwoManager>();
            resultTwoManager.setupImage();

        }
        if (testMode)
        {
            requestImg = canvasTrans.Find("Request").GetComponent<Image>();
            for (int i = 0; i < 4; i++)
            {
                colorSourcesImg[i] = canvasTrans.Find("ColorSource").Find("Slot" + i.ToString()).GetComponent<Image>();
            }
            resultImg = canvasTrans.Find("Result").GetComponent<Image>();
            result2Img = canvasTrans.Find("Result2").GetComponent<Image>();
        }
        /*int id = 0;
        for (int a = 0; a < 8; a++)
        {
            for (int b = 0; b < 8; b++)
            {
                for (int c = 0; c < 8; c++)
                {
                    for (int d = 0; d < 8; d++)
                    {
                        colorEncodedForTest[id] = colorEncoded[a, b, c, d];
                        id++;
                    }
                }
            }
        }*/
    }

    void showColorSources(int a, int b, int c, int d)
    {
        colorSourcesImg[0].color = colorPreset[a];
        colorSourcesImg[1].color = colorPreset[b];
        colorSourcesImg[2].color = colorPreset[c];
        colorSourcesImg[3].color = colorPreset[d];
    }

    public static float colorSqrtDiff(Color a, Color b)
    {
        return Basic.sqrt(a.r - b.r) + Basic.sqrt(a.g - b.g) + Basic.sqrt(a.b - b.b);
    }

    Color avg4color(int a, int b, int c, int d)
    {
        return (colorPreset[a] + colorPreset[b] + colorPreset[c] + colorPreset[d]) / 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (testMode)
        {
            requestImg.color = requestColor;
            PixelSet pixelSet = FindBetterColor(requestColor);
            showColorSources(pixelSet.col[0], pixelSet.col[1], pixelSet.col[2], pixelSet.col[3]);
            resultImg.color = avg4color(pixelSet.col[0], pixelSet.col[1], pixelSet.col[2], pixelSet.col[3]);
            result2Img.color = colorSet[FindNearColor(requestColor)];
        }
    }
}

public class PixelSet
{
    public int[] col = new int[4];
}