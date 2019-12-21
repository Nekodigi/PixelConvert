using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonoBehaviourCustom : MonoBehaviour
{


    public IEnumerator DelayFlameEnd(Action action)
    {
        yield return null;
        action();
    }

    public IEnumerator DelayNFlameEnd(int count,Action action)
    {
        for (int i = 0; i < count; i++)
        {
            yield return null;
        }
        action();
    }

    public IEnumerator DelaySeconds(float second, Action action)
    {
        yield return new WaitForSeconds(second);
        action();
    }

    public static void TryAction(Action action, string Message)
    {
        try
        {
            action();
        }
        catch
        {
            if (Message != "")
            {
                Debug.Log(Message);
            }
        }
    }

    public  static void TryAction(Action action)
    {
        TryAction(action, "");
    }

    public string FileNameToExtension(string fileName)
    {
        string[] paths = fileName.Split('.');
        return paths[paths.Length - 1];
    }
    
    public bool FileNameExtensionExists(string fileName)
    {
        string[] paths = fileName.Split('.');
        return paths.Length > 1;
    }

    public void DestroyChild(Transform trans)
    {
        for(int i = 0; i < trans.childCount; i++)
        {
            Destroy(trans.GetChild(i).gameObject);
        }
    }
}

//moreExtension--------------------------------------------------------------

public static partial class StringExtensions
{

    /// <summary>
    /// 指定した文字列がいくつあるか
    /// </summary>
    public static int CountOf(this string self, params string[] strArray)
    {
        int count = 0;

        foreach (string str in strArray)
        {
            if (str != "")
            {
                int index = self.IndexOf(str, 0);
                while (index != -1)
                {
                    count++;
                    index = self.IndexOf(str, index + str.Length);
                }
            }
        }

        return count;
    }

}
