using System.Windows;
using PathEditor.Models;
using PathEditor.ModelViews;

namespace PathEditor.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new PathEditorViewModel(new PathRepository());
        }
    }
}
