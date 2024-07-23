namespace PflegeVerbundhilfe.TaskManagement.DataTransferObjects
{
    public class PagedRecord
    {
        public int TotalItems { get; private set; }
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public List<TodoTask> Items { get; private set; }

        public PagedRecord(int totalItems, int pageNumber, int pageSize, List<TodoTask> items)
        {
            TotalItems = totalItems;
            PageNumber = pageNumber;
            PageSize = pageSize;

            Items = items;
        }
    }
}
