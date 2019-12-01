using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExtendMath;
using UnityEditor;


public class MainManager : MonoBehaviourCustom
{
    public float whiteOfset;
    public bool testMode;
    public bool enableR1;//
    public bool enableR2;//
    public bool use4cdiff;
    public static int colorCount = 16;
    public int inWidth;
    public int inHeight;
    public int inFrame;
    public static int selectFrame = 0;
    public static int width;
    public static int height;
    public static int frame;
    public Color requestColor;
    public Color[] colorPreset = new Color[16];
    public static Color[] colorSet;
    public static Color[,,,] colorEncoded = new Color[16,16,16,16];
    public static Frame[] frames;
    public Texture2D[] tex;
    public string inExportPath;
    public static string exportPath;
    //public Color[] colorEncodedForTest = new Color[4096];
    Transform canvasTrans;
    Image requestImg;
    Image[,] colorSourcesImg = new Image[2,4];
    Image[] resultImg = new Image[3];
    OriginalManager originalManager;
    ResultManager[] resultManager = new ResultManager[2];
    ResultTwoManager resultTwoManager;


    // Start is called before the first frame update
    void Start()
    {
        Defining();
    }

    public static int FindNearColor(Color goalColor, float div)
    {
        float minDiff = Mathf.Infinity;
        int id = 0;
        for (int i = 0; i < MainManager.colorCount; i++)
        {
            float diff = colorSqrtDiff(goalColor, colorSet[i]/div);
            if (minDiff > diff)
            {
                minDiff = diff;
                id = i;
            }

        }
        return id;
    }

    public static int FindNearColor(Color goalColor)
    {
        return FindNearColor(goalColor, 1);
    }

    

    

    public static PixelSet FourCdiff(Color goalColor)
    {
        PixelSet pixelSet = new PixelSet();
        for (int i = 0; i < 4; i++)
        {
            pixelSet.col[i] = FindNearColor(goalColor, 4);
            goalColor -= colorSet[pixelSet.col[i]]/4;
        }
        return pixelSet;
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
        exportPath = inExportPath;
        if (enableR1)
        {
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
        }
        if (!testMode)
        {
            frame = tex.Length;
            for (int i = 0; i < frame; i++)
            {
                SetTextureImporterFormat(tex[i], true);
            }
            width = tex[0].width;
            height = tex[0].height;
            frames = new Frame[frame];
            for (int a = 0; a < frame; a++)
            {
                frames[a] = new Frame();
                frames[a].colors = new Color[width * height];
                frames[a].colors = tex[a].GetPixels();
                for (int i = 0; i < width * height; i++)
                {
                    frames[a].colors[i] += new Color(whiteOfset, whiteOfset, whiteOfset);
                }
            }
            originalManager = canvasTrans.Find("Original").GetComponent<OriginalManager>();
            originalManager.DrawImage();
            if (use4cdiff)
            {
                resultManager[0] = canvasTrans.Find("Result0").GetComponent<ResultManager>();
                //StartCoroutine(DelayFlameEnd(() => { resultManager.DrawImage(); }));
            }
            if (enableR1)
            {
                resultManager[1] = canvasTrans.Find("Result1").GetComponent<ResultManager>();
                //StartCoroutine(DelayFlameEnd(() => { resultManager.DrawImage(); }));
            }
            resultTwoManager = canvasTrans.Find("Result2").GetComponent<ResultTwoManager>();
            resultTwoManager.DrawImage();

        }
        if (testMode)
        {
            requestImg = canvasTrans.Find("Request").GetComponent<Image>();
            for (int n = 0; n < 2; n++)
            {
                for (int i = 0; i < 4; i++)
                {
                    colorSourcesImg[n, i] = canvasTrans.Find("ColorSource " + n.ToString()).Find("Slot" + i.ToString()).GetComponent<Image>();
                }
            }
            for (int i = 0; i < 3; i++)
            {
                resultImg[i] = canvasTrans.Find("Result"+i.ToString()).GetComponent<Image>();
            }
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

    void showColorSources(int n, int a, int b, int c, int d)
    {
        colorSourcesImg[n, 0].color = colorPreset[a];
        colorSourcesImg[n, 1].color = colorPreset[b];
        colorSourcesImg[n, 2].color = colorPreset[c];
        colorSourcesImg[n, 3].color = colorPreset[d];
    }

    public static float colorSqrtDiff(Color a, Color b)
    {
        return Basic.sqrt(a.r - b.r) + Basic.sqrt(a.g - b.g) + Basic.sqrt(a.b - b.b);
    }

    Color avg4color(int a, int b, int c, int d)
    {
        return (colorPreset[a] + colorPreset[b] + colorPreset[c] + colorPreset[d]) / 4;
    }

    Color avg4color(PixelSet pixelSet)
    {
        return avg4color(pixelSet.col[0], pixelSet.col[1], pixelSet.col[2], pixelSet.col[3]);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (testMode)
        {
            PixelSet pixelSet1 = FourCdiff(requestColor);
            PixelSet pixelSet2 = FindBetterColor(requestColor);
            requestImg.color = requestColor;
            showColorSources(0, pixelSet1.col[0], pixelSet1.col[1], pixelSet1.col[2], pixelSet1.col[3]);
            showColorSources(1, pixelSet2.col[0], pixelSet2.col[1], pixelSet2.col[2], pixelSet2.col[3]);
            resultImg[0].color = avg4color(pixelSet1);
            resultImg[1].color = avg4color(pixelSet2);
            resultImg[2].color = colorSet[FindNearColor(requestColor)];
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectFrame++;
            if (selectFrame >= frame)
            {
                selectFrame = 0;
            }
            originalManager.DrawImage();
            resultTwoManager.DrawImage();
        }
        if (enableR1 && Input.GetKeyDown(KeyCode.R))
        {
            resultManager[1].DrawImage();
        }
        if (use4cdiff && Input.GetKeyDown(KeyCode.R))
        {
            resultManager[0].DrawImage();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (use4cdiff)
            {
                resultManager[0].export();
            }
            else if (enableR1)
            {
                resultManager[1].export();
            }
            else if (enableR2)
            {
                resultTwoManager.export();
            }
        }
    }

    public static void SetTextureImporterFormat(Texture2D texture, bool isReadable)
    {
        if (null == texture) return;

        string assetPath = AssetDatabase.GetAssetPath(texture);
        var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (tImporter != null)
        {
            tImporter.textureType = TextureImporterType.Default;

            tImporter.isReadable = isReadable;
            AssetDatabase.ImportAsset(assetPath);
            AssetDatabase.Refresh();
        }
    }
}

public class PixelSet
{
    public int[] col = new int[4];
}

public class Frame
{
    public Color[] colors;
}