using System; // Added for Func
using System.Windows;
// Explicitly qualify KeyEventArgs to avoid ambiguity
// using System.Windows.Input; // No longer needed if fully qualified below

namespace MathSol.IDE
{
    public partial class InputDialog : Window
    {
        public string InputText { get; private set; }
        private Func<string, string> _validationFunc; // Optional validation function

        // Constructor for simple input
        public InputDialog(string title, string prompt, string defaultValue = "")
        {
            InitializeComponent();
            this.Title = title;
            PromptLabel.Content = prompt;
            InputTextBox.Text = defaultValue;
            InputTextBox.Focus();
            InputTextBox.SelectAll();
        }

        // Constructor with validation
        public InputDialog(string title, string prompt, string defaultValue, Func<string, string> validationFunc)
            : this(title, prompt, defaultValue)
        {
            _validationFunc = validationFunc;
        }


        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (_validationFunc != null)
            {
                string validationMessage = _validationFunc(InputTextBox.Text);
                if (!string.IsNullOrEmpty(validationMessage))
                {
                    ValidationMessageTextBlock.Text = validationMessage;
                    ValidationMessageTextBlock.Visibility = Visibility.Visible;
                    return; // Don't close if validation fails
                }
            }
            InputText = InputTextBox.Text;
            DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        // Explicitly use System.Windows.Input.KeyEventArgs
        private void InputTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                OkButton_Click(sender, e);
            }
            else if (e.Key == System.Windows.Input.Key.Escape)
            {
                CancelButton_Click(sender, e);
            }
        }
    }
}
