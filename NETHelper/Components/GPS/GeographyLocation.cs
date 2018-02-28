using GenjiCore.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore.Components.GPS
{
    public static class GeographyLocation
    {
        private static string uri = "http://maps.google.com/maps/api/geocode/xml?";
        public static Coordinates GetAddress(double latitude, double longitude)
        {
            string str = string.Format("latlng={0}&sensor=false", (object)(latitude.ToString() + "," + (object)longitude));
            Stream responseStream = WebRequest.Create(GeographyLocation.uri + str).GetResponse().GetResponseStream();
            DataSet dataSet = new DataSet();
            int num = (int)dataSet.ReadXml(responseStream);
            return GeographyLocation.GetCoordinates(dataSet);
        }

        public static Coordinates GetCoordinates(string address)
        {
            address = Uri.EscapeDataString(address);
            string str = string.Format("address={0}&sensor=false", (object)address);
            Stream responseStream = WebRequest.Create(GeographyLocation.uri + str).GetResponse().GetResponseStream();
            DataSet dataSet = new DataSet();
            int num = (int)dataSet.ReadXml(responseStream);
            return GeographyLocation.GetCoordinates(dataSet);
        }

        private static Coordinates GetCoordinates(DataSet dataSet)
        {
            Coordinates coordinates = new Coordinates();
            if (dataSet != null)
            {
                if (dataSet.Tables.Contains("location"))
                {
                    DataTable table = dataSet.Tables["location"];
                    string str1 = "lat";
                    string str2 = "lng";
                    if (table != null && table.Columns.Contains(str1) && table.Columns.Contains(str2))
                    {
                        foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
                        {
                            if (!row.IsNull(str1) && !row.IsNull(str2))
                            {
                                coordinates.Latitude = row.Field<string>(str1).TryGetValue<double>();
                                coordinates.Longitude = row.Field<string>(str2).TryGetValue<double>();
                                break;
                            }
                        }
                    }
                }
                if (dataSet.Tables.Contains("result"))
                {
                    DataTable table = dataSet.Tables["result"];
                    string str = "formatted_address";
                    if (table != null && table.Columns.Contains(str))
                    {
                        foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
                        {
                            if (!row.IsNull(str))
                            {
                                coordinates.Address = row.Field<string>(str);
                                break;
                            }
                        }
                    }
                }
            }
            return coordinates;
        }

        public static double GetDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            double num1 = false ? 3960.0 : 6371.0;
            double radians1 = (latitude2 - latitude1).ToRadians();
            double radians2 = (longitude2 - longitude1).ToRadians();
            double num2 = 2.0 * Math.Asin(Math.Min(1.0, Math.Sqrt(Math.Sin(radians1 / 2.0) * Math.Sin(radians1 / 2.0) + Math.Cos(latitude1.ToRadians()) * Math.Cos(latitude2.ToRadians()) * Math.Sin(radians2 / 2.0) * Math.Sin(radians2 / 2.0))));
            return num1 * num2;
        }

        public static double ToRadians(this double value)
        {
            return Math.PI / 180.0 * value;
        }
    }
}
