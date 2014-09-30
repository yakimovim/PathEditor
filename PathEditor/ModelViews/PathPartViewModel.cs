using System;
using System.Diagnostics;
using PathEditor.Models;

namespace PathEditor.ModelViews
{
    internal class PathPartViewModel : BaseViewModel
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _pathPart;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _index;

        [DebuggerStepThrough]
        public PathPartViewModel(string pathPart, int index)
        {
            if (pathPart == null) throw new ArgumentNullException("pathPart");
            _pathPart = pathPart;
            Index = index;
        }

        public string Path
        {
            [DebuggerStepThrough]
            get { return _pathPart; }
            [DebuggerStepThrough]
            set
            {
                if (_pathPart != value)
                {
                    _pathPart = value;
                    OnPropertyChanged();
                    OnPropertyChanged("Exists");
                }
            }
        }

        public int Index
        {
            [DebuggerStepThrough]
            get { return _index; }
            [DebuggerStepThrough]
            set
            {
                if (_index != value)
                {
                    _index = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool Exists
        {
            [DebuggerStepThrough]
            get { return Path.ExpandedDirectoryExists(); }
        }

        [DebuggerStepThrough]
        public override string ToString()
        {
            return _pathPart;
        }
    }
}