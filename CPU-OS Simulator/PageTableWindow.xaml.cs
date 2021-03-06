﻿using System;
using System.Windows;
using System.Windows.Controls;
using CPU_OS_Simulator.Memory;

namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for PageTableWindow.xaml
    /// </summary>
    public partial class PageTableWindow : Window
    {
        private MemoryWindow parent;
        //private PhysicalMemoryWindow physicalMemoryParent;

        /// <summary>
        /// Default Constructor for page table window
        /// </summary>
        private PageTableWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Constructor for page table window that takes the window instance that is creating this window
        /// PLEASE NOTE: This constructor should always be used so data can be passed back to the parent window
        /// </summary>
        /// <param name="parent">The window that is creating this window </param>
        public PageTableWindow(MemoryWindow parent)
        {
            this.parent = parent;
            //physicalMemoryParent = null;
            InitializeComponent();
        }

        ///// <summary>
        ///// Constructor for page table window that takes the window instance that is creating this window
        ///// PLEASE NOTE: This constructor should always be used so data can be passed back to the parent window
        ///// </summary>
        ///// <param name="parent">The window that is creating this window </param>
        //public PageTableWindow(PhysicalMemoryWindow parent)
        //{
        //    this.physicalMemoryParent = parent;
        //    this.parent = null;
        //    InitializeComponent();
        //}

        private void PageTableWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            lst_Pages.ItemsSource = null;
            lst_Pages.Items.Clear();
            if (parent.MainParentWindow != null)
            {
                lst_Pages.ItemsSource = parent.MainParentWindow.Memory.Table.Entries;
            }
            else if (parent.PhysicalMemoryParentWindow != null)
            {
                if (parent.PhysicalMemoryParentWindow.OsParent != null)
                {
                    lst_Pages.ItemsSource = parent.PhysicalMemoryParentWindow.OsParent.WindowParent.Memory.Table.Entries;
                }
                else if (parent.PhysicalMemoryParentWindow.UtilisationParent != null)
                {
                    lst_Pages.ItemsSource =
                        parent.PhysicalMemoryParentWindow.UtilisationParent.OsParent.WindowParent.Memory.Table.Entries;
                }
                else
                {
                    MessageBox.Show(
                   "ERROR: An ERROR has occurred in the window manager please report this to your tutor, the program will now terminate");
                    Environment.Exit(1);
                }
            }
            else
            {
                MessageBox.Show(
                    "ERROR: An ERROR has occurred in the window manager please report this to your tutor, the program will now terminate");
                Environment.Exit(1);
            }

        }

        private void chk_Stay_On_Top_Checked(object sender, RoutedEventArgs e)
        {
            Topmost = true;
        }

        private void chk_Stay_On_Top_Unchecked(object sender, RoutedEventArgs e)
        {
            Topmost = false;
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btn_Swap_Out_Click(object sender, RoutedEventArgs e)
        {
            if (((Button) sender).Content.ToString().Equals("SWAP OUT"))
            {
                int index = lst_Pages.SelectedIndex;
                if (parent.MainParentWindow != null)
                {
                    parent.MainParentWindow.Memory.Table.Entries[index].Page.SwapOut(0, index);
                }
                else if (parent.PhysicalMemoryParentWindow != null)
                {
                    if (parent.PhysicalMemoryParentWindow.UtilisationParent != null)
                    {
                        parent.PhysicalMemoryParentWindow.UtilisationParent.OsParent.WindowParent.Memory.Table.Entries[index].Page.SwapOut(0,index);
                    }
                    else if (parent.PhysicalMemoryParentWindow.OsParent != null)
                    {
                        parent.PhysicalMemoryParentWindow.OsParent.WindowParent.Memory.Table.Entries[index].Page.SwapOut(0, index);
                    }
                    else
                    {
                        MessageBox.Show(
                                "ERROR: An ERROR has occurred in the window manager please report this to your tutor, the program will now terminate");
                        Environment.Exit(1);
                    }
                }
                else
                {
                    MessageBox.Show(
                            "ERROR: An ERROR has occurred in the window manager please report this to your tutor, the program will now terminate");
                    Environment.Exit(1);
                }
            }
            else if (((Button) sender).Content.ToString().Equals("SWAP IN"))
            {
                int index = lst_Pages.SelectedIndex;
                if (parent.MainParentWindow != null)
                {
                    parent.MainParentWindow.Memory.Table.Entries[index].Page.SwapIn(0, index);
                }
                else if (parent.PhysicalMemoryParentWindow != null)
                {
                    if (parent.PhysicalMemoryParentWindow.UtilisationParent != null)
                    {
                        parent.PhysicalMemoryParentWindow.UtilisationParent.OsParent.WindowParent.Memory.Table.Entries[index].Page.SwapIn(0, index);
                    }
                    else if (parent.PhysicalMemoryParentWindow.OsParent != null)
                    {
                        parent.PhysicalMemoryParentWindow.OsParent.WindowParent.Memory.Table.Entries[index].Page.SwapIn(0, index);
                    }
                    else
                    {
                        MessageBox.Show(
                                "ERROR: An ERROR has occurred in the window manager please report this to your tutor, the program will now terminate");
                        Environment.Exit(1);
                    }
                }
                else
                {
                    MessageBox.Show(
                            "ERROR: An ERROR has occurred in the window manager please report this to your tutor, the program will now terminate");
                    Environment.Exit(1);
                }
            }
            else
            {
                throw new Exception("Unknown memory page operation requested " + ((Button) sender).Content);
            }
            UpdateEntries();
        }
        /// <summary>
        /// This function updates the page table entries shown in the user interface
        /// </summary>
        private void UpdateEntries()
        {
            lst_Pages.ItemsSource = null;
            lst_Pages.Items.Clear();
            if (parent.MainParentWindow != null)
            {
                lst_Pages.ItemsSource = parent.MainParentWindow.Memory.Table.Entries;
            }
            else if (parent.PhysicalMemoryParentWindow != null)
            {
                if (parent.PhysicalMemoryParentWindow.UtilisationParent != null)
                {
                    lst_Pages.ItemsSource = parent.PhysicalMemoryParentWindow.UtilisationParent.OsParent.WindowParent.Memory.Table.Entries;
                }
                else if (parent.PhysicalMemoryParentWindow.OsParent != null)
                {
                    lst_Pages.ItemsSource = parent.PhysicalMemoryParentWindow.OsParent.WindowParent.Memory.Table.Entries;
                }
                else
                {
                    MessageBox.Show(
                            "ERROR: An ERROR has occurred in the window manager please report this to your tutor, the program will now terminate");
                    Environment.Exit(1);
                }
            }
            else
            {
                MessageBox.Show(
                        "ERROR: An ERROR has occurred in the window manager please report this to your tutor, the program will now terminate");
                Environment.Exit(1);
            }
        }

        private void lst_Pages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PageTableEntry selectedEntry = (PageTableEntry) lst_Pages.SelectedItem;
            if (selectedEntry != null)
            { 
                if (selectedEntry.SwappedOut)
                {
                    btn_Swap_Out.Content = "SWAP IN";
                }
                else
                {
                    btn_Swap_Out.Content = "SWAP OUT";
                }
            }
        }
    }
}
