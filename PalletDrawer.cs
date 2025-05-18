using Emgu.CV.Structure;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.CvEnum;
using System.Drawing;

namespace PalletTrace
{
    internal class PalletDrawer 
    {
        #region Fields
        private MCvScalar borderColor;
        private MCvScalar gridColor;
        private MCvScalar serialNumberColor;
        private bool showPalletDimentions;
        #endregion

        #region Constructor
        public PalletDrawer()
        {
            BorderColor = new MCvScalar(0, 0, 255);
            SerialNumberColor = new MCvScalar(0, 255, 255);
            GridColor = new MCvScalar(0, 255, 0);
            DimentionTextColor = new MCvScalar(255, 255, 255);
        }
        #endregion

        #region Properties
        public bool ShowPalletDimensions
        {
            get { return showPalletDimentions; }
            set { showPalletDimentions = value; }
        }

        public MCvScalar BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; }
        }

        public MCvScalar SerialNumberColor
        {
            get { return serialNumberColor; }
            set { serialNumberColor = value; }
        }

        public MCvScalar GridColor
        {
            get { return gridColor; }
            set { gridColor = value; }
        }
        private MCvScalar dimentionTextColor;

        public MCvScalar DimentionTextColor
        {
            get { return dimentionTextColor; }
            set { dimentionTextColor = value; }
        }
        #endregion

        #region PublicMethods
        /// <summary>
        /// Draws rotated grid with serialnumbers on detected pallet
        /// </summary>
        /// <param name="image"></param>
        /// <param name="rotatedRectangle"></param>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <param name="serialNumbers"></param>
        /// <param name="orientation"></param>
        public void DrawRotatedGridWithSerialNumbers(Mat image, RotatedRect rotatedRectangle, int rows, int columns, string[] serialNumbers, string orientation)
        {
            PointF[] orderedCorners;
            PointF corner0, corner1, corner3, vectorA, vectorB, vectorX, vectorY;
            float lengthVectorA, lengthVectorB, gridStepX, gridStepY;

            orderedCorners = GetOrderedCorners(rotatedRectangle);
            corner0 = orderedCorners[0];
            corner1 = orderedCorners[1];
            corner3 = orderedCorners[3];

            vectorA = new PointF(corner1.X - corner0.X, corner1.Y - corner0.Y);
            vectorB = new PointF(corner3.X - corner0.X, corner3.Y - corner0.Y);
            lengthVectorA = (float)Math.Sqrt(Math.Pow(vectorA.X, 2) + Math.Pow(vectorA.Y, 2));
            lengthVectorB = (float)Math.Sqrt(Math.Pow(vectorB.X, 2) + Math.Pow(vectorB.Y, 2));

            if (showPalletDimentions)
            {
                DrawPalletDimentions(image, vectorA, vectorB, corner0, lengthVectorA, lengthVectorB);
            }

            if (lengthVectorA > lengthVectorB)
            {
                vectorX = new PointF(vectorA.X / lengthVectorA, vectorA.Y / lengthVectorA);
                vectorY = new PointF(vectorB.X / lengthVectorB, vectorB.Y / lengthVectorB);
                gridStepX = lengthVectorA / columns;
                gridStepY = lengthVectorB / rows;
            }
            else
            {
                vectorX = new PointF(vectorB.X / lengthVectorB, vectorB.Y / lengthVectorB);
                vectorY = new PointF(vectorA.X / lengthVectorA, vectorA.Y / lengthVectorA);
                gridStepX = lengthVectorB / columns;
                gridStepY = lengthVectorA / rows;
            }
            DrawGrid(image, rows, columns, corner0, vectorX, vectorY, gridStepX, gridStepY);
            DrawRotatedRectangle(image, orderedCorners);
            DrawSerialNumbers(image, columns, rows, corner0, vectorX, vectorY, gridStepX, gridStepY, serialNumbers);
        }

        /// <summary>
        /// Draws a rotated grid on detected pallet.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="rotatedRectangle"></param>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        public void DrawRotatedGrid(Mat image, RotatedRect rotatedRectangle, int rows, int columns)
        {
            PointF[] corners;
            PointF corner0, corner1, corner3, vectorA, vectorB, vectorX, vectorY;
            float lengthVectorA, lengthVectorB, gridStepX, gridStepY;

            corners = rotatedRectangle.GetVertices();

            corner0 = corners[0];
            corner1 = corners[1];
            corner3 = corners[3];

            // Lag vektorer for sidene
            vectorA = new PointF(corner1.X - corner0.X, corner1.Y - corner0.Y); // side A (p0 -> p1)
            vectorB = new PointF(corner3.X - corner0.X, corner3.Y - corner0.Y); // side B (p0 -> p3)

            // Beregn lengde
            lengthVectorA = (float)Math.Sqrt(Math.Pow(vectorA.X, 2) + Math.Pow(vectorA.Y, 2));
            lengthVectorB = (float)Math.Sqrt(Math.Pow(vectorB.X, 2) + Math.Pow(vectorB.Y, 2));

            // Bestem X/Y-retning basert på lengste side = X-retning
            if (lengthVectorA > lengthVectorB)
            {
                vectorX = new PointF(vectorA.X / lengthVectorA, vectorA.Y / lengthVectorA);
                vectorY = new PointF(vectorB.X / lengthVectorB, vectorB.Y / lengthVectorB);
                gridStepX = lengthVectorA / columns;
                gridStepY = lengthVectorB / rows;
            }
            else
            {
                vectorX = new PointF(vectorB.X / lengthVectorB, vectorB.Y / lengthVectorB);
                vectorY = new PointF(vectorA.X / lengthVectorA, vectorA.Y / lengthVectorA);
                gridStepX = lengthVectorB / columns;
                gridStepY = lengthVectorA / rows;
            }
            DrawGrid(image, rows, columns, corner0, vectorX, vectorY, gridStepX, gridStepY);
            DrawRotatedRectangle(image, corners);
            if (ShowPalletDimensions)
            {
                DrawPalletDimentions(image, vectorA, vectorB, corner0, lengthVectorA, lengthVectorB);
            }
        }
        #endregion

        #region PrivateMethods
        /// <summary>
        /// Returns corners of rotated rectangle and sets top left corner index=0
        /// </summary>
        /// <param name="rotatedRectangle"></param>
        /// <returns></returns>
        private PointF[] GetOrderedCorners(RotatedRect rotatedRectangle)
        {
            PointF[] corners, ordered;
            int topLeftIndex;
            float minSum, sum;

            corners = rotatedRectangle.GetVertices();
            ordered = new PointF[4];
            topLeftIndex = 0;
            minSum = corners[0].X + corners[0].Y;

            for (int i = 1; i < 4; i++)
            {
                sum = corners[i].X + corners[i].Y;
                if (sum < minSum)
                {
                    minSum = sum;
                    topLeftIndex = i;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                ordered[i] = corners[(topLeftIndex + i) % 4];
            }
            return ordered;
        }

        /// <summary>
        /// Outlines rectangle in red
        /// </summary>
        /// <param name="image"></param>
        /// <param name="rectangleCorners"></param>
        private void DrawRotatedRectangle(Mat image, PointF[] rectangleCorners)
        {
            Point[] outline = Array.ConvertAll(rectangleCorners, Point.Round);
            CvInvoke.Polylines(image, outline, true, borderColor, 2);
        }

        /// <summary>
        /// Draws grid based on calculated points and vectors
        /// </summary>
        /// <param name="image"></param>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <param name="corner0"></param>
        /// <param name="vectorX"></param>
        /// <param name="vectorY"></param>
        /// <param name="gridStepX"></param>
        /// <param name="gridStepY"></param>
        private void DrawGrid(Mat image, int rows, int columns, PointF corner0, PointF vectorX, PointF vectorY, float gridStepX, float gridStepY)
        {
            PointF LineStart, LineEnd;
            // Tegn rader
            for (int r = 1; r < rows; r++)
            {
                LineStart = new PointF(corner0.X + vectorY.X * gridStepY * r, corner0.Y + vectorY.Y * gridStepY * r);
                LineEnd = new PointF(LineStart.X + vectorX.X * gridStepX * columns, LineStart.Y + vectorX.Y * gridStepX * columns);
                CvInvoke.Line(image, Point.Round(LineStart), Point.Round(LineEnd), new MCvScalar(0, 255, 0), 1);
            }
            // Tegn kolonner
            for (int c = 1; c < columns; c++)
            {
                LineStart = new PointF(corner0.X + vectorX.X * gridStepX * c, corner0.Y + vectorX.Y * gridStepX * c);
                LineEnd = new PointF(LineStart.X + vectorY.X * gridStepY * rows, LineStart.Y + vectorY.Y * gridStepY * rows);
                CvInvoke.Line(image, Point.Round(LineStart), Point.Round(LineEnd), new MCvScalar(0, 255, 0), 1);
            }
        }

        /// <summary>
        /// Draws Serialnumbers on rectangle
        /// </summary>
        /// <param name="image"></param>
        /// <param name="columns"></param>
        /// <param name="rows"></param>
        /// <param name="corner0"></param>
        /// <param name="vectorX"></param>
        /// <param name="vectorY"></param>
        /// <param name="gridStepX"></param>
        /// <param name="gridStepY"></param>
        /// <param name="serialNumbers"></param>
        private void DrawSerialNumbers(Mat image, int columns, int rows, PointF corner0, PointF vectorX, PointF vectorY, float gridStepX, float gridStepY, string[] serialNumbers)
        {
            PointF cellTopLeft, cellCenter;
            Point center, textOrigin;
            Size textSize;
            int totalCells, gridIndexNumber, thickness, baseline;
            double fontScale;
            string serial;

            totalCells = columns * rows;
            gridIndexNumber = 0;
            thickness = 1;
            fontScale = 0.4;
            baseline = 0;


            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    cellTopLeft = new PointF(corner0.X + vectorX.X * gridStepX * c + vectorY.X * gridStepY * r, corner0.Y + vectorX.Y * gridStepX * c + vectorY.Y * gridStepY * r);

                    cellCenter = new PointF(cellTopLeft.X + (vectorX.X * gridStepX + vectorY.X * gridStepY) / 2, cellTopLeft.Y + (vectorX.Y * gridStepX + vectorY.Y * gridStepY) / 2);
                    serial = serialNumbers[gridIndexNumber];
                    center = Point.Round(cellCenter);
                    textSize = CvInvoke.GetTextSize(serial, FontFace.HersheySimplex, fontScale, thickness, ref baseline);
                    textOrigin = new Point(center.X - textSize.Width / 2, center.Y + textSize.Height / 2);

                    CvInvoke.PutText(image, serial, textOrigin, FontFace.HersheySimplex, fontScale, serialNumberColor, thickness);

                    gridIndexNumber++;
                }
            }
        }

        /// <summary>
        /// Writes rectangle height and length om image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="vectorA"></param>
        /// <param name="vectorB"></param>
        /// <param name="corner0"></param>
        /// <param name="lengthVectorA"></param>
        /// <param name="lengthVectorB"></param>
        private void DrawPalletDimentions(Mat image, PointF vectorA, PointF vectorB, PointF corner0, float lengthVectorA, float lengthVectorB)
        {
            PointF normA, normB, midA, midB;
            Point textPointA, textPointB;
            string textA, textB;
            double fontScale = 0.5;
            int thickness = 1;
            // Normaliser vektorene
            normA = new PointF(vectorA.X / lengthVectorA, vectorA.Y / lengthVectorA);
            normB = new PointF(vectorB.X / lengthVectorB, vectorB.Y / lengthVectorB);

            // Beregn midtpunkt for tekst
            midA = new PointF(corner0.X + normA.X * (lengthVectorA / 2), corner0.Y + normA.Y * (lengthVectorA / 2));
            midB = new PointF(corner0.X + normB.X * (lengthVectorB / 2), corner0.Y + normB.Y * (lengthVectorB / 2));
            textPointA = Point.Round(new PointF(midA.X - 15, midA.Y - 10));
            textPointB = Point.Round(new PointF(midB.X - 40, midB.Y));

            textA = lengthVectorA.ToString("0");
            textB = lengthVectorB.ToString("0");

            CvInvoke.PutText(image, textA, textPointA, FontFace.HersheySimplex, fontScale, dimentionTextColor, thickness);
            CvInvoke.PutText(image, textB, textPointB, FontFace.HersheySimplex, fontScale, dimentionTextColor, thickness);
        }
        #endregion
    }
}