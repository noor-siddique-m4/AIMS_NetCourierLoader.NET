using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.metafour.aims.services.NCLoader
{
    class Globals
    {
        public const string DEFAULT_DATE = "01 Jan 1900";
        public const string DEFAULT_STRING = "";
        public const int DEFAULT_KEY = -1;
        public const int DEFAULT_NUMBER = 0;
        public const bool DEFAULT_BOOLEAN = false;
        public const int DEFAULT_BIT_FLAG = 0;

        public const char ALLOWED = 'A';
        public const char NOT_ALLOWED = 'N';
        public const char USER_DEFINED = 'U';

        public const string TRACKING = "TRACKING";
        public const string LOCATION = "SPRINT_LHR";

        public static string SMTP_ACCOUNT_NAME = "";
        public static string SMTP_ACCOUNT_PASSWORD = "";
        public static string SMTP_AUTHENTICATION = "";
        public static string SMTP_SERVER = "";
        public static string LOCAL_ADMINISTRATOR_EMAIL = "";

        public static string TRACKING_MESSAGE_ID = "METACS_TRACKING";

        public static string LOG_FILES_PATH = "";

        //    //Timer polling interval
        public static int giPollingInterval = 0;

        public static string IMPORT_DIR;
        public static string PENDING_DIR;
        public static string PROCESSED_DIR;
        public static string METACS_TRACKING_FILE_NAME;
        public static string METACS_DATA_FILE_NAME;

        public static string LOG_FILE_SIZE;
        public static string LOG_FILE_COUNT;


        public static Int32 FIELD_GUID = 0;
        public static Int32 FIELD_CONSIGNMENT = 1;
        public static Int32 FIELD_DATE = 2;
        public static Int32 FIELD_TIME = 3;
        public static Int32 FIELD_CODE = 4;

        public static DateTime dtLastGUIDPurge;

        public enum DataFile
        {
            CustomerAcctCode = 0,
            AWB = 1,
            ShipmentDate = 2,
            CustomerRef = 3,
            AgentAWB = 4,
            AgentName = 5,
            DestinationCity = 6,
            PODDate = 7,
            PODTime = 8,
            PODName = 9,
            Consignee = 10,
            CneeAddr1 = 11,
            CneeAddr2 = 12,
            CneeAddr3 = 13,
            CneeAddr4 = 14,
            Description = 15,
            NOP = 16,
            Weight = 17,
            ServiceType = 18,
            InvoiceAmountUKL = 19,
            Misc1 = 20,
            Misc2 = 21,
            InvoiceNumber = 22,
            InvoiceDate = 23
        }
    }
}
