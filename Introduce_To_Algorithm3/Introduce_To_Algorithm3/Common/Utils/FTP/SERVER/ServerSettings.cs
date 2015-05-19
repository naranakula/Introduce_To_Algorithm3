using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Introduce_To_Algorithm3.Common.Utils.FTP.SERVER
{
    public enum SettingsKey
    {
        MAX_PASSV_PORT,
        MIN_PASSV_PORT,
        FTP_PORT,
        AUTO_START_FTP,
        ENABLE_FTP_LOGGING,
        HTTP_PORT,
        AUTO_START_HTTP,
        HTTP_LOGIN_ID,
        HTTP_PASSWORD,
        ENABLE_NOTIFY_ICON,
        ENABLE_FTPFOLDER_ICON,
        ENABLE_QUICK_CONFIG_MENU,
        AUTO_SEND_ERROR_REPORT,
        ENABLE_APD,
        MOVE_FILES_TO_RECYCLE_BIN,
        DATE_TIME_FORMAT
    }

    public class ApplicationSettings
    {
        #region declerations

        public static XmlDocument Settings;
        public static FTPServer FtpServer;


        #endregion
    }
}
