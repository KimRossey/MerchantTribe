using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using MerchantTribe.Web;

namespace BVSoftware.Commerce.Utilities
{
    public class ImageProcessing
    {
        private const int TINYWIDTH = 50;
        private const int TINYHEIGHT = 50;
        private const int SMALLWIDTH = 220;
        private const int SMALLHEIGHT = 220;
        private const int MEDIUMWIDTH = 440;
        private const int MEDIUMHEIGHT = 440;
        private const int LOGOWIDTH = 310;
        private const int LOGOHEIGHT = 110;
        private const int BANNERWIDTH = 700;
        private const int BANNERHEIGHT = 400;
        private const int THEMEPREVIEWHEIGHT = 120;
        private const int THEMEPREVIEWWIDTH = 160;
        private const int THEMEPREVIEWBIGHEIGHT = 480;
        private const int THEMEPREVIEWBIGWIDTH = 640;

        private static bool ShrinkImage(string originalFile, string outputDirectory, int maxWidth, int maxHeight)
        {
            bool result = true;

            try
            {
                string pathOfOriginal = Path.GetDirectoryName(originalFile);
                string pathOfOutput = Path.Combine(pathOfOriginal, outputDirectory);
                if (!Directory.Exists(pathOfOutput))
                {
                    Directory.CreateDirectory(pathOfOutput);
                }
                string outputFile = Path.Combine(pathOfOutput, Path.GetFileName(originalFile));
                Images.ShrinkImageFileOnDisk(originalFile, outputFile, maxWidth, maxHeight);
            }
            catch (Exception ex)
            {
                result = false;
                EventLog.LogEvent(ex);
            }

            return result;
        }

        public static bool ShrinkToTiny(string originalFile)
        {
            return ShrinkImage(originalFile, "tiny", TINYWIDTH, TINYHEIGHT);
        }
        public static bool ShrinkToSmall(string originalFile)
        {
            return ShrinkImage(originalFile, "small", SMALLWIDTH, SMALLHEIGHT);
        }
        public static bool ShrinkToMedium(string originalFile)
        {
            return ShrinkImage(originalFile, "medium", MEDIUMWIDTH, MEDIUMHEIGHT);            
        }
        public static bool ShrinkToLogo(string originalFile)
        {
            return ShrinkImage(originalFile, "logo", LOGOWIDTH, LOGOHEIGHT);            
        }
        public static bool ShrinkToBanner(string originalFile)
        {
            return ShrinkImage(originalFile, "banner", BANNERWIDTH, BANNERHEIGHT);            
        }

        public static bool ShrinkToThemePreview(string originalFile)
        {
            return ShrinkImage(originalFile, "", THEMEPREVIEWWIDTH, THEMEPREVIEWHEIGHT);
        }
        public static bool ShrinkToThemePreviewBig(string originalFile)
        {
            return ShrinkImage(originalFile, "", THEMEPREVIEWBIGWIDTH, THEMEPREVIEWBIGHEIGHT);
        }
    }
}
