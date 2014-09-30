using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace PathEditor.Views.UserControls
{
    public class SortableListView : ListView
    {
        private GridViewColumnHeader _listViewSortCol;
        private SortAdorner _listViewSortAdorner;

        public SortableListView()
        {
            AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(GridViewColumnHeaderClickedHandler));
        }

        private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader) ?? (e.OriginalSource as GridViewColumnHeader);
            if (column == null)
            { return; }

            string sortBy = column.Tag.ToString();
            if (_listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(_listViewSortCol).Remove(_listViewSortAdorner);
                Items.SortDescriptions.Clear();
            }

            var newDir = ListSortDirection.Ascending;
            if (_listViewSortCol == column && _listViewSortAdorner.Direction == newDir)
            {
                newDir = ListSortDirection.Descending;
            }

            _listViewSortCol = column;
            _listViewSortAdorner = new SortAdorner(_listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(_listViewSortCol).Add(_listViewSortAdorner);
            Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        public void Refresh()
        {
            var sortDescriptions = Items.SortDescriptions.ToList();
            Items.SortDescriptions.Clear();
            sortDescriptions.ForEach(sd => Items.SortDescriptions.Add(sd));
        }
    }
}