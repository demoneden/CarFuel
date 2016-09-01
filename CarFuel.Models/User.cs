using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFuel.Models {
    public class User {
        
        [Key]
        public Guid UserId { get; set; }
        
        [Required]
        public string DisplayName { get; set; }

        [Required]
        public ConsumptionRateUnit ConsumptionRate { get; set; }

        public virtual ICollection<Car> Cars { get; set; }

        public User() {
            Cars = new HashSet<Car>();
        }

    }
}
