using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MonetaAPI.Models
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

    public class CreateTransactionRequest : TransactionsRequest
    {
        public Transaction Transaction { get; set; }
    }

    public class DeleteCategoryRequest 
    {
        public Guid deletedCategory { get; set; }
        public Nullable<Guid> newCategory { get; set; }
    }
}
