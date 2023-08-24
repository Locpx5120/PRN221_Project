using System;
using System.Collections.Generic;

namespace DauTayShop.Models
{
    public partial class Bill
    {
        public Bill()
        {
            BillDetails = new HashSet<BillDetail>();
        }

        public int Id { get; set; }
        public DateTime DateCheckIn { get; set; }
        public DateTime? DateCheckOut { get; set; }
        public int IdTable { get; set; }
        public int Status { get; set; }

        public virtual TableFood IdTableNavigation { get; set; } = null!;
        public virtual ICollection<BillDetail> BillDetails { get; set; }
    }
}
