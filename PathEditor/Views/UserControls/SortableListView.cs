using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;

namespace PathEditor.Views.UserControls
{
    public class SortableListView : ListView
    {
        private GridViewColumnHeader _listViewSortCol;
        private SortAdorner _listViewSortAdorner;

        public SortableListView()
        {
            AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(GridViewColumnHeaderClickedHandler));
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);

            SortByFirstColumn();
        }

        private void SortByFirstColumn()
        {
            if (View is GridView)
            {
                if (((GridView) View).Columns.Count > 0)
                {
                    var columnHeader = ((GridView) View).Columns[0].Header as GridViewColumnHeader;
                    SetSorting(columnHeader);
                }
            }
        }

        private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader columnHeader = (sender as GridViewColumnHeader) ?? (e.OriginalSource as GridViewColumnHeader);

            SetSorting(columnHeader);
        }

        private void SetSorting(GridViewColumnHeader columnHeader)
        {
            if (columnHeader == null)
            { return; }

            string sortBy = columnHeader.Tag.ToString();
            if (_listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(_listViewSortCol).Remove(_listViewSortAdorner);
                Items.SortDescriptions.Clear();
            }

            var newDir = ListSortDirection.Ascending;
            if (_listViewSortCol == columnHeader && _listViewSortAdorner.Direction == newDir)
            {
                newDir = ListSortDirection.Descending;
            }

            _listViewSortCol = columnHeader;
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