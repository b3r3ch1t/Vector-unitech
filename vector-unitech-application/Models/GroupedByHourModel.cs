using System;
using System.Collections.Generic;

namespace vector_unitech_application.Models
{
    public class GroupedByHourModel
    {
        public DateTime Data { get; set; }
        public IList<string> Nomes { get; set; }
    }
}
