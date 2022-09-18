using UnityEngine;

namespace PoppyPlaytimeCards.Util
{
    internal class ThemeManager
    {
        private static CardThemeColor.CardThemeColorType _poppyRed;

        public static void LoadThemes()
        {
            _poppyRed = CardThemeLib.CardThemeLib.instance.CreateOrGetType("PoppyRed",
            new CardThemeColor
            {
                bgColor = new Color(0, 0, 0),
                targetColor = new Color(0.9811321f, 0.001f, 0.001f)
            });
        }

        public static CardThemeColor.CardThemeColorType GetRedTheme()
        {
            return _poppyRed;
        }
    }
}
