using System.Windows;
using System.Windows.Controls; // Required for Button, Border, GridSplitter etc.

namespace MathSol.IDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Store the default width and min-width of the side panel and splitter columns
        private GridLength defaultSidePanelWidth;
        private GridLength defaultSplitterWidth;
        private double defaultSidePanelMinWidth; // Store the original MinWidth
        private double defaultSplitterMinWidth;  // Store the original MinWidth

        public MainWindow()
        {
            InitializeComponent();
            // Hook up the Loaded event to capture initial widths/min-widths after layout is done
            this.Loaded += MainWindow_Loaded;
        }

        /// <summary>
        /// Stores the initial column widths and MinWidths once the window is loaded and ready.
        /// Also sets the initial visibility state of the side panel columns and splitter
        /// based on the ExplorerPanel's default visibility in XAML.
        /// </summary>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Store the designed widths and MinWidths from XAML
            defaultSidePanelWidth = SidePanelColumn.Width;
            defaultSplitterWidth = SplitterColumn.Width;
            defaultSidePanelMinWidth = SidePanelColumn.MinWidth; // Capture initial MinWidth
            defaultSplitterMinWidth = SplitterColumn.MinWidth; // Capture initial MinWidth

            // Check the initial visibility state defined in XAML for ExplorerPanel
            if (ExplorerPanel.Visibility != Visibility.Visible)
            {
                // If Explorer is not visible by default, collapse the columns and hide the splitter
                CollapseSidePanelColumns();
            }
            else
            {
                // Otherwise, ensure the columns and splitter are visible
                RestoreSidePanelColumns();
            }
        }

        /// <summary>
        /// Handles clicks on the Activity Bar buttons (Explorer, Search, Settings).
        /// Toggles the visibility of the corresponding panel and collapses/restores
        /// the side panel columns and the vertical splitter accordingly.
        /// </summary>
        private void ActivityBarButton_Click(object sender, RoutedEventArgs e)
        {
            // Ensure the sender is a Button and its Tag is a string (panel name)
            if (sender is Button clickedButton && clickedButton.Tag is string panelName)
            {
                // Find the UI element (Border) corresponding to the panel name
                UIElement targetPanel = FindName(panelName) as UIElement;

                if (targetPanel != null)
                {
                    // Check if the panel we intend to show/toggle is currently visible
                    bool isTargetVisible = targetPanel.Visibility == Visibility.Visible;

                    // --- Step 1: Always hide all individual panels ---
                    ExplorerPanel.Visibility = Visibility.Collapsed;
                    SearchPanel.Visibility = Visibility.Collapsed;
                    SettingsPanel.Visibility = Visibility.Collapsed;

                    // --- Step 2: Decide whether to show the target panel and restore columns/splitter, or collapse them ---
                    if (!isTargetVisible)
                    {
                        // If the target panel wasn't visible, make it visible now.
                        targetPanel.Visibility = Visibility.Visible;
                        // Ensure the columns and splitter are restored to their default state.
                        RestoreSidePanelColumns();
                    }
                    else
                    {
                        // If the target panel was already visible, clicking its button again
                        // means we should hide the side panel area completely.
                        CollapseSidePanelColumns();
                        // The target panel itself remains collapsed from Step 1.
                    }
                }
            }
        }

        /// <summary>
        /// Collapses the side panel and splitter columns by setting their width and MinWidth to 0,
        /// and explicitly hides the GridSplitter control.
        /// This effectively hides the entire side panel area including the splitter line.
        /// </summary>
        private void CollapseSidePanelColumns()
        {
            // Set column widths to zero to collapse them
            SidePanelColumn.Width = new GridLength(0);
            SplitterColumn.Width = new GridLength(0);
            // Set MinWidth to 0 to allow full collapse
            SidePanelColumn.MinWidth = 0;
            SplitterColumn.MinWidth = 0; // Splitter column MinWidth might not be strictly needed but good practice
            // Explicitly hide the GridSplitter control itself
            VerticalSplitter.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Restores the side panel and splitter columns to their default widths and MinWidths,
        /// and explicitly makes the GridSplitter control visible again.
        /// </summary>
        private void RestoreSidePanelColumns()
        {
            // Restore original widths stored during the Loaded event
            SidePanelColumn.Width = defaultSidePanelWidth;
            SplitterColumn.Width = defaultSplitterWidth;
            // Restore original MinWidths
            SidePanelColumn.MinWidth = defaultSidePanelMinWidth;
            SplitterColumn.MinWidth = defaultSplitterMinWidth;
            // Explicitly show the GridSplitter control
            VerticalSplitter.Visibility = Visibility.Visible;
        }


        // Placeholder for the Run button's click event handler.
        // Make sure to add Click="RunButton_Click" to the RunButton in MainWindow.xaml
        // private void RunButton_Click(object sender, RoutedEventArgs e)
        // {
        //     // Example: Append message to output console
        //     OutputConsole.Text += "\n>>> Executing code...";
        //     // Example: Get code from editor
        //     // string codeToRun = CodeEditor.Text;
        //     // TODO: Implement actual code execution logic using codeToRun
        //
        //     // Scroll the output console to the bottom to show the latest messages
        //     OutputScrollViewer.ScrollToBottom();
        // }
    }
}
