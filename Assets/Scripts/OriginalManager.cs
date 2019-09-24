using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OriginalManager : MonoBehaviourCustom
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayFlameEnd(() => { setupImage(); }));
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
        for (int y = 0; y < MainManager.height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject pixel = Instantiate(MainManager.pixel, transform, this.gameObject) as GameObject;
                pixel.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetX + x * pixelSize, offsetY + y * pixelSize);
                pixel.GetComponent<RectTransform>().sizeDelta = new Vector2(pixelSize, pixelSize);
                pixel.GetComponent<Image>().color = MainManager.imagePixels[y*MainManager.width+x];

            }
        }
    }
}
