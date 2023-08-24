using System;
using System.Collections.Generic;

namespace DauTayShop.Models
{
    public partial class Food
    {
        public Food()
        {
            BillDetails = new HashSet<BillDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int IdCategory { get; set; }
        public double Price { get; set; }

        public virtual FoodCategory IdCategoryNavigation { get; set; } = null!;
        public virtual ICollection<BillDetail> BillDetails { get; set; }
    }
}
