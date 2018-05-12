// -- **********************************
// -- CLASS:    Extract 
// -- module for extracting folders and files from Zip packages
// -- Author:   Paul McKillop
// -- Date:     May 2018
// -- **********************************


using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace RenamerWPF
{
    public static class Extract
    {
        public static int MoveZip(string sourceFile, string destinationFolder)
        {
            // -- Check if already copied
            if (File.Exists(destinationFolder + System.IO.Path.GetFileName(sourceFile)))
            {
                DialogResult resultReplace = (DialogResult)MessageBox.Show("File exists. Do you want to replace it?", "File exists",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultReplace == System.Windows.Forms.DialogResult.Yes)
                {
                    MessageBox.Show(("Deleting and replacing"));
                    File.Delete(destinationFolder + System.IO.Path.GetFileName(sourceFile));
                    File.Copy(sourceFile, destinationFolder + System.IO.Path.GetFileName(sourceFile));
                }
                else
                {
                    MessageBox.Show("File unchanged");
                }
            }
            else
            {
                File.Copy(sourceFile, destinationFolder + System.IO.Path.GetFileName(sourceFile));
            }

            return 1;
        }

        public static int ExtractFile(string sourceFile, string destinationFolder)
        {
            // -- Extract the files
            string zipFile = destinationFolder + System.IO.Path.GetFileName(sourceFile);
            string extractPath = destinationFolder;

            ZipFile.ExtractToDirectory(zipFile, destinationFolder);

            File.Delete(zipFile);

            return 1;
        }
    }
}
