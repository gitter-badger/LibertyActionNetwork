using System.Collections.Generic;
using System.Data;

namespace Li.Lan.Data
{
    public class CollectionToDataTableConverter
    {
        public static DataTable CreateDataTable(IEnumerable<int> collection)
        {
            return CreateDataTable(collection, "Id");
        }

        public static DataTable CreateDataTable(IEnumerable<int> collection, string columnName)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(columnName, typeof(int));

            if (collection != null)
            {
                foreach (var item in collection)
                {
                    dataTable.Rows.Add(item);
                }
            }

            return dataTable;
        }

        public static DataTable CreateDataTable(IEnumerable<short> collection)
        {
            return CreateDataTable(collection, "Id");
        }

        public static DataTable CreateDataTable(IEnumerable<short> collection, string columnName)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(columnName, typeof(short));

            if (collection != null)
            {
                foreach (var item in collection)
                {
                    dataTable.Rows.Add(item);
                }
            }

            return dataTable;
        }
    }
}