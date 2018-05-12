// -- **********************************
// -- CLASS:    Rename 
// -- module for routines renaming files
// -- Author:   Paul McKillop
// -- Date:     May 2018
// -- **********************************


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RenamerWPF
{
    public  class Rename
    {

        /// <summary>
        /// Main rename method calling utility methods below
        /// </summary>
        /// <param name="renameFolderPath"></param>
        public void RenameAll(string renameFolderPath, string taskPrefix)
        {
            // -- handling variables
            int i, j, k;
            string fExt = string.Empty;

            // -- for renaming directories
            // -- Get recursive information of everything in the directory
            DirectoryInfo d = new DirectoryInfo(renameFolderPath);

            try
            {
                // -- ToDo Routine to check if conversion already done
                // -- Strategy
                k = 0;
                // -- down one layer nested
                foreach (DirectoryInfo subDir in d.GetDirectories())
                {
                    // -- Tracking of operations done
                    i = 0;
                    string originalPathAndName = subDir.FullName.ToString();
                    // strip path information
                    string originalNameMinusPath = DirectoryNameMinusPath(originalPathAndName);
                    // -- construct the new name
                    string newName = Path.Combine(renameFolderPath, NewFolderName(originalNameMinusPath, taskPrefix));
                    // -- Get rid of old and replace with new
                    Directory.Move(originalPathAndName, newName);
                    // Increment number of files handled
                    i++;

                    // -- Iterate to construct names for folders and files
                    DirectoryInfo n = new DirectoryInfo(newName);
                    // -- Use an array of files
                    FileInfo[] files = n.GetFiles();
                    // -- handler for names of multiple files in a folder using suffix style 001 -- 999
                    j = 1;
                    foreach (FileInfo f in files)
                    {
                        // -- construct name for file when moved
                        // -- increment file name suffix where multiple files in folder (up to 999)
                        // -- Preserve file extensions
                        // -- add file extension from original file
                        fExt = f.Extension.ToString();
                        string changeTo = taskPrefix + fExt;


                        // -- Handle multiple files in the folder so that
                        // -- different file names given. Will be added to all files
                        // -- as a suffix
                        string fileNumber;
                        //-- Add leading zeros where under 1000 for sorting rules
                        if (j < 10)
                        {
                            fileNumber = "00" + j.ToString();
                        }
                        else if (j >= 10 && j < 100)
                        {
                            fileNumber = "0" + j.ToString();
                        }
                        else
                        {
                            fileNumber = j.ToString();
                        }
                        // -- leave original intact and make a copy in Rename root folder
                        // -- use the folder name as file name with fileNUmber suffix and extension
                        File.Copy(f.FullName, Path.Combine(f.Directory.ToString(), f.Directory.ToString() + "_" + fileNumber + fExt));
                        j++;
                        k++;
                    }

                    // -- Display change information
                    // -- TODO i increment is in the wrong place
                }
            }
            catch (Exception e)
            {
                MessageBox.Show((e.Message));
                return;
            }

        }
        
        
        // -- Utility methods
        // method to find the position of the end of the path
        private int EndOfPath(string myFileName)
        {
            int lastBackSlash = myFileName.LastIndexOf("\\");

            return lastBackSlash;

        }

        // -- Extract just the name of the directory
        private string DirectoryNameMinusPath(string myDirectory)
        {
            int nameLength = myDirectory.Length;
            int pathEnd = EndOfPath(myDirectory);

            string withoutPath = myDirectory.Substring(pathEnd + 1, nameLength - (pathEnd + 1));

            return withoutPath;
        }

        // -- New name folder maker
        private string NewFolderName(string myFileName, string prefix)
        {
            // handler variables. Markers of characters
            int firstSpace = 0;
            int firstUnderscore = 0;
            int secondUnderscore = 0;
            int filenameLength = 0;

            string forename = string.Empty;
            string surname = string.Empty;
            string downloadNumber = string.Empty;
            string tempName = string.Empty;

            // marker positions in filename
            filenameLength = myFileName.Length;
            firstSpace = myFileName.IndexOf(" ");
            firstUnderscore = myFileName.IndexOf("_");

            // loop to find second underscore start from first underscore
            for (int i = firstUnderscore + 1; i < filenameLength; i++)
            {
                if (myFileName[i].ToString() == "_")
                {
                    secondUnderscore = i;
                    break;
                }
            }

            // -- Extract portions of the downloaded folder name
            forename = myFileName.Substring(0, firstSpace);
            surname = myFileName.Substring(firstSpace + 1, firstUnderscore - firstSpace - 1);
            downloadNumber = myFileName.Substring(firstUnderscore + 1, secondUnderscore - firstUnderscore - 1);

            // construct and return
            tempName = prefix + " " + surname + ", " + forename;
            return tempName;
        }
    }
}
