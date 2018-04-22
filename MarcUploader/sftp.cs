using Renci.SshNet;
using System.IO;

namespace MarcUploader
{
    class sftp
    {
        public static void UploadSFTPFile(string host, string username,
        string password, string sourcefile, string destinationpath, int port)
        {
            using (SftpClient client = new SftpClient(host, port, username, password))
            {
                client.Connect();
                client.ChangeDirectory("upload/");
                using (FileStream fs = new FileStream(sourcefile, FileMode.Open))
                {
                    client.BufferSize = 4 * 1024;
                    client.UploadFile(fs, Path.GetFileName(sourcefile));
                }
            }
        }
    }
}