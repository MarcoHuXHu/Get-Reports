using System;
using System.Xml;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace soapkit
{
    class MainClass
    {
        public static void WriteToExcel(List<Dictionary<string, string>> data)
        {
            string filePath = Properties.Settings.report_Path;
            string delimiter = ",";
            string[] line = {"ID", "Name"};
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(delimiter, line));

            foreach (Dictionary<string, string> row in data) 
            {
                line[0] = row["id"]; line[1] = row["name"];
				sb.AppendLine(string.Join(delimiter, line));
            }
            File.WriteAllText(filePath, sb.ToString()); 
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("start...");
			Console.WriteLine("getting reports...");
            ServiceProxy service = ServiceProxy.GetSimpleService();
			// Call Login and get session ID.
            string strSessionID = service.Login(
                Properties.Settings.username, Properties.Settings.password, Properties.Settings.appId);
			// Get Reports
			XmlNode reports = service.GetReports();

            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
			foreach (XmlElement child in reports.ChildNodes)
			{
                Dictionary<string, string> line = new Dictionary<string, string>();
                foreach (XmlElement element in child.ChildNodes)
                    line.Add(element.Name.ToLower(), element.InnerText);
                data.Add(line);
			}
            data.Sort((x, y) => (x["id"].Length-y["id"].Length)*2 + String.Compare(x["id"], y["id"]));
			Console.WriteLine("saving reports...");
			// Write to CSV
			WriteToExcel(data);

            Console.WriteLine("end!");
        }
    }
}