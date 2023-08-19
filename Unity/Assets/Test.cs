using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [Button]
    private void Keyboard()
    {
        Clear();
        Parpare();
        var keys = GameObject.FindObjectsByType<Key>(FindObjectsSortMode.None);

        foreach (Key key in keys)
        {
            key.Image.ColorAlpha(key.Rate / maxRate);
        }
    }

    public Image template;
    public Transform content;

    [Button]
    private void 柱状图()
    {
        Clear();
        Parpare();
        var keys = GameObject.FindObjectsByType<Key>(FindObjectsSortMode.None).ToList();
        keys.Sort((k1, k2) => k2.Rate.CompareTo(k1.Rate));
        foreach (Key key in keys)
        {
            var img = Instantiate(template, content);
            img.rectTransform.SetSizeHeight(key.Rate / maxRate * 800);
            img.transform.Find("name").GetComponent<Text>().text = key.KeyCode;
            img.transform.Find("rate").GetComponent<Text>().text = (key.Rate * 100).ToString("0.00");
        }
    }

    private float maxRate;

    private void Parpare()
    {
        var keys = GameObject.FindObjectsByType<Key>(FindObjectsSortMode.None);
        Dictionary<string, Key> keyDic = new Dictionary<string, Key>();
        foreach (Key key in keys)
        {
            keyDic[key.KeyCode.ToLower()] = key;
        }

        var lines = File.ReadAllLines(@"C:\Users\admin\Desktop\klingv1.4_win64\app.log");
        int totalWords = 0;
        foreach (string line in lines)
        {
            var newLine = Regex.Replace(line, @"\[.*?\]", "");
            var words = newLine.Split(" ");
            foreach (string word in words)
            {
                if (string.IsNullOrEmpty(word))
                {
                    continue;
                }

                if (word == "+")
                {
                    continue;
                }

                if (keyDic.TryGetValue(word.ToLower(), out var key))
                {
                    key.Count++;
                    totalWords++;
                }
                else
                {
                    print($"找不到{word}");
                }
            }
        }

        maxRate = 0;
        foreach (Key k in keys)
        {
            k.Rate = k.Count * 1.0f / totalWords;
            if (k.Rate > maxRate)
            {
                maxRate = k.Rate;
            }
        }
    }

    [Button]
    public void Clear()
    {
        var keys = GameObject.FindObjectsByType<Key>(FindObjectsSortMode.None);
        foreach (Key key in keys)
        {
            key.Count = 0;
        }
    }

}