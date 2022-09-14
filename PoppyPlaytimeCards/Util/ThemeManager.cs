using UnityEngine;

namespace PoppyPlaytimeCards.Util
{
    internal class ThemeManager
    {
        public static void LoadThemes()
        {
            // poppyRed = 
            // poppyBlue = CardThemeLib.CardThemeLib.instance.CreateOrGetType("PoppyBlue",
            //     new CardThemeColor
            //     {
            //         bgColor = new Color(0, 0, 0),
            //         targetColor = new Color(0, 0.333f, 1)
            //     });
        }

        public static CardThemeColor.CardThemeColorType GetRedTheme()
        {
            return CardThemeLib.CardThemeLib.instance.CreateOrGetType("PoppyRed",
                new CardThemeColor
                {
                    bgColor = new Color(0, 0, 0),
                    targetColor = new Color(0.9811321f, 0.001f, 0.001f)
                });
        }
    }
}
