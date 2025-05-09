using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EzDecoder.PopUpProgress;

namespace EzDecoder.Utilities
{
    public static class Methods
    {

        public static void DecodeSingleCSV(bool isDirectory)
        {
            string path = GetPath(isDirectory, "csv");

            if (string.IsNullOrEmpty(path)) return;

            if (isDirectory)
            {
                if (Directory.Exists(path))
                {
                    foreach (var file in Directory.GetFiles(path, "*.csv"))
                    {
                        Logic.Methods.CsvTomlDecoder(file);
                    }
                }
                else
                {
                    ShowErrorMessage("Directory does not exist or one of the files is corrupted.");
                }
            }
            else
            {
                if (File.Exists(path))
                {
                    Logic.Methods.CsvTomlDecoder(path);
                    ShowInfoMessage("File successfully decoded: " + Path.GetFileName(path));
                }
                else
                {
                    ShowErrorMessage("File does not exist.");
                }
            }
        }


        public static void DecodeSingleTOML(bool isDirectory)
        {
            string path = GetPath(isDirectory, "toml");

            if (string.IsNullOrEmpty(path)) return; // User cancelled

            if (isDirectory)
            {
                if (Directory.Exists(path))
                {
                    foreach (var file in Directory.GetFiles(path, "*.toml"))
                    {
                        Logic.Methods.CsvTomlDecoder(file);
                    }
                }
                else
                {
                    ShowErrorMessage("Directory does not exist or one of the files is corrupted.");
                }
            }
            else
            {
                if (File.Exists(path))
                {
                    Logic.Methods.CsvTomlDecoder(path);
                    ShowInfoMessage("File successfully decoded: " + Path.GetFileName(path));
                }
                else
                {
                    ShowErrorMessage("File does not exist.");
                }
            }
        }

        public static void DecodeSingleBANK(bool isDirectory)
        {
            ShowInfoMessage("BANK decoding is in development! Please check back later.");
        }

        public static void DecodeDirectory()
        {
            string path = GetPath(true, "directory");

            if (string.IsNullOrEmpty(path)) return;

            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);
                var relevantFiles = files.Where(file =>
                    file.EndsWith(".csv", StringComparison.OrdinalIgnoreCase) ||
                    file.EndsWith(".toml", StringComparison.OrdinalIgnoreCase)).ToList();

                if (relevantFiles.Count == 0)
                {
                    ShowInfoMessage("No supported files (.csv or .toml) found in the directory.");
                    return;
                }

                using (var progressForm = new ProgressForm())
                {
                    progressForm.Show();
                    int i = 0;
                    foreach (var file in relevantFiles)
                    {
                        i++;
                        progressForm.UpdateStatus("Decoding: " + Path.GetFileName(file), i, relevantFiles.Count);
                        Logic.Methods.CsvTomlDecoder(file);
                        Application.DoEvents(); // Allows UI to refresh
                    }
                    progressForm.Close();
                }

                ShowInfoMessage("All files in the directory were successfully decoded.");
            }
            else
            {
                ShowErrorMessage("Directory does not exist or one of the files is corrupted.");
            }
        }


        public static String GetMusicFile()
        {
            var configPath = "config.toml";
            if (File.Exists(configPath))
            {
                var tomlContent = File.ReadAllText(configPath);
                foreach (var line in File.ReadLines(configPath))
                {
                    if (line.StartsWith("musicFile ="))
                    {
                        return line.Substring("musicFile =".Length).Trim();
                    }
                }
            }
            return null;
        }

        public static void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowInfoMessage(string message)
        {
            MessageBox.Show(message, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static string GetPath(bool isDirectory, string type)
        {
            if (isDirectory)
            {
                using (var folderDialog = new FolderBrowserDialog())
                {
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        return folderDialog.SelectedPath;
                    }
                    return null;
                }
            }
            else
            {
                using (var fileDialog = new OpenFileDialog())
                {
                    switch (type.ToLower())
                    {
                        case "csv":
                            fileDialog.Filter = "CSV files (*.csv)|*.csv";
                            break;
                        case "toml":
                            fileDialog.Filter = "TOML files (*.toml)|*.toml";
                            break;
                        case "bank":
                            fileDialog.Filter = "BANK files (*.bank)|*.bank";
                            break;
                        default:
                            fileDialog.Filter = "All files (*.*)|*.*";
                            break;
                    }

                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        return fileDialog.FileName;
                    }
                    return null;
                }
            }
        }

    }
}