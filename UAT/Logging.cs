using System;
using System.IO;
using System.Text;

namespace UAT
{
    public static class Logging
    {
        public static void WriteLog(string userName, string machineName, string dateTime, string activity)
        {
            string message = BuildCsvData(userName, machineName, dateTime, activity);
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\UAT.csv", true);
                sw.WriteLine(message);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void WriteLog(string message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\UAT.txt", true);
                sw.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + ": " + message);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string BuildHeaderCsvFile()
        {
            StringBuilder sb = new StringBuilder();
            //add CSV file header
            sb.Append("USER NAME");
            sb.Append(",");
            sb.Append("MACHINE NAME");
            sb.Append(",");
            sb.Append("DATETIME");
            sb.Append(",");
            sb.Append("ACTIVITY");
            sb.AppendLine();
            return sb.ToString();
        }

        /// <summary>
        /// Export to CSV file
        /// Only CSV comma delimited files are supported 
        /// </summary>
        private static string BuildCsvData(string userName, string machineName, string dateTime, string activity)
        {
            string csvData = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append(userName);
            sb.Append(",");
            sb.Append(machineName);
            sb.Append(",");
            sb.Append(dateTime);
            sb.Append(",");
            sb.Append(activity);
            csvData = sb.ToString();
            return csvData;
        }
    }
}
