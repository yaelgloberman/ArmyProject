using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
namespace ArmyExe2.Models
{
    public class Username
    {
        public string Id { get; set; }


        public string FullName { get; set; }

        public string StreetAddress { get; set; }

        public string Phone { get; set; }
        public string MobilePhone { get; set; }

        public DateTime BirthDate { get; set; }
        
        public string? ImageData { get; set; }
        public DateTime? PositiveForCorona { get; set; }
        public DateTime? RecoverFromCorona { get; set; }

    }
}
