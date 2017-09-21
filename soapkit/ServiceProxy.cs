using System;
using System.Xml;

namespace soapkit
{
    /// <summary>
    /// Service proxy object used for communicating with the web services and transform response XML to object representations.	
    /// </summary>
    public class ServiceProxy
    {
        /// <summary>
        /// Web Service reference
        /// </summary>
        private DirectoryService.Directory m_Directory = new DirectoryService.Directory();
        // private TracksService.Tracks m_Tracks = new TracksService.Tracks();
        // private GeocoderService.Geocoder m_Geocoder = new GeocoderService.Geocoder();
        private ReportingService.Reporting m_Reporting = new ReportingService.Reporting();

        // Current Session ID
        private string m_strSessionID;

        // Current Application ID
        private int m_iApplicationID;

        // Simgleton Instance of the service Proxy
        private static ServiceProxy m_SimpleService = new ServiceProxy();

        /// <summary>
        /// Get the ServiceProxy object.
        /// </summary>
        /// <returns></returns>
        public static ServiceProxy GetSimpleService()
        {
            return m_SimpleService;
        }

        /// <summary>
        /// Private constructor.
        /// SimpleService is used as a singleton.
        /// </summary>
        private ServiceProxy()
        { }

        #region Directory Service

        /// <summary>
        /// Login.
        /// </summary>
        /// <param name="strUsername">Username</param>
        /// <param name="strPassword">Password</param>
        /// <param name="iApplicationID">ID of the application to logon to.</param>
        /// <returns>SessionID string used in following service calls.</returns>
        public string Login(string strUsername, string strPassword, int iApplicationID)
        {
            // Call the Directory WebService Login.
            XmlNode xmlResponse = m_Directory.Login(strUsername, strPassword, iApplicationID);

            // Check response for errors in response.
            CheckError(xmlResponse);

            // Get the session id returned by GpsGate Server.
            m_strSessionID = xmlResponse.InnerText;

            // Set internal application ID prtoperty.
            m_iApplicationID = iApplicationID;

            return m_strSessionID;
        }

        #endregion

        #region Reporting Service
        /// <summary>
        /// Gets list of reports for selected application
        /// </summary>
        /// <returns>xml document with collection of report names and ids</returns>
        public XmlNode GetReports()
        {
            //call web service for current session and application
            return m_Reporting.GetReports(m_strSessionID, m_iApplicationID);
        }

        /// <summary>
        /// Start Generating report
        /// </summary>
        /// <param name="iReportID">report id</param>
        /// <param name="dtStartDate">start date of report data period</param>
        /// <param name="dtEndDate">end date of report data period</param>
        /// <returns>xml document with handle for report processing</returns>
        public XmlNode GenerateReport(int iReportID, DateTime dtStartDate, DateTime dtEndDate)
        {
            //call web service for current session, appication, report and period
            return m_Reporting.GenerateReport(m_strSessionID, iReportID, dtStartDate, dtEndDate);
        }

        /// <summary>
        /// Get report status <i>this is used to check if report finish processing</i>
        /// </summary>
        /// <param name="iHandleID">Handle id, returned by the generatereport method</param>
        /// <returns>xml document with new handle data</returns>
        public XmlNode GetReportStatus(int iHandleID)
        {
            return m_Reporting.GetReportStatus(m_strSessionID, iHandleID);
        }

        /// <summary>
        /// Fetch report xml document, this method should only be called when handle
        /// from GenerateReport and GetReportStatus has <b>Done</b> status
        /// </summary>
        /// <param name="iHandleID">Handle id, returned by the generatereport method</param>
        /// <returns>xml document with report data and style</returns>
        public XmlNode FetchReport(int iHandleID)
        {
            return m_Reporting.FetchReport(m_strSessionID, iHandleID);
        }

        /// <summary>
        /// gets list of currently processing reports
        /// </summary>
        /// <returns>a xml document with the list of handlers for reports, this
        /// list should typically have only at most 1 element</returns>
        public XmlNode GetProcessingReports()
        {
            return m_Reporting.GetProcessingReports(m_strSessionID, m_iApplicationID);
        }


        /// <summary>
        /// cancel processing report
        /// </summary>
        /// <param name="iHandleID">Handle id, returned by the generatereport method</param>
        /// <returns></returns>
        public XmlNode CancelReport(int iHandleID)
        {
            return m_Reporting.CancelReport(m_strSessionID, iHandleID);
        }

        #endregion

        /// <summary>
        /// Check the response for error.
        /// 
        /// If exception node found. this method throws a new Exception with the error message.
        /// </summary>
        /// <param name="xml"></param>
        private static void CheckError(XmlNode xml)
        {
            if (xml != null && xml.FirstChild != null && xml.FirstChild.Name == "exception")
            {
                throw new Exception(xml.SelectSingleNode("//exception/message").InnerText);
            }
        }
    }
}
