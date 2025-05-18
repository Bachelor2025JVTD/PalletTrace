using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletTrace
{
    internal class Pallet
    {
        #region Fields
        private string orientation;
        private int palletID;
        private string[] products;
        #endregion

        #region Constructor
        public Pallet()
        {
            products = new string[6];
        }
        #endregion

        #region Properties
        public string Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        public int PalletID
        {
            get { return palletID; }
            set { palletID = value; }
        }

        public string[] Products
        {
            get { return products; }
            set { products = value; }
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when an unknown RFID-tag is scanned
        /// </summary>
        public event EventHandler UnknownRfidTag;
        #endregion

        #region PublicMethods
        /// <summary>
        /// Loads Products[6] with products corresponding with PalletId.
        /// If there is no matching PalletId, Products[6] set to null.
        /// </summary>
        /// <param name="palletRFIDTag"></param>
        public void GetPalletAndOrientationWithProducts(string palletRFIDTag)
        {
            DataBaseHandler dataBaseHandler;
            try
            {
                dataBaseHandler = new DataBaseHandler();
                dataBaseHandler.RunStoredProcedure("GetPalletByRFID", "@RFIDTag", palletRFIDTag, out DataTable values);

                if (values.Rows.Count > 0)
                {
                    DataRow row = values.Rows[0];
                    if (row.ItemArray[0].ToString() == "Ikke funnet")
                    {
                        for (int i = 0; i < products.Length; i++)
                        {
                            products[i] = null;
                        }
                        UnknownRfidTag?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        PalletID = Convert.ToInt32(row["PalletID"]);
                        orientation = row["Orientation"].ToString();
                        GetProducts(dataBaseHandler, palletID);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Feil: " + ex.Message);
            }
        }
        #endregion

        #region PrivateMethods
        /// Loads products into Pallet-object
        private void GetProducts(DataBaseHandler dataBaseHandler, int palletId)
        {
            int position;
            string serialNumber;
            try
            {
                dataBaseHandler.RunStoredProcedure("GetProductsByPallet", "@PalletID", palletId, out DataTable values);
                foreach (DataRow product in values.Rows)
                {
                    position = Convert.ToInt32(product["Position"]) - 1;
                    serialNumber = product["SerialNo"].ToString();
                    if (serialNumber == null)
                    {
                        serialNumber = "(tom)";
                    }
                    if (orientation == "RFIDFront")
                    {
                        Products[position] = serialNumber;
                    }
                    else
                    {
                        Products[(Products.Length-1) - position] = serialNumber;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}