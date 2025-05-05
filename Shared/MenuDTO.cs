using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class MenuDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PizzaDTO Pizza { get; set; }
        public DrinkDTO Drink { get; set; }
        public ExtraDTO Extra { get; set; }
        public decimal Price { get; set; }
    }


}
