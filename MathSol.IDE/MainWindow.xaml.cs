using System;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Xml;
using System.Collections;

// Using aliases
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;

// AvalonEdit specific using statements
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

// Assuming your Interpreter and CodeFile classes are in these namespaces
using MathSol.Interpreter;
using MathSol.Interpreter.FileSystem;

// Important: Add a reference to System.Windows.Forms.dll for FolderBrowserDialog

namespace MathSol.IDE
{
    public class FileSystemItem
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public bool IsDirectory { get; set; }
        public string Icon => IsDirectory ? "\uE8D5" : "\uE7C3";
        public ObservableCollection<FileSystemItem> Children { get; set; }

        // Property to get parent, useful for refreshing TreeView after rename/delete
        // This would typically be set when items are added to the TreeView or loaded.
        // For simplicity, we'll find the parent dynamically when needed for now.
        // public FileSystemItem Parent { get; set; } 

        public FileSystemItem()
        {
            Children = new ObservableCollection<FileSystemItem>();
        }
    }

    public class TextBlockWriter : TextWriter
    {
        private readonly TextBlock _outputTextBlock;
        private readonly ScrollViewer _outputScrollViewer;
        private readonly Action<Action> _dispatcherAction;

        public TextBlockWriter(TextBlock outputTextBlock, ScrollViewer outputScrollViewer, Action<Action> dispatcherAction)
        {
            _outputTextBlock = outputTextBlock;
            _outputScrollViewer = outputScrollViewer;
            _dispatcherAction = dispatcherAction;
        }

        public override void Write(char value)
        {
            _dispatcherAction(() => {
                _outputTextBlock.Text += value;
                if (_outputScrollViewer.VerticalOffset == _outputScrollViewer.ScrollableHeight || _outputScrollViewer.ScrollableHeight == 0)
                {
                    _outputScrollViewer.ScrollToBottom();
                }
            });
        }

        public override void WriteLine(string value)
        {
            _dispatcherAction(() => {
                _outputTextBlock.Text += value + Environment.NewLine;
                if (_outputScrollViewer.VerticalOffset == _outputScrollViewer.ScrollableHeight || _outputScrollViewer.ScrollableHeight == 0)
                {
                    _outputScrollViewer.ScrollToBottom();
                }
            });
        }

        public override System.Text.Encoding Encoding => System.Text.Encoding.UTF8;
    }


    public partial class MainWindow : Window
    {
        private GridLength defaultSidePanelWidth;
        private GridLength defaultSplitterWidth;
        private double defaultSidePanelMinWidth;
        private double defaultSplitterMinWidth;

        private string _currentOpenFilePath = null;
        private TextWriter _originalConsoleOut;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            UpdateRunButtonState();
            LoadMslSyntaxHighlighting();
            AvalonTextEditor.TextChanged += AvalonTextEditor_TextChanged;
        }

        private void LoadMslSyntaxHighlighting()
        {
            string xshdContent =
                @"<SyntaxDefinition name=""MSL"" xmlns=""http://icsharpcode.net/sharpdevelop/syntaxdefinition/2008"">
                    <Color name=""Comment"" foreground=""Green"" exampleText=""# comment"" />
                    <Color name=""Preprocessor"" foreground=""Purple"" fontWeight=""bold"" exampleText=""%include"" />
                    <Color name=""Keyword"" foreground=""Blue"" fontWeight=""bold"" exampleText=""define if else"" />
                    <Color name=""Function"" foreground=""DarkCyan"" fontWeight=""bold"" exampleText=""print() diff()""/>
                    <Color name=""String"" foreground=""#D69D85"" exampleText=""&quot;a string&quot;"" />
                    <Color name=""CharLiteral"" foreground=""#D69D85"" exampleText=""'c'"" />
                    <Color name=""Number"" foreground=""#B5CEA8"" exampleText=""3.14 100 2.5e-3"" />
                    <Color name=""Boolean"" foreground=""#569CD6"" fontWeight=""bold"" exampleText=""true false""/>
                    <Color name=""Operator"" foreground=""#DCDCDC"" exampleText="":= + - * / ^ =="" />
                    <Color name=""Delimiter"" foreground=""#DCDCDC"" exampleText=""( ) { } , ;"" />
                    <Color name=""Symbol"" foreground=""#9CDCFE"" exampleText=""x y my_var""/>

                    <RuleSet ignoreCase=""false"">
                        <Span color=""Comment"" begin=""#"" />
                        <Rule color=""Preprocessor"">^\s*%include</Rule>
                        <Span color=""String"" multiline=""false""><Begin>""</Begin><End>""</End><RuleSet><Rule>\\.</Rule></RuleSet></Span>
                        <Span color=""CharLiteral"" multiline=""false""><Begin>'</Begin><End>'</End><RuleSet><Rule>\\.</Rule></RuleSet></Span>
                        <Keywords color=""Keyword""><Word>if</Word><Word>else</Word><Word>while</Word><Word>define</Word><Word>as</Word><Word>return</Word></Keywords>
                        <Keywords color=""Boolean""><Word>true</Word><Word>false</Word></Keywords>
                        <Keywords color=""Function""><Word>print</Word><Word>diff</Word><Word>integrate</Word><Word>solve</Word><Word>simplify</Word><Word>plot</Word><Word>show</Word><Word>kind</Word><Word>construct</Word><Word>free_of</Word><Word>number_of_operands</Word><Word>substitute</Word><Word>sequential_substitute</Word><Word>type</Word><Word>gcd</Word><Word>lcm</Word><Word>fraction</Word><Word>sqrt</Word><Word>sin</Word><Word>cos</Word></Keywords>
                        <Rule color=""Operator"">:=</Rule>
                        <Rule color=""Operator"">[\+\-\*\/\^==&lt;&gt;!=\&amp;\|\~%]</Rule>
                        <Rule color=""Delimiter"">[()\[\]{},.;:]</Rule>
                        <Rule color=""Number"">\b(\d+(\.\d*)?|\.\d+)([eE][+-]?\d+)?\b | 0[xX][0-9a-fA-F]+\b</Rule>
                        <Rule color=""Symbol"">[a-zA-Z_][a-zA-Z0-9_]*</Rule>
                    </RuleSet>
                </SyntaxDefinition>";
            try
            {
                using (var reader = new XmlTextReader(new StringReader(xshdContent)))
                {
                    var mslHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                    HighlightingManager.Instance.RegisterHighlighting("MSL", new[] { ".msl" }, mslHighlighting);
                    AvalonTextEditor.SyntaxHighlighting = mslHighlighting;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying syntax highlighting: {ex.Message}", "Highlighting Error");
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            defaultSidePanelWidth = SidePanelColumn.Width;
            defaultSplitterWidth = SplitterColumn.Width;
            defaultSidePanelMinWidth = SidePanelColumn.MinWidth;
            defaultSplitterMinWidth = SplitterColumn.MinWidth;

            if (ExplorerPanel.Visibility != Visibility.Visible)
            {
                CollapseSidePanelColumns();
            }
            else
            {
                RestoreSidePanelColumns();
            }
            _originalConsoleOut = Console.Out;
        }

        private void ActivityBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton && clickedButton.Tag is string panelName)
            {
                UIElement targetPanel = FindName(panelName) as UIElement;
                if (targetPanel != null)
                {
                    bool isTargetCurrentlyVisible = targetPanel.Visibility == Visibility.Visible;
                    ExplorerPanel.Visibility = Visibility.Collapsed;
                    SearchPanel.Visibility = Visibility.Collapsed;
                    SettingsPanel.Visibility = Visibility.Collapsed;

                    if (!isTargetCurrentlyVisible)
                    {
                        targetPanel.Visibility = Visibility.Visible;
                        RestoreSidePanelColumns();
                    }
                    else
                    {
                        CollapseSidePanelColumns();
                    }
                }
            }
        }

        private void CollapseSidePanelColumns()
        {
            SidePanelColumn.Width = new GridLength(0);
            SplitterColumn.Width = new GridLength(0);
            SidePanelColumn.MinWidth = 0;
            SplitterColumn.MinWidth = 0;
            VerticalSplitter.Visibility = Visibility.Collapsed;
        }

        private void RestoreSidePanelColumns()
        {
            SidePanelColumn.Width = defaultSidePanelWidth;
            SplitterColumn.Width = defaultSplitterWidth;
            SidePanelColumn.MinWidth = defaultSidePanelMinWidth;
            SplitterColumn.MinWidth = defaultSplitterMinWidth;
            VerticalSplitter.Visibility = Visibility.Visible;
        }

        private void ChooseDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "Select a directory to open in the Explorer";
            dialog.UseDescriptionForTitle = true;

            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
            {
                string selectedPath = dialog.SelectedPath;
                CurrentDirectoryPathTextBlock.Text = selectedPath;
                LoadInitialDirectory(selectedPath);
            }
        }

        private void LoadInitialDirectory(string rootPath)
        {
            DirectoryTreeView.Items.Clear();
            _currentOpenFilePath = null;
            AvalonTextEditor.Text = "// Choose a file from the Explorer to open...";
            AvalonTextEditor.IsModified = false;
            UpdateTitle();
            UpdateRunButtonState();
            UpdateSaveButtonState();
            try
            {
                var rootDirInfo = new DirectoryInfo(rootPath);
                var rootItem = new FileSystemItem
                {
                    Name = rootDirInfo.Name,
                    FullPath = rootDirInfo.FullName,
                    IsDirectory = true
                };

                if (Directory.Exists(rootItem.FullPath))
                {
                    try
                    {
                        if (Directory.EnumerateFileSystemEntries(rootItem.FullPath).Any())
                        {
                            rootItem.Children.Add(new FileSystemItem { Name = "Loading..." });
                        }
                    }
                    catch (UnauthorizedAccessException) { /* Ignore */ }
                }

                DirectoryTreeView.Items.Add(rootItem);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error loading root directory: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                CurrentDirectoryPathTextBlock.Text = "Error loading directory.";
            }
        }

        private void PopulateChildren(FileSystemItem parentItem)
        {
            if (parentItem == null || !parentItem.IsDirectory) return;

            parentItem.Children.Clear();
            try
            {
                var dirInfo = new DirectoryInfo(parentItem.FullPath);

                foreach (var subDir in dirInfo.GetDirectories())
                {
                    var subDirItem = new FileSystemItem { Name = subDir.Name, FullPath = subDir.FullName, IsDirectory = true };
                    try
                    {
                        if (Directory.Exists(subDirItem.FullPath) && Directory.EnumerateFileSystemEntries(subDirItem.FullPath).Any())
                        {
                            subDirItem.Children.Add(new FileSystemItem { Name = "Loading..." });
                        }
                    }
                    catch (UnauthorizedAccessException) { /* Ignore */ }
                    parentItem.Children.Add(subDirItem);
                }

                foreach (var fileInfo in dirInfo.GetFiles("*.msl"))
                {
                    parentItem.Children.Add(new FileSystemItem { Name = fileInfo.Name, FullPath = fileInfo.FullName, IsDirectory = false });
                }
            }
            catch (System.UnauthorizedAccessException) { parentItem.Children.Add(new FileSystemItem { Name = "Access Denied", IsDirectory = false }); }
            catch (System.Exception ex) { parentItem.Children.Add(new FileSystemItem { Name = $"Error: {ex.Message.Substring(0, System.Math.Min(ex.Message.Length, 30))}...", IsDirectory = false }); }
        }


        private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is TreeViewItem treeViewItem && treeViewItem.DataContext is FileSystemItem fsItem)
            {
                if (fsItem.IsDirectory && fsItem.Children.Count == 1 && fsItem.Children[0].Name == "Loading...")
                {
                    PopulateChildren(fsItem);
                }
            }
        }

        private void DirectoryTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            _currentOpenFilePath = null;
            AvalonTextEditor.IsModified = false;

            if (e.NewValue is FileSystemItem selectedItem)
            {
                if (!selectedItem.IsDirectory &&
                    selectedItem.Name != "Loading..." &&
                    selectedItem.Name != "Access Denied" &&
                    !selectedItem.Name.StartsWith("Error:"))
                {
                    try
                    {
                        string fileContent = File.ReadAllText(selectedItem.FullPath);
                        AvalonTextEditor.Text = fileContent;
                        _currentOpenFilePath = selectedItem.FullPath;
                        AvalonTextEditor.IsModified = false;
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show($"Error reading file '{selectedItem.Name}':\n{ex.Message}", "File Read Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        AvalonTextEditor.Text = $"// Error: Could not read file '{selectedItem.FullPath}'\n// {ex.Message}";
                        AvalonTextEditor.IsModified = false;
                    }
                }
                else if (selectedItem.IsDirectory)
                {
                    AvalonTextEditor.Text = $"// Selected directory: {selectedItem.FullPath}";
                    AvalonTextEditor.IsModified = false;
                }
            }
            UpdateRunButtonState();
            UpdateSaveButtonState();
            UpdateTitle();
        }

        private void UpdateRunButtonState()
        {
            RunButton.IsEnabled = !string.IsNullOrEmpty(_currentOpenFilePath) &&
                                  _currentOpenFilePath.EndsWith(".msl", StringComparison.OrdinalIgnoreCase);
        }

        private void UpdateSaveButtonState()
        {
            SaveFileButton.IsEnabled = AvalonTextEditor.IsModified && !string.IsNullOrEmpty(_currentOpenFilePath);
        }

        private void UpdateTitle()
        {
            string baseTitle = "MathSol IDE";
            if (!string.IsNullOrEmpty(_currentOpenFilePath))
            {
                string modifiedIndicator = AvalonTextEditor.IsModified ? "*" : "";
                this.Title = $"{baseTitle} - {Path.GetFileName(_currentOpenFilePath)}{modifiedIndicator}";
            }
            else
            {
                this.Title = baseTitle;
            }
        }

        private void AvalonTextEditor_TextChanged(object sender, EventArgs e)
        {
            UpdateSaveButtonState();
            UpdateTitle();
        }


        private async void RunButton_Click(object sender, RoutedEventArgs e)
        {
            if (!RunButton.IsEnabled || string.IsNullOrEmpty(_currentOpenFilePath))
            {
                MessageBox.Show("Please open a .msl file to run.", "No .msl File", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (AvalonTextEditor.IsModified)
            {
                MessageBoxResult saveResult = MessageBox.Show($"File '{Path.GetFileName(_currentOpenFilePath)}' has unsaved changes. Save before running?",
                                                              "Unsaved Changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (saveResult == MessageBoxResult.Yes)
                {
                    SaveFile();
                    if (AvalonTextEditor.IsModified)
                    {
                        MessageBox.Show("File not saved. Execution cancelled.", "Save Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                else if (saveResult == MessageBoxResult.Cancel)
                {
                    return;
                }
            }


            OutputConsole.Text = $">>> Running {Path.GetFileName(_currentOpenFilePath)}...\n";
            OutputScrollViewer.ScrollToBottom();

            var consoleWriter = new TextBlockWriter(OutputConsole, OutputScrollViewer, (action) => Dispatcher.Invoke(action));

            if (_originalConsoleOut == null) _originalConsoleOut = Console.Out;
            Console.SetOut(consoleWriter);

            try
            {
                var codeFile = MathSol.Interpreter.FileSystem.CodeFile.FromProcessedFile(_currentOpenFilePath, AvalonTextEditor.Text);

                await System.Threading.Tasks.Task.Run(() =>
                {
                    var interpreter = new MathSol.Interpreter.Interpreter();
                    interpreter.Interpret(codeFile);
                });
                Dispatcher.Invoke(() => OutputConsole.Text += $"\n>>> Execution finished.\n");
            }
            catch (System.Exception ex)
            {
                Dispatcher.Invoke(() => OutputConsole.Text += $"\n--- RUNTIME ERROR ---\n{ex.ToString()}\n---------------------\n");
            }
            finally
            {
                Console.SetOut(_originalConsoleOut);
                Dispatcher.Invoke(() => OutputScrollViewer.ScrollToBottom());
            }
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        private void SaveFile()
        {
            if (string.IsNullOrEmpty(_currentOpenFilePath))
            {
                MessageBox.Show("No file is currently open to save.", "Save Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                File.WriteAllText(_currentOpenFilePath, AvalonTextEditor.Text);
                AvalonTextEditor.IsModified = false;
                MessageBox.Show($"File '{Path.GetFileName(_currentOpenFilePath)}' saved successfully.", "File Saved", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file '{_currentOpenFilePath}':\n{ex.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            UpdateSaveButtonState();
            UpdateTitle();
        }

        private string ValidateFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return "File name cannot be empty.";
            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                return "File name contains invalid characters.";
            if (!fileName.EndsWith(".msl", StringComparison.OrdinalIgnoreCase))
                return "File name must end with .msl";
            return null; // No error
        }

        private void NewFileButton_Click(object sender, RoutedEventArgs e)
        {
            string targetDirectory = CurrentDirectoryPathTextBlock.Text;

            if (string.IsNullOrWhiteSpace(targetDirectory) || !Directory.Exists(targetDirectory))
            {
                if (DirectoryTreeView.SelectedItem is FileSystemItem selectedFsItem)
                {
                    targetDirectory = selectedFsItem.IsDirectory ? selectedFsItem.FullPath : Path.GetDirectoryName(selectedFsItem.FullPath);
                }
                else if (DirectoryTreeView.Items.Count > 0 && DirectoryTreeView.Items[0] is FileSystemItem rootFsItem && rootFsItem.IsDirectory)
                {
                    targetDirectory = rootFsItem.FullPath;
                }
                else
                {
                    MessageBox.Show("Please select a directory in the Explorer or use 'Choose Directory' to set a root working directory.", "New File Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(targetDirectory) || !Directory.Exists(targetDirectory))
            {
                MessageBox.Show($"Target directory '{targetDirectory}' is not valid or does not exist.", "New File Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            InputDialog inputDialog = new InputDialog("New MSL File", "Enter file name (e.g., script.msl):", "Untitled.msl", ValidateFileName);
            inputDialog.Owner = this;
            if (inputDialog.ShowDialog() == true)
            {
                string newFileName = inputDialog.InputText;
                string newFilePath = Path.Combine(targetDirectory, newFileName);

                if (File.Exists(newFilePath))
                {
                    MessageBox.Show($"File '{newFileName}' already exists in this directory.", "File Exists", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    File.WriteAllText(newFilePath, "// New MathSol File\n");

                    FileSystemItem parentNodeToRefresh = null;
                    if (DirectoryTreeView.ItemsSource != null)
                    {
                        parentNodeToRefresh = FindFileSystemItem(DirectoryTreeView.ItemsSource, targetDirectory);
                    }

                    if (parentNodeToRefresh != null && parentNodeToRefresh.IsDirectory)
                    {
                        if (parentNodeToRefresh.Children.Any() && parentNodeToRefresh.Children[0].Name != "Loading...")
                        {
                            PopulateChildren(parentNodeToRefresh);
                        }
                        else if (!parentNodeToRefresh.Children.Any(c => c.Name == "Loading..."))
                        {
                            if (parentNodeToRefresh.Children.Count == 0)
                            {
                                if (Directory.EnumerateFileSystemEntries(parentNodeToRefresh.FullPath).Any())
                                {
                                    parentNodeToRefresh.Children.Add(new FileSystemItem { Name = "Loading..." });
                                }
                            }
                            else
                            {
                                PopulateChildren(parentNodeToRefresh);
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(CurrentDirectoryPathTextBlock.Text) &&
                            Directory.Exists(CurrentDirectoryPathTextBlock.Text) &&
                            Path.GetDirectoryName(newFilePath).Equals(Path.GetFullPath(CurrentDirectoryPathTextBlock.Text), StringComparison.OrdinalIgnoreCase))
                        {
                            LoadInitialDirectory(CurrentDirectoryPathTextBlock.Text);
                        }
                    }

                    _currentOpenFilePath = newFilePath;
                    AvalonTextEditor.Text = File.ReadAllText(newFilePath);
                    AvalonTextEditor.IsModified = false;
                    UpdateRunButtonState();
                    UpdateSaveButtonState();
                    UpdateTitle();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating new file '{newFilePath}':\n{ex.Message}", "New File Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RenameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.Tag is FileSystemItem fsItemToRename)
            {
                string oldName = fsItemToRename.Name;
                string prompt = fsItemToRename.IsDirectory ? "Enter new directory name:" : "Enter new file name (with .msl extension):";

                Func<string, string> validationFunc = newName => {
                    if (string.IsNullOrWhiteSpace(newName)) return "Name cannot be empty.";
                    if (newName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0) return "Name contains invalid characters.";
                    if (!fsItemToRename.IsDirectory && !newName.EndsWith(".msl", StringComparison.OrdinalIgnoreCase)) return "File name must end with .msl";
                    if (newName.Equals(oldName, StringComparison.OrdinalIgnoreCase)) return null; // No change is not an error

                    string parentDir = Path.GetDirectoryName(fsItemToRename.FullPath);
                    if (Directory.Exists(Path.Combine(parentDir, newName)) || File.Exists(Path.Combine(parentDir, newName)))
                    {
                        return $"An item named '{newName}' already exists in this location.";
                    }
                    return null; // No error
                };

                InputDialog inputDialog = new InputDialog("Rename Item", prompt, oldName, validationFunc);
                inputDialog.Owner = this;

                if (inputDialog.ShowDialog() == true)
                {
                    string newName = inputDialog.InputText;
                    if (newName.Equals(oldName, StringComparison.OrdinalIgnoreCase)) return; // No actual change

                    string parentDirectoryPath = Path.GetDirectoryName(fsItemToRename.FullPath);
                    string newFullPath = Path.Combine(parentDirectoryPath, newName);

                    try
                    {
                        if (fsItemToRename.IsDirectory)
                        {
                            Directory.Move(fsItemToRename.FullPath, newFullPath);
                        }
                        else
                        {
                            File.Move(fsItemToRename.FullPath, newFullPath);
                        }

                        // Update the FileSystemItem in the TreeView
                        // This is tricky because we need to find its parent and refresh.
                        // A simpler way for now is to refresh the parent node if it's loaded,
                        // or the root if the renamed item was a root item.

                        if (_currentOpenFilePath == fsItemToRename.FullPath)
                        {
                            _currentOpenFilePath = newFullPath; // Update path if the open file was renamed
                            // The content in AvalonTextEditor is still the same, but IsModified might need re-evaluation
                            // or we just keep it as is. The title will update.
                            UpdateTitle();
                        }

                        // Find parent to refresh
                        FileSystemItem parentNode = FindParentFileSystemItem(DirectoryTreeView.ItemsSource, fsItemToRename);
                        if (parentNode != null)
                        {
                            PopulateChildren(parentNode); // Refresh parent's children
                        }
                        else if (DirectoryTreeView.Items.Contains(fsItemToRename)) // It was a root item
                        {
                            LoadInitialDirectory(CurrentDirectoryPathTextBlock.Text); // Reload root
                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error renaming '{oldName}' to '{newName}':\n{ex.Message}", "Rename Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        // Helper to find a FileSystemItem by path (recursive)
        private FileSystemItem FindFileSystemItem(IEnumerable items, string path)
        {
            if (items == null) return null;
            foreach (object objItem in items)
            {
                if (objItem is FileSystemItem item)
                {
                    if (item.FullPath.Equals(path, StringComparison.OrdinalIgnoreCase)) return item;
                    if (item.IsDirectory && item.Children.Any())
                    {
                        FileSystemItem foundInChild = FindFileSystemItem(item.Children, path);
                        if (foundInChild != null) return foundInChild;
                    }
                }
            }
            return null;
        }

        // Helper to find the parent of a FileSystemItem
        private FileSystemItem FindParentFileSystemItem(IEnumerable items, FileSystemItem childToFind)
        {
            if (items == null) return null;
            foreach (object objItem in items)
            {
                if (objItem is FileSystemItem currentParent)
                {
                    if (currentParent.IsDirectory && currentParent.Children.Contains(childToFind))
                    {
                        return currentParent;
                    }
                    FileSystemItem foundInChildsChildren = FindParentFileSystemItem(currentParent.Children, childToFind);
                    if (foundInChildsChildren != null)
                    {
                        return foundInChildsChildren;
                    }
                }
            }
            return null;
        }
    }
}
