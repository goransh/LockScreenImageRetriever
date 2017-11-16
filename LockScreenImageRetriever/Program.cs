using System;
using System.IO;
using System.Windows.Forms;

namespace LockScreenImageRetriever {

    /// <summary>
    /// Locates images used by Windows to display lock screen images from Bing (and a few other images) 
    /// and copies them to a destination folder of the user's choosing. A .jpg extension is also added to 
    /// the files.
    /// </summary>
    class Program {

        static string[] FindFiles() {
            string appData = Environment.GetEnvironmentVariable("LocalAppData");
            string assets =
                appData + @"\Packages\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\LocalState\Assets";
            string[] files;
            try {
                files = Directory.GetFiles(assets);
            }
            catch (IOException e) {
                MessageBox.Show(e.GetBaseException().Message, e.GetType().FullName, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
                throw;
            }
            return files;
        }

        [STAThread]
        static void Main(string[] args) {

            FolderBrowserDialog folderBrowser = new FolderBrowserDialog {Description = "Select destination folder."};
            DialogResult result = folderBrowser.ShowDialog();

            string selectedPath = folderBrowser.SelectedPath;

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(selectedPath)) {

                foreach (string file in FindFiles()) {

                    string[] path = file.Split(Path.DirectorySeparatorChar);
                    string name = path[path.Length - 1];

                    try {
                        File.Copy(file, $"{selectedPath}{Path.DirectorySeparatorChar}{name}.jpg", true);
                    }
                    catch (IOException e) {
                        Console.WriteLine(e.GetBaseException().Message);
                    }
                }

                MessageBox.Show($"Images successfully saved to {selectedPath}.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
    }
}
