using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Utils {

    public static class UnityHelper
    {

        // Convert System.Color to Unity.Ui.Color by argb uint hex
        public static Color FromSysColor(uint argb)
        {

            float r, g, b, a;
            var color = new Color();

            a = (argb >> 24) & 0xFF;
            r = (argb >> 16) & 0xFF;
            g = (argb >> 8)  & 0xFF;
            b = (argb >> 0)  & 0xFF;

            color.r = r / 255;
            color.g = g / 255;
            color.b = b / 255;
            color.a = a / 255;

            return color;
        }
    }
}

