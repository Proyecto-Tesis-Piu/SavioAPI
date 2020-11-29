using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MonetaAPI.Models
{
    public class Feedback
    {
        [Column("User Id")]
        public Guid? UserId { get; set; }

        [Column("Id")]
        public Guid? FeedbackId { get; set; }

        [Column("Title")]
        [StringLength(150)]
        public String Title { get; set; }

        [Column("Details")]
        [StringLength(5000)]
        public String Details { get; set; }
    }
}
