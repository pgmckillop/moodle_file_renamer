// -- **********************************
// -- CLASS:    Utility 
// -- module for routines working with files
// -- Author:   Paul McKillop
// -- Date:     May 2018
// -- **********************************


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RenamerWPF
{
    public static class Utility
    {
        public static void ExploreRename(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                var startInfo = new ProcessStartInfo
                {
                    Arguments = folderPath,
                    FileName = "explorer.exe"
                };
                Process.Start(startInfo);
            }
            else
            {
                MessageBox.Show("Rename folder not found");
            }
        }
    }
}
