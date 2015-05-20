using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Introduce_To_Algorithm3.Common.Utils.FTP.SERVER
{
    public class FTPUser
    {
        public bool CanDeleteFiles,
            CanDeleteFolders,
            CanRenameFiles,
            CanRenameFolders,
            CanStoreFiles,
            CanStoreFolder,
            CanViewHiddenFiles,
            CanViewHiddenFolders,
            CanCopyFiles;

        public string UserName = "";
        public string StartUpDirectory = "";
        public string CurrentWorkingDirectory = "\\";
        public bool IsAuthenticated = false;
        public string Password;


        public void LoadProfile(string userName)
        {
            try
            {
                if (userName == this.UserName)
                {
                    return;
                }
                this.UserName = userName;
                if (this.UserName.Length == 0)
                {
                    return;
                }
                XmlNodeList Users = ApplicationSettings.GetUserList();
                IsAuthenticated = false;

                foreach (XmlNode User in Users)
                {
                    if (User.Attributes[0].Value != UserName) continue;

                    Password = User.Attributes[1].Value;
                    StartUpDirectory = User.Attributes[2].Value;

                    char[] Permissions = User.Attributes[3].Value.ToCharArray();

                    CanStoreFiles = Permissions[0] == '1';
                    CanStoreFolder = Permissions[1] == '1';
                    CanRenameFiles = Permissions[2] == '1';
                    CanRenameFolders = Permissions[3] == '1';
                    CanDeleteFiles = Permissions[4] == '1';
                    CanDeleteFolders = Permissions[5] == '1';
                    CanCopyFiles = Permissions[6] == '1';
                    CanViewHiddenFiles = Permissions[7] == '1';
                    CanViewHiddenFolders = Permissions[8] == '1';

                    break;
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        public bool Authenticate(string password)
        {
            if (password == this.Password)
            {
                IsAuthenticated = true;
            }
            else
            {
                IsAuthenticated = false;
            }

            return IsAuthenticated;
        }


        public bool ChangeDirectory(string dir)
        {
            CurrentWorkingDirectory = dir;
            return true;
        }



    }
}
