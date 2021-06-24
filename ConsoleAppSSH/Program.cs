using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Renci.SshNet;

namespace ConsoleAppSSH
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var number = 489;
            var password = "secret";

            var host = $"webfacesearchgpu{number}.westeurope.cloudapp.azure.com";
            var username = "dave";

            //var connectionInfo = new ConnectionInfo("sftp.foo.com",
            //    "guest",
            //    new PasswordAuthenticationMethod("guest", "pwd"),
            //    new PrivateKeyAuthenticationMethod("rsa.key"));

            //using (var client = new SftpClient(connectionInfo))
            //{
            //    client.Connect();
            //}

            // 1.run a command and get stdout back
            //using var client = new SshClient(host, username, password);
            //client.Connect();
            //// stderr is &2
            //var cmd = client.CreateCommand("echo 12345; echo 654321 >&2");
            //var result = cmd.Execute();
            //Console.Write($"stdout: {result}");

            //var reader = new StreamReader(cmd.ExtendedOutputStream);
            //var stderr = reader.ReadToEnd();
            //Console.Write($"stderr: {stderr}");

            //client.Disconnect();

            // 2.how to stream the stdout?
            // ShellStream class
            // subscribe to DataReceived event?
            //using var client = new SshClient(host, username, password);
            //client.Connect();
            //using var shellStream = client.CreateShellStream("Tail", 0, 0, 0, 0, 1024);
            //shellStream.DataReceived += ShellDataReceived;

            //var prompt = $"dave@osrfacesearchgpu{number}vm:~$";
            //var promptFS = $"dave@osrfacesearchgpu{number}vm:~/facesearch$";
            //// make sure the prompt is there - regex not working yet
            ////var output = shellStream.Expect(new Regex(@"[$>]"));
            //var output = shellStream.Expect(prompt);

            //shellStream.WriteLine("cd facesearch");
            //shellStream.Expect(promptFS);

            //shellStream.WriteLine("./facesearch.py");
            //shellStream.Expect(promptFS);

            //client.Disconnect();

            //// testing stdout and stderr (both are displayed)
            ////shellStream.WriteLine("echo 12345; echo 654321 >&2");

            //static void ShellDataReceived(object sender, Renci.SshNet.Common.ShellDataEventArgs e)
            //{
            //    // this always writes sent data to console
            //    Console.Write(Encoding.UTF8.GetString(e.Data));
            //}

            // 3.SFTP
            // upload files
            //var localFilePath = @"c:\temp\test.txt";
            //var fileName = "lots-of-images.zip";
            //var localFilePath = $@"c:\temp\{fileName}";
            ////var remoteFilePath = "/tmp/test.txt";
            //var remoteFilePath = $@"/home/dave/facesearch/facesearch_cloud/{fileName}";
            //using var client = new SftpClient(host, username, password);
            //try
            //{
            //    client.Connect();
            //    using var s = File.OpenRead(localFilePath);
            //    // there is an Async BeginUploadFile
            //    // explore https://github.com/sshnet/SSH.NET/tree/develop/src/Renci.SshNet.Tests/Classes
            //    client.UploadFile(s, remoteFilePath);

            //    var scriptFileName = "extractFiles.sh";
            //    var scriptLocalFilePath = $@"c:\temp\{scriptFileName}";
            //    var scriptRemotePath = $@"/home/dave/facesearch/facesearch_cloud/{scriptFileName}";
            //    using var t = File.OpenRead(scriptLocalFilePath);
            //    client.UploadFile(t, scriptRemotePath);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}
            //finally
            //{
            //    client.Disconnect();
            //}

            //// download
            using var sftp = new SftpClient(host, username, password);
            try
            {
                sftp.Connect();

                var remoteDirectory = "/tmp";

                // view remote files
                var files = sftp.ListDirectory(remoteDirectory);
                foreach (var file in files)
                {
                    Console.WriteLine(file.Name);
                }

                // download file from remote
                string pathRemoteFile = "/tmp/myScript.txt";
                // Path where the file should be saved once downloaded (locally)
                string pathLocalFile = @"c:\temp\myScript.txt";

                using (Stream fileStream = File.OpenWrite(pathLocalFile))
                {
                    sftp.DownloadFile(pathRemoteFile, fileStream);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An exception has been caught " + e.ToString());
            }
            finally
            {
                sftp.Disconnect();
            }
        }
    }
}
