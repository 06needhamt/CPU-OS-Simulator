using System;
using System.IO;
using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace CPU_OS_Simulator.Compiler
{
    /// <summary>
    /// This class represents a program source file to be compiled by the compiler
    /// </summary>
    public class SourceFile
    {
        private string path = String.Empty;
        private string fileContents = String.Empty;

        public SourceFile(string path)
        {
            this.path = path;
            try
            {
                String[] temp = File.ReadAllLines(path);
                foreach (string str in temp)
                {
                    fileContents += str;
                }
            }
            catch (DirectoryNotFoundException directoryNotFoundException)
            {
                MessageBox.Show("The Requested Directory was not found");
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                MessageBox.Show("You do not have permissions to access this file");
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                MessageBox.Show("The Requested File Was Not Found");
            }
            catch (IOException ioException)
            {
                MessageBox.Show("There was an error while reading the file");
            }
            catch (SecurityException securityException)
            {
                MessageBox.Show("Security exception occurred when reading the file");
            }

        }

        public SourceFile(ref TextBox textbox)
        {
            if (textbox == null)
            {
                throw new InvalidOperationException("Text Box reference can not be NULL");
                
            }
            fileContents = textbox.Text;
        }

        public string FileContents
        {
            get { return fileContents; }
            set { fileContents = value; }
        }

        public string Path1
        {
            get { return path; }
            set { path = value; }
        }
    }
}