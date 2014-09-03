using PathEditor.Models;
using PathEditor.ModelViews;

namespace PathEditor.Views
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new PathEditorViewModel(new PathRepository());
        }
    }
}
