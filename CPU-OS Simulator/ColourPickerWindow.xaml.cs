using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for ColourPickerWindow.xaml
    /// </summary>
    public partial class ColourPickerWindow : Window
    {
        private ConsoleWindow parent;
        private Color selectedColor;
        private const double ROW_SIZE = 25.0;

        /// <summary>
        /// Constructor for Colour picker window
        /// </summary>
        public ColourPickerWindow()
        {
            InitializeComponent();
            CreateGrid();
        }
        /// <summary>
        /// Constructor for colour picker window that takes the window instance that is creating this window
        /// PLEASE NOTE: This constructor should always be used so data can be passed back to the main window
        /// </summary>
        /// <param name="parent">The window that is creating this window </param>
        public ColourPickerWindow(ConsoleWindow parent)
        {
            this.parent = parent;
            InitializeComponent();
            CreateGrid();
        }
        /// <summary>
        /// This function creates the grid where the colour picker buttons are displayed
        /// </summary>
        private void CreateGrid()
        {
            // Create the Grid
            Grid DynamicGrid = new Grid();
            DynamicGrid.Width = 400;
            DynamicGrid.HorizontalAlignment = HorizontalAlignment.Left;
            DynamicGrid.VerticalAlignment = VerticalAlignment.Top;
            DynamicGrid.ShowGridLines = false;
            DynamicGrid.Background = new SolidColorBrush(Colors.White);

            for (int a = 0; a < 7; a++) // add the columns
            {
                ColumnDefinition col = new ColumnDefinition();
                DynamicGrid.ColumnDefinitions.Add(col);
            }

            for (int b = 0; b < 20; b++) // add the rows
            {
                RowDefinition row = new RowDefinition();
                GridLength length = new GridLength(ROW_SIZE);
                row.MinHeight = ROW_SIZE;
                row.Height = length;
                row.MaxHeight = ROW_SIZE;
                DynamicGrid.RowDefinitions.Add(row);
            }
            DynamicGrid.Height = DynamicGrid.RowDefinitions.Count*ROW_SIZE + 5;
            CreateButtons(ref DynamicGrid); // create the buttons
            this.Height = DynamicGrid.Height + 40;
            this.Width = DynamicGrid.Width + 30;
            root_Grid.Width = Width;
            root_Grid.Height = Height;
            root_Grid.Children.Add(DynamicGrid); // add the grid to the main window
        }
        /// <summary>
        /// This method populates the grid with colour picking buttons
        /// </summary>
        /// <param name="DynamicGrid"> a reference to the grid that the buttons should be displayed in</param>
        private void CreateButtons(ref Grid DynamicGrid)
        {
            int index = 0;
            string fullname = typeof (Colors).Assembly.FullName;
            Assembly ass = Assembly.Load(fullname);
            PropertyInfo[] colourInfo = ass.GetType("System.Windows.Media.Colors").GetProperties(); // Load system colour information
            for (int i = 0; i < DynamicGrid.ColumnDefinitions.Count; i++)
            {
                for (int j = 0; j < DynamicGrid.RowDefinitions.Count; j++)
                {
                    Button button = new Button(); // create a button
                    button.Content = String.Empty;
                    button.Margin = new Thickness(3,3,3,3); // define the margin
                    button.Background = new SolidColorBrush((Color) colourInfo[index].GetValue(null)); // set the button's background colour
                    button.Click += delegate(object sender, RoutedEventArgs args)
                    {
                        if (parent.rdb_TextColour.IsChecked.Value) // if we are choosing the text colour
                        {
                            parent.txt_Console.Foreground = (((SolidColorBrush) ((Button)sender).Background)); // set the text colour
                        }
                        else if(parent.rdb_ScreenColour.IsChecked.Value) // if we are choosing the screen colour
                        {
                            parent.txt_Console.Background = (((SolidColorBrush)((Button)sender).Background)); // set the screen colour

                        }
                        this.Close(); // close the colour picker window
                    };
                    Grid.SetColumn(button,i); // set the column that the button should be displayed in
                    Grid.SetRow(button,j); // set the row that the button should be displayed in
                    DynamicGrid.Children.Add(button); // add the button to the grid
                    index++; // increment the colour index
                }
            }
        }

        /// <summary>
        /// Property for the selected colour
        /// </summary>
        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }
    }
}