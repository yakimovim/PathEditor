using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using PathEditor.Models;

namespace PathEditor.ModelViews
{
    internal class MainViewModel : BaseViewModel
    {
        private readonly Person _person;

        [DebuggerStepThrough]
        public MainViewModel(Person person)
        {
            if (person == null) throw new ArgumentNullException("person");
            _person = person;
        }

        public string Name
        {
            get { return _person.Name; }
            set
            {
                if (value != _person.Name)
                {
                    _person.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SayHello
        {
            get
            {
                return new DelegateCommand((arg) =>
                {
                    MessageBox.Show(string.Format("Hello, {0}", Name));
                },
                (arg) => !string.IsNullOrWhiteSpace(Name));
            }
        }
    }
}
