using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace CSharp.Script.Utils
{
    public static class DirectoryUtils
    {
        public static bool TrySetDirectoryFullControl(string path)
        {
            try
            {
                SetDirectoryFullControl(path);
                return true;
            }
            catch 
            {
                return false;
            }
        }

        /// <summary>
        /// set Everyone to full control this directory
        /// </summary>
        /// <param name="path"></param>
        public static void SetDirectoryFullControl(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
                return;

            var dSecurity = directoryInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));

            directoryInfo.SetAccessControl(dSecurity);
        }
    }
}
