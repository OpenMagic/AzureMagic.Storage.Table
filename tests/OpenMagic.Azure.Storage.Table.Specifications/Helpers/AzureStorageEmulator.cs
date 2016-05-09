using System.Diagnostics;
using System.Linq;
using Anotar.LibLog;

namespace OpenMagic.Azure.Storage.Table.Specifications.Helpers
{
    internal class AzureStorageEmulator
    {
        private const string ProcessName = "AzureStorageEmulator";
        private const string FileName = @"C:\Program Files (x86)\Microsoft SDKs\Azure\Storage Emulator\AzureStorageEmulator.exe";

        internal static void StartIfNotRunning()
        {
            if (IsRunning())
            {
                return;
            }
            Start();
        }

        private static void Start()
        {
            LogTo.Info("Starting Azure Storage Emulator...");

            var start = new ProcessStartInfo
            {
                Arguments = "start",
                FileName = FileName
            };


            var process = new Process
            {
                StartInfo = start
            };

            process.Start();
            process.WaitForExit();

            LogTo.Info("Started Azure Storage Emulator.");
        }

        private static bool IsRunning()
        {
            var isRunning = Process.GetProcessesByName(ProcessName).Any();

            LogTo.Info($"Azure Storage Emulator is {(isRunning ? "" : "not")} running.");
            return isRunning;
        }
    }
}