using System;
using System.IO;
using System.Text;

namespace com.metafour.aims.services.NCLoader
{
    class Logger
    {
        private const string DEFAULT_LOG_FILE_NAME = "ErrorLog.txt";
        private const int DEFAULT_LOG_FILE_SIZE = 40;
        private const string DEFAULT_NO_FILES = "30";

        private string sLogFileName = "ApplicationLog";
        const string LOG_FILE_SUFFIX = ".html";

        //private Int32 DEFAULT_LOG_FILE_SIZE = 1024 * 40;
        private Int32 DEFAULT_MAX_NO_FILES = 10;

        #region Singleton

        private static Logger instance;

        public static Logger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Logger();
                }
                return instance;
            }
        }

        #endregion

        #region Properties

        private string m_sClassName;
        private Int32 m_lClassErrorId;
        private string m_sLogFileName;
        private Int32 m_lLogFileSize;
        private Int32 m_lNumberOfFiles;
        private bool m_bRaiseAlerts;

        public string LogFileName
        {
            get
            {
                return m_sLogFileName;
            }
            set
            {
                m_sLogFileName = value;
            }
        }

        public Int32 LogFileSize
        {
            get
            {
                return m_lLogFileSize;
            }
            set
            {
                m_lLogFileSize = value;
            }
        }

        public Int32 NumberOfFiles
        {
            get
            {
                return m_lNumberOfFiles;
            }
            set
            {
                m_lNumberOfFiles = value;
            }
        }

        public Int32 ClassErrorId
        {
            get
            {
                return m_lClassErrorId;
            }
            set
            {
                m_lClassErrorId = value;
            }
        }

        public string ClassName
        {
            get
            {
                return m_sClassName;
            }
            set
            {
                m_sClassName = value;
            }
        }

        #endregion

        public Logger()
        {
            //Me.Globals.LOG_FILES_PATH = Path.GetDirectoryName(sLogFileName)
            //Me.sLogFileName = Path.GetFileNameWithoutExtension(sLogFileName)
        }

        public string WriteToFileWithHorizontalLine(string sMessage)
        {
            sMessage = sMessage + "<hr />";
            WriteToFile(sMessage + "<hr />");
            return sMessage;
        }

        public string AddColourMarkup(string sMessage, string sColour)
        {
            return ("<font color='" + (sColour + ("'>" + (sMessage + "</font>"))));
        }

        public string WriteToFileColour(string sMessage, string sColour)
        {
            sMessage = AddColourMarkup(sMessage, sColour);
            WriteToFile(AddColourMarkup(sMessage, sColour));
            return sMessage;
        }

        public string AddBoldMarkup(string sMessage)
        {
            return ("<b>" + (sMessage + "</b>"));
        }

        public string WriteToFileBold(string sMessage)
        {
            sMessage = AddBoldMarkup(sMessage);
            WriteToFile(sMessage);
            return sMessage;
        }

        public string WriteToFileBoldColour(string sMessage, string sColour)
        {
            sMessage = AddColourMarkup(AddBoldMarkup(sMessage), sColour);
            WriteToFile(sMessage);
            return sMessage;
        }

        public string AddUpsizeMarkup(string sMessage)
        {
            return ("<font size='+1'>" + (sMessage + "</font>"));
        }

        public string WriteToFileUpsize(string sMessage)
        {
            sMessage = AddUpsizeMarkup(sMessage);
            WriteToFile(AddUpsizeMarkup(sMessage));
            return sMessage;
        }

        public void WriteToFile(string sMessage)
        {
            FileStream oFileStream = default(FileStream);
            StreamWriter oTextWriter = default(StreamWriter);

            string sFileHeader = null;
            string sFileFooter = null;

            string sCurrentFileName = null;
            string sArchiveFileName = null;
            Int32 nFileSize = default(Int32);
            Int32 nFileCount = default(Int32);

            if (sMessage != string.Empty)
            {
                sMessage = "<font color=gray>" + DateTime.Now.ToString("dd-MMM HH:mm:ss.ff") + "</font>&nbsp;" + sMessage;
            }

            sFileHeader = ("<html><head><title>Log</title></head><body style='font-size: xx-small; font-family: Verdana'>" + "\r" + "\r\n");
            sFileFooter = ("</body></html>");

            Directory.CreateDirectory(Globals.LOG_FILES_PATH);
            // fails gracefully when directory already exists

            sCurrentFileName = Globals.LOG_FILES_PATH + "\\" + sLogFileName + LOG_FILE_SUFFIX;

            if (!File.Exists(sCurrentFileName))
            {
                oFileStream = File.Create(sCurrentFileName);
                oTextWriter = new StreamWriter(oFileStream, Encoding.UTF8);
                oTextWriter.WriteLine(sFileHeader + sMessage);
                oTextWriter.Close();
            }
            else
            {
                oFileStream = File.Open(sCurrentFileName, FileMode.Append);
                oTextWriter = new StreamWriter(oFileStream, Encoding.UTF8);
                oTextWriter.WriteLine("<br>");
                oTextWriter.WriteLine(sMessage);
                oTextWriter.Close();
            }

            nFileSize = (int)new FileInfo(sCurrentFileName).Length;
            if (nFileSize > Convert.ToInt32(Globals.LOG_FILE_SIZE))
            {
                oFileStream = File.Open(sCurrentFileName, FileMode.Append);
                oTextWriter = new StreamWriter(oFileStream, Encoding.UTF8);
                oTextWriter.WriteLine(sFileFooter);
                oTextWriter.Close();

                string[] sarrFiles = Directory.GetFiles(Globals.LOG_FILES_PATH, sLogFileName + "#*" + LOG_FILE_SUFFIX);
                nFileCount = sarrFiles.Length;
                sArchiveFileName = Globals.LOG_FILES_PATH + "\\" + sLogFileName + "#" + nFileCount.ToString() + LOG_FILE_SUFFIX;

                if (nFileCount >= Convert.ToInt32(Globals.LOG_FILE_COUNT))
                {
                    File.Delete(sArchiveFileName);
                    nFileCount = nFileCount - 1;
                }

                for (Int32 nFileNumber = nFileCount; nFileNumber >= 1; nFileNumber += -1)
                {
                    sArchiveFileName = Globals.LOG_FILES_PATH + "\\" + sLogFileName + "#" + (nFileNumber).ToString() + LOG_FILE_SUFFIX;
                    string sNewArchiveFilename = Globals.LOG_FILES_PATH + "\\" + sLogFileName + "#" + (nFileNumber + 1).ToString() + LOG_FILE_SUFFIX;
                    File.Move(sArchiveFileName, sNewArchiveFilename);
                }
                File.Move(sCurrentFileName, Globals.LOG_FILES_PATH + "\\" + sLogFileName + "#1" + LOG_FILE_SUFFIX);
                oFileStream = File.Create(sCurrentFileName);
                oTextWriter = new StreamWriter(oFileStream, Encoding.UTF8);
                oTextWriter.WriteLine(sFileHeader + "Log file started " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + "<br><br>");
                oTextWriter.Close();
            }
        }
    }
}
