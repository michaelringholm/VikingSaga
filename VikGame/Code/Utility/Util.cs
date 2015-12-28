using System;
using System.IO;
using System.Windows;
using Vik.Code.Controls.Utility;

namespace Vik.Code.Utility
{
    public static class Util
    {
        // Does at least minimumPct of src overlap dst?
        public static bool Overlap(Rect src, Rect dst, double minimumPct = 0.2)
        {
            double srcArea = src.Height * src.Width;
            src.Intersect(dst);
            double overlapArea = src.Width * src.Height;
            if (overlapArea <= 0)
                return false;

            double overlapPct = overlapArea / srcArea;
            if (double.IsInfinity(overlapPct))
                return false;

            return (overlapPct > minimumPct);
        }

        public static string GetStoreFileForProfile(string profileName)
        {
            return Path.Combine(Util.GetFolderForLocalData(), "SaveGame_" + profileName + ".txt");
        }

        public static string GetFolderForLocalData()
        {
            string localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            localPath = Path.Combine(localPath, "VikGame");

            if (!Directory.Exists(localPath))
                Directory.CreateDirectory(localPath);

            return localPath;
        }

        public static void ErrorPopup(string caption, string message, params object[] param)
        {
            string formattedMessage = string.Format(message, param);
            UiUtil.VikMessageBox(formattedMessage, VikMessageBoxButtons.OK);
        }
    }
}
