using System.Collections.Generic;

namespace NorthwindMvc.Models.EmployeeModels
{
    public class EmployeesListModel
    {
        public IEnumerable<EmployeeModel> Employees { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
