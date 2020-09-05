using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Threading;
using Logging;

namespace Editor._jqa
{
    public class JqaManager
    {
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string JqaDownloadUrl =
            "https://jqassistant.org/wp-content/uploads/2020/01/jqassistant-commandline-neo4jv3-1.8.0-distribution.zip";

        private const string JqaCsharpPluginFileName = "jqa-csharp-plugin-0.1.0.jar";

        private const string JqaCsharpPluginDownloadUrl =
            "https://github.com/softvis-research/jqa-csharp-plugin/releases/download/v0.1.0-alpha/" +
            JqaCsharpPluginFileName;

        private readonly JqaPaths _jqaPaths;
        private Thread _installThread;

        public JqaManager(JqaPaths jqaPaths)
        {
            _jqaPaths = jqaPaths;
        }

        public bool CheckIfJqaIsInstalled()
        {
            return Directory.Exists(_jqaPaths.BuildJqaInstallationPath());
        }

        public bool CheckIfJqaIsInstalling()
        {
            return _installThread != null && _installThread.IsAlive;
        }

        public void InstallJqa()
        {
            InstallJqaAsync();
        }

        private void InstallJqaAsync()
        {
            _installThread = new Thread(
                () =>
                {
                    Thread.CurrentThread.IsBackground = true;

                    Log.Debug("Creating installation directory for jQA under {} ...",
                        _jqaPaths.BuildJqaInstallationPath());
                    Directory.CreateDirectory(_jqaPaths.BuildJqaInstallationPath());

                    DownloadJqa();
                    DownloadCSharpPlugin();

                    Log.Debug("Finished installing CQA.");
                }
            );

            _installThread.Start();
        }

        private void DownloadCSharpPlugin()
        {
            string jarTargetPath = Path.Combine(_jqaPaths.BuildJqaPluginFolderPath(), JqaCsharpPluginFileName);

            using (WebClient webClient = new WebClient())
            {
                Log.Debug(
                    "Downloading jQA C# plugin from URL {} and saving it to {} ...",
                    JqaCsharpPluginDownloadUrl,
                    jarTargetPath
                );

                webClient.DownloadFile(
                    JqaCsharpPluginDownloadUrl,
                    jarTargetPath
                );

                Log.Debug("Finished downloading jQA C# plugin.");
            }
        }

        private void DownloadJqa()
        {
            string zipTargetPath = Path.Combine(_jqaPaths.BuildJqaInstallationPath(), "temp.zip");

            using (WebClient webClient = new WebClient())
            {
                Log.Debug("Downloading jQA from URL {} and saving it to {} ...", JqaDownloadUrl, zipTargetPath);

                webClient.DownloadFile(
                    JqaDownloadUrl,
                    zipTargetPath
                );

                Log.Debug("Finished downloading jQA.");
            }

            Log.Debug("Extracting zip at {} ...", zipTargetPath);
            ZipFile.ExtractToDirectory(zipTargetPath, _jqaPaths.BuildJqaInstallationPath());

            Log.Debug("Deleting zip at {} ...", zipTargetPath);
            File.Delete(zipTargetPath);
        }

        public void UninstallJqa()
        {
            Directory.Delete(_jqaPaths.BuildJqaInstallationPath(), true);
            Log.Debug("Successfully uninstalled CQA.");
        }

        public void AbortInstallation()
        {
            _installThread.Abort();
        }
    }
}