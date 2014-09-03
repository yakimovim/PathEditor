using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PathEditor.Models;

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

            PathParts = new ObservableCollection<PathPartViewModel>(_repository.GetPathParts().Select(p => new PathPartViewModel(p)));
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
                    var newPathPart = new PathPartViewModel("???");
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

        public ICommand SetPath
        {
            get
            {
                return new DelegateCommand(arg =>
                {
                    _repository.SetPathFromParts(PathParts.Select(p => p.Path).ToArray());
                    Application.Current.Shutdown();
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