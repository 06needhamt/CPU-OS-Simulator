using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CPU_OS_Simulator
{
    public class Logger
    {
        LogWindow outputWindow;
        public Logger(LogWindow window)
        {
            this.outputWindow = window;
        }

        public void Info(string message)
        {
            EnumLogLevel level;
            if (Enum.TryParse<EnumLogLevel>(outputWindow.cmb_Level.SelectedItem.ToString(), out level))
            {
                if ((int)level >= (int)EnumLogLevel.VERBOSE)
                {
                    outputWindow.txtLog.Text += "[INFO] " + message + "\n";
                }
            }
            else
            {
                MessageBox.Show("Invalid log level selected");
            }
        }


        public void Warning(string message)
        {
            EnumLogLevel level;
            if (Enum.TryParse<EnumLogLevel>(outputWindow.cmb_Level.SelectedItem.ToString(), out level))
            {
                if ((int)level >= (int)EnumLogLevel.LOW)
                {
                    outputWindow.txtLog.Text += "[Warning] " + message + "\n";
                }
            }
            else
            {
                MessageBox.Show("Invalid log level selected");
            }
        }


        public void Error(string message)
        {
            EnumLogLevel level;
            if (Enum.TryParse<EnumLogLevel>(outputWindow.cmb_Level.SelectedItem.ToString(), out level))
            {
                if ((int)level >= (int)EnumLogLevel.MEDIUM)
                {
                    outputWindow.txtLog.Text += "[ERROR] " + message + "\n";
                }
            }
            else
            {
                MessageBox.Show("Invalid log level selected");
            }
        }


        public void Critical(string message)
        {
            EnumLogLevel level;
            if (Enum.TryParse<EnumLogLevel>(outputWindow.cmb_Level.SelectedItem.ToString(), out level))
            {
                if ((int)level >= (int)EnumLogLevel.HIGH)
                {
                    outputWindow.txtLog.Text += "[CRITICAL] " + message + "\n";
                }
            }
            else
            {
                MessageBox.Show("Invalid log level selected");
            }
        }
    }
}
