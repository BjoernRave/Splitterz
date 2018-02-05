using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class color : MonoBehaviour
{


    public Color Color1;
    public Color Color2;
    public Color Color3;
    public Color Color4;
    public Color Color5;
    public Color Color6;
    public Color Color7;
    public Color Color8;
    public Color Color9;
    public Color Color10;
    public Color Color11;
    public Color Color12;
    public static Color[] ColorPalette = new Color[12];

    // Use this for initialization
    void Start()
    {
        ColorPalette[0] = Color1;
        ColorPalette[1] = Color2;
        ColorPalette[2] = Color3;
        ColorPalette[3] = Color4;
        ColorPalette[4] = Color5;
        ColorPalette[5] = Color6;
        ColorPalette[6] = Color7;
        ColorPalette[7] = Color8;
        ColorPalette[8] = Color9;
        ColorPalette[9] = Color10;
        ColorPalette[10] = Color11;
        ColorPalette[11] = Color12;

    }
}