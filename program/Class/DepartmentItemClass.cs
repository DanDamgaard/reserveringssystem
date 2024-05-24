using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program.Class
{
    public class DepartmentItemClass
    {
        public DepartmentItemClass(int id, int itemId, string itemNo, int departmentId, string status, string customerName, string customerPhone, DateTime? startDate, DateTime? endDate, string departmentName, string itemName)
        {
            this.id = id;
            this.itemId = itemId;
            this.itemNo = itemNo;
            this.departmentId = departmentId;
            this.status = status;
            this.customerName = customerName;
            this.customerPhone = customerPhone;
            this.startDate = startDate;
            this.endDate = endDate;
            this.departmentName = departmentName;
            this.itemName = itemName;
        }

        public int id { get; set; }
        public int itemId { get; set; }
        public string itemNo { get; set; } = string.Empty;
        public int departmentId { get; set; }
        public string status { get; set; } = string.Empty;
        public string customerName { get; set; } = string.Empty;
        public string customerPhone { get; set; } = string.Empty;
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string departmentName { get; set; } = string.Empty;
        public string itemName { get; set; } = string.Empty;

        public string startDateString { get { return dateConvert(startDate); } }
        public string endDateString { get { return dateConvert(endDate); } }

        public string dateConvert(DateTime? dt)
        {
            string newDate = "";

            if (dt != null)
            {
                string tempDate = dt.ToString();

                DateTime date = DateTime.Parse(tempDate);

                newDate = date.ToString("dd-MM-yyyy");
            }

            return newDate;
        }
    }
}
