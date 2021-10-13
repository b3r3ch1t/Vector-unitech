using System;
using System.Collections.Generic;

namespace vector_unitech_core.Entities
{
    public class GroupedByHour
    {
        public DateTime CreatedAt { get; set; }

        public List<TestEntity> ListEntity { get; set; }
    }
}
