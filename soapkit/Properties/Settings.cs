using System;
namespace soapkit.Properties
{
    public static class Settings
    {
        static public string host = "http://54.169.39.186/";
		static public string service_Directory = host + "GpsGateServer/Services/Directory.asmx";
		static public string service_Reporting = host + "GpsGateServer/Services/Reporting.asmx";
        static public string username = "Tester";
        static public string password = "Test123";
        static public int appId = 4;
        static public string report_Path = @"Reports.csv"; 
	}
}
