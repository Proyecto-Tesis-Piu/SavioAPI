using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SavioAPI.Models
{
    public class TransactionsRequest
    {
        [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> FromDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> ToDate { get; set; }
    }

    public class DeleteTransactionRequest : TransactionsRequest { 
        public Guid TransactionID { get; set; }
    }
}
