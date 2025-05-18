using Emgu.CV.Structure;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletTrace
{
    internal class ExcludedAreaHandler
    {
        #region Fields
        private MCvScalar excludedRegionColor;
        private Rectangle? excludeRegion = null;
        private bool showExcludedRegion;
        #endregion

        #region Constructor
        public ExcludedAreaHandler()
        {
            ExcludedRegionColor = new MCvScalar(0, 0, 255);
        }
        #endregion

        #region Properties
        public bool ShowExcludedRegion
        {
            get { return showExcludedRegion; }
            set { showExcludedRegion = value; }
        }

        public Rectangle? ExcludedRegion
        {
            get { return excludeRegion; }
            set { excludeRegion = value; }
        }

        public MCvScalar ExcludedRegionColor
        {
            get { return excludedRegionColor; }
            set { excludedRegionColor = value; }
        }
        #endregion

        #region PublicMethods
        /// <summary>
        /// Turns excluded region Black RGB (0,0,0) with option to show exluded rectangle as a faded red rectangle.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageWithExcludedRectangle"></param>
        /// <param name="showExcludedRectangle"></param>
        public void ExcludeRectangle(Mat image, Mat imageWithExcludedRectangle, bool showExcludedRectangle)
        {
            Mat overlay;
            double opacity;
            CvInvoke.Rectangle(imageWithExcludedRectangle, ExcludedRegion.Value, new MCvScalar(0, 0, 0), -1);
            if (showExcludedRectangle)
            {
                overlay = image.Clone();
                opacity = 0.3;
                CvInvoke.Rectangle(overlay, ExcludedRegion.Value, excludedRegionColor, -1); 
                CvInvoke.AddWeighted(image, 1 - opacity, overlay, opacity, 0, image);
            }
        }
        #endregion
    }
}
