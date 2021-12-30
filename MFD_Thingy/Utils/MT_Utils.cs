using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MFD_Thingy
{
    public static class MT_Utils
    {
        public const int G_DefaultItemSpacing = 10;
        public const int G_DefaultItemMinWidth = 120;
        public const int G_DefaultHoldTime = 0;
        public static Color G_DefaultScreenBackground = Colors.Black;
        public static Color G_DefaultBackground = Colors.Black;
        public static Color G_DefaultForground = Colors.WhiteSmoke;

        public static string _ToJson(object P_Object)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(P_Object);
        }


        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        public static bool _IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }


        public static int _GetDefaultHoldTime(string P_Holdtime)
        {
            if (string.IsNullOrEmpty(P_Holdtime) == true)
            {
                return G_DefaultHoldTime;
            }

            return Convert.ToInt32(P_Holdtime);
        }


        public static int _GetDefaultMinWidth(string P_Width)
        {
            if (string.IsNullOrEmpty(P_Width) == true)
            {
                return G_DefaultItemMinWidth;
            }

            return Convert.ToInt32(P_Width);
        }

        public static int _GetDefaultSpacing(string P_Spacing)
        {
            if ( string.IsNullOrEmpty(P_Spacing) == true)
            {
                return G_DefaultItemSpacing;
            }

            return Convert.ToInt32(P_Spacing);
        }


        public static int _GetDefaultMinWidth(int? P_Width)
        {
            if (P_Width == null)
            {
                return G_DefaultItemMinWidth;
            }

            return P_Width.Value;
        }

        public static int _GetDefaultSpacing(int? P_Spacing)
        {
            if(P_Spacing == null)
            {
                return G_DefaultItemSpacing;
            }

            return P_Spacing.Value;
        }

        public static string _ConvertColorToString(Color P_Color)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}", P_Color.R, P_Color.G, P_Color.B);
        }

        public static Color _ConvertStringToColor(String hex, Color P_Default)
        {
            if(string.IsNullOrEmpty(hex) == true)
            {
                return P_Default;
            }
            //remove the # at the front
            hex = hex.Replace("#", "");

            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;

            int start = 0;

            //handle ARGB strings (8 characters long)
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                start = 2;
            }

            //convert RGB characters to bytes
            r = byte.Parse(hex.Substring(start, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(start + 2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(start + 4, 2), System.Globalization.NumberStyles.HexNumber);

            return System.Windows.Media.Color.FromArgb(a, r, g, b);
        }
    }
}
