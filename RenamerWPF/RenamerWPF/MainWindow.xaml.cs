using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace RenamerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // -- Select source file
        private void SelectSourceButton_OnClick(object sender, RoutedEventArgs e)
        {
            // -- Get the source zip file location for selection
            var ofd = new OpenFileDialog
            {
                Filter = "Zip files|*.zip|All Files|*.*",
                DefaultExt = "*.zip",
                Multiselect = false
            };

            if (ofd.ShowDialog() == true)
            {
                string filename = ofd.FileName;
                SourceFileLocationTextBox.Text = filename;
            }
        }

        // -- Destination location
        private void SelectDestinationButton_OnClick(object sender, RoutedEventArgs e)
        {
            // -- Get the destination folder Using Steam-age folder dialog
            var fbd = new FolderBrowserDialog
            {
                Description = "Select destination folder",
                ShowNewFolderButton = true
            };
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DestinationFileLocationTextBox.Text = fbd.SelectedPath;
            }
        }

        // -- Do the renaming
        private void RenameFilesButton_OnClick(object sender, RoutedEventArgs e)
        {
            // -- Harvest source and destination
            // -- Error trap the vital values

            // -- Source zip
            string sourceFile = SourceFileLocationTextBox.Text;
            if (string.IsNullOrEmpty(sourceFile))
            {
                MessageBox.Show("A source zip file must be selected", "No file selected", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                SourceFileLocationTextBox.Focus();
                return;
            }
            // -- Destination folder
            string destinationFolder = DestinationFileLocationTextBox.Text;
            if (string.IsNullOrEmpty(destinationFolder))
            {
                MessageBox.Show("A destination folder must be selected", "No destination selected", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                DestinationFileLocationTextBox.Focus();
                return;
            }
            
            // -- Manage paths using escape characters where needed
            sourceFile = Regex.Replace(sourceFile, @"\\", @"\\");
            destinationFolder = $"{destinationFolder}{@"\"}";
            destinationFolder = Regex.Replace(destinationFolder, @"\\", @"\\");
            
            // -- Tast Prefix or default
            string taskPrefix = PrefixTextBox.Text;
            if (string.IsNullOrEmpty(taskPrefix))
            {
                taskPrefix = "Task";
            }

            // -- copy the zip over
            int fileMoved = Extract.MoveZip(sourceFile, destinationFolder);

            // -- Use the Extract Class to unzip the package folders
            int result = Extract.ExtractFile(sourceFile, destinationFolder);

            // -- Use Rename class to rename files and folders
            Rename rename = new Rename();
            // -- Call rename method
            rename.RenameAll(destinationFolder, taskPrefix);

            // -- DEBUG TODO: destination folder not opening
            // -- Open folder with renamed files and folders
            Utility.ExploreRename(destinationFolder);
            
            // -- Clear text boxes
            Clear();


        }

        // -- Clear form textboxes for reset
        private void ClearButton_OnClick(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            PrefixTextBox.Text = "";
            SourceFileLocationTextBox.Text = "";
            DestinationFileLocationTextBox.Text = "";
        }
    }
}
