﻿using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows;
using FontStyles = System.Drawing.FontStyle;

namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for FontPickerWindow.xaml
    /// </summary>
    public partial class FontPickerWindow : Window
    {
        private ConsoleWindow parent;
        /// <summary>
        /// Default constructor for font picker window
        /// </summary>
        public FontPickerWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Constructor for font picker window that takes the window instance that is creating this window
        /// PLEASE NOTE: This constructor should always be used so data can be passed back to the parent window
        /// </summary>
        /// <param name="parent">The window that is creating this window </param>
        public FontPickerWindow(ConsoleWindow parent)
        {
            this.parent = parent;
            InitializeComponent();
        }

        private void FontPickerWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateFonts();
            PopulateStyles();

        }

        /// <summary>
        /// This function populates the list of font styles
        /// </summary>
        private void PopulateStyles()
        {
            lstbx_Style.ItemsSource = null;
            lstbx_Style.Items.Add(FontStyles.Regular);
            lstbx_Style.Items.Add(FontStyles.Bold);
            lstbx_Style.Items.Add(FontStyles.Italic);
            lstbx_Style.Items.Add(FontStyles.Underline);
            lstbx_Style.Items.Add(FontStyles.Strikeout);
        }

        /// <summary>
        /// This function populates the list of fonts
        /// </summary>
        private void PopulateFonts()
        {
            lstbx_Font.ItemsSource = null;
            InstalledFontCollection installedFontCollection = new InstalledFontCollection();

            foreach (FontFamily font in installedFontCollection.Families)
            {
                lstbx_Font.Items.Add(font.Name);
            }
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            if (lstbx_Font.SelectedItem == null)
            {
                parent.FontName = "Consolas";
            }
            else
            {
                parent.FontName = lstbx_Font.SelectedItem.ToString();
            }
            if (lstbx_Size.SelectedItem == null)
            {
                parent.FontSizes = 12;
            }
            else
            {
                parent.FontSizes = (int) lstbx_Size.SelectedItem;
            }
            if (lstbx_Style.SelectedItems == null || lstbx_Style.SelectedItems.Count == 0)
            {
                parent.FontStyles = 0;
            }
            else
            {
                for (int index = 0; index < lstbx_Font.SelectedItems.Count; index++)
                {
                    FontStyles style = (FontStyles) Enum.Parse(typeof (FontStyles), lstbx_Style.Items[index].ToString());
                    parent.FontStyles |= (int)style;
                }
            }

            Close();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
