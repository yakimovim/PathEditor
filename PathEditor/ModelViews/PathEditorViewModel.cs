using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using PathEditor.Models;
using PathEditor.Views.UserControls;
using Application = System.Windows.Application;

namespace PathEditor.ModelViews
{
    internal class PathEditorViewModel : BaseViewModel
    {
        private readonly PathRepository _repository;
        private PathPartViewModel _selectedPathPart;

        [DebuggerStepThrough]
        public PathEditorViewModel(PathRepository repository)
        {
            if (repository == null) throw new ArgumentNullException("repository");
            _repository = repository;

            PathParts = new ObservableCollection<PathPartViewModel>(_repository.GetPathParts().Select((p, i) => new PathPartViewModel(p, i + 1)));
        }

        public ObservableCollection<PathPartViewModel> PathParts { get; private set; }

        public PathPartViewModel SelectedPathPart
        {
            [DebuggerStepThrough]
            get { return _selectedPathPart; }
            [DebuggerStepThrough]
            set
            {
                if (_selectedPathPart != value)
                {
                    _selectedPathPart = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AddNewPathPart
        {
            get
            {
                return new DelegateCommand(arg =>
                {
                    var newPathPart = new PathPartViewModel("???", PathParts.Count + 1);
                    PathParts.Add(newPathPart);
                    SelectedPathPart = newPathPart;
                });
            }
        }

        public ICommand DeleteSelectedPathPart
        {
            get
            {
                return new DelegateCommand(arg =>
                {
                    PathParts.Remove(SelectedPathPart);
                    SelectedPathPart = null;
                },
                arg => SelectedPathPart != null);
            }
        }

        public ICommand MovePathPartUp
        {
            get
            {
                return new DelegateCommand(arg =>
                {
                    var previousPathPart = PathParts.First(p => p.Index == SelectedPathPart.Index - 1);

                    SwapIndexes(SelectedPathPart, previousPathPart);

                    ((SortableListView) arg).Refresh();
                },
                arg => SelectedPathPart != null && SelectedPathPart.Index > 1);
            }
        }

        public ICommand MovePathPartDown
        {
            get
            {
                return new DelegateCommand(arg =>
                {
                    var nextPathPart = PathParts.First(p => p.Index == SelectedPathPart.Index + 1);

                    SwapIndexes(SelectedPathPart, nextPathPart);

                    ((SortableListView) arg).Refresh();
                },
                arg => SelectedPathPart != null && SelectedPathPart.Index < PathParts.Count);
            }
        }

        private void SwapIndexes(PathPartViewModel part1, PathPartViewModel part2)
        {
            var index = part1.Index;
            part1.Index = part2.Index;
            part2.Index = index;
        }

        public ICommand BrowsePath
        {
            get
            {
                return new DelegateCommand(arg =>
                {
                    var dlg = new FolderBrowserDialog();
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        SelectedPathPart.Path = dlg.SelectedPath;
                    }
                },
                arg => SelectedPathPart != null);
            }
        }

        public ICommand SetPath
        {
            get
            {
                return new DelegateCommand(arg =>
                {
                    _repository.SetPathFromParts(PathParts.OrderBy(p => p.Index).Select(p => p.Path).ToArray());
                    Application.Current.Shutdown();
                });
            }
        }

        public ICommand AutoComplete
        {
            get
            {
                return new DelegateCommand(arg =>
                {
                    var textBox = (AutoCompleteTextBox) arg;

                    var autoCompleteProvider = AutoCompleteProviderFactory.GetAutoCompleteProvider(textBox.Text);

                    textBox.AppendSelectedText(autoCompleteProvider.GetAutoCompleteText());
                });
            }
        }

        public ICommand Exit
        {
            get
            {
                return new DelegateCommand(arg => Application.Current.Shutdown());
            }
        }
    }
}