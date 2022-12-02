using AAH_AutoSim.TestCase.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AAH_AutoSim.TestCase.Util
{
    /// <summary>
    /// This is an attached property for the View. The purpose of this class is to auto scroll down the bottom item.
    /// Reference: https://stackoverflow.com/questions/1027051/how-to-autoscroll-on-wpf-datagrid/24645286#24645286
    /// There's an bug in function Subscribe() from the original code. After fixed that bug, all is well.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DataGridBehavior
    {
        // Register an attached dependency property with the specified
        // property name, property type, owner type, and property metadata.
        public static readonly DependencyProperty AutoScrollProperty = DependencyProperty.RegisterAttached(
            "AutoScroll", typeof(bool), typeof(DataGridBehavior), new PropertyMetadata(default(bool), AutoScrollChangedCallback));

        private static readonly Dictionary<DataGrid, NotifyCollectionChangedEventHandler> handlersDict = new Dictionary<DataGrid, NotifyCollectionChangedEventHandler>();

        private static void AutoScrollChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var dataGrid = dependencyObject as DataGrid;
            if (dataGrid == null)
            {
                throw new InvalidOperationException("Dependency object is not DataGrid.");
            }

            if ((bool)args.NewValue)
            {
                Subscribe(dataGrid);
                dataGrid.Unloaded += DataGridOnUnloaded;
                dataGrid.Loaded += DataGridOnLoaded;
            }
            else
            {
                Unsubscribe(dataGrid);
                dataGrid.Unloaded -= DataGridOnUnloaded;
                dataGrid.Loaded -= DataGridOnLoaded;
            }
        }

        private static void Subscribe(DataGrid dataGrid)
        {
            var handler = new NotifyCollectionChangedEventHandler((sender, eventArgs) => ScrollToEnd(dataGrid));
            handlersDict[dataGrid] = handler;
            ((INotifyCollectionChanged)dataGrid.Items).CollectionChanged += handler;
            ScrollToEnd(dataGrid);
        }

        private static void Unsubscribe(DataGrid dataGrid)
        {
            NotifyCollectionChangedEventHandler handler;
            handlersDict.TryGetValue(dataGrid, out handler);
            if (handler == null)
            {
                return;
            }
            ((INotifyCollectionChanged)dataGrid.Items).CollectionChanged -= handler;
            handlersDict.Remove(dataGrid);
        }

        private static void DataGridOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var dataGrid = (DataGrid)sender;
            if (GetAutoScroll(dataGrid))
            {
                Subscribe(dataGrid);
            }
        }

        private static void DataGridOnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var dataGrid = (DataGrid)sender;
            if (GetAutoScroll(dataGrid))
            {
                Unsubscribe(dataGrid);
            }
        }

        private static void ScrollToEnd(DataGrid datagrid)
        {
            if (datagrid.Items.Count == 0)
            {
                return;
            }
            datagrid.ScrollIntoView(datagrid.Items[datagrid.Items.Count - 1]);
        }

        // Declare a set accessor method.
        public static void SetAutoScroll(DependencyObject element, bool value)
        {
            element.SetValue(AutoScrollProperty, value);
        }

        // Declare a get accessor method.
        public static bool GetAutoScroll(DependencyObject element)
        {
            return (bool)element.GetValue(AutoScrollProperty);
        }
    }
}
