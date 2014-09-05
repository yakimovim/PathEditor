using System.Windows;
using System.Windows.Input;

namespace PathEditor.Views.UserControls
{
    public class AutoCompleteTextBox : System.Windows.Controls.TextBox
    {
        public static ICommand GetAutoCompleteCommand(DependencyObject obj)
        { return (ICommand)obj.GetValue(DialogResultProperty); }

        public static void SetAutoCompleteCommand(DependencyObject obj, ICommand value)
        { obj.SetValue(DialogResultProperty, value); }

        public static readonly DependencyProperty DialogResultProperty = DependencyProperty.Register("AutoCompleteCommand",
            typeof(ICommand),
            typeof(AutoCompleteTextBox));

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (CaretIndex == Text.Length)
            {
                if (e.Key == Key.Delete || e.Key == Key.Back)
                { return; }

                var autoCompleteCommand = GetAutoCompleteCommand(this);
                if (autoCompleteCommand != null && autoCompleteCommand.CanExecute(this))
                {
                    autoCompleteCommand.Execute(this);
                }
            }
        }

        public void AppendSelectedText(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            { return; }

            var currentEndPosition = Text.Length;

            Text += textToAppend;

            CaretIndex = currentEndPosition;
            SelectionStart = currentEndPosition;
            SelectionLength = textToAppend.Length;
        }
    }
}
