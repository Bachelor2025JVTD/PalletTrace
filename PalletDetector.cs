using Emgu.CV.Structure;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;

namespace PalletTrace
{

    internal class PalletDetector
    {
        #region Fields
        private int targetWidth;
        private int targetHeight;
        private int tolerance;
        #endregion

        #region Constructor
        public PalletDetector(int targetWidth, int targetHeight, int tolerance)
        {
            TargetHeight = targetHeight;
            TargetWidth = targetWidth;
            Tolerance = tolerance;
        }
        #endregion

        #region Properties
        public int TargetWidth
        {
            get { return targetWidth; }
            set { targetWidth = value; }
        }

        public int TargetHeight
        {
            get { return targetHeight; }
            set { targetHeight = value; }
        }

        public int Tolerance
        {
            get { return tolerance; }
            set { tolerance = value; }
        }
        #endregion

        #region PublicMethods
        /// <summary>
        /// Detects and returns dark pallets on light backgrounds
        /// </summary>
        /// <param name="image"></param>
        /// <param name="excludedAreaHandler"></param>
        /// <returns></returns>
        public RotatedRect? DetectPallet(Mat image, ExcludedAreaHandler excludedAreaHandler)
        {
            Mat grayImage, thresholdImage;
            VectorOfVectorOfPoint contours;
            RotatedRect rotatedRectangle;
            PointF[] vertices;
            Point[] points;
            float width, height;

            grayImage = new Mat();
            thresholdImage = new Mat();
            contours = new VectorOfVectorOfPoint();

            CvInvoke.CvtColor(image, grayImage, ColorConversion.Bgr2Gray);
            CvInvoke.Threshold(grayImage, thresholdImage, 50, 255, ThresholdType.BinaryInv);

            if (excludedAreaHandler.ExcludedRegion.HasValue)
            {
                excludedAreaHandler.ExcludeRectangle(image, thresholdImage, excludedAreaHandler.ShowExcludedRegion);
            }

            CvInvoke.FindContours(thresholdImage, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

            for (int i = 0; i < contours.Size; i++)
            {
                rotatedRectangle = CvInvoke.MinAreaRect(contours[i]);
                width = rotatedRectangle.Size.Width;
                height = rotatedRectangle.Size.Height;

                if ((Math.Abs(width - targetWidth) < tolerance && Math.Abs(height - targetHeight) < tolerance) ||  (Math.Abs(width - targetHeight) < tolerance && Math.Abs(height - targetWidth) < tolerance))
                {
                    vertices = rotatedRectangle.GetVertices();
                    points = Array.ConvertAll(vertices, Point.Round);
                    return rotatedRectangle;
                }
            }
            return null;
        }
        #endregion
    }
}