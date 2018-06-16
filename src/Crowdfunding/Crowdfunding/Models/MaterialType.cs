using System;
using System.Collections.Generic;

namespace Crowdfunding.Models
{
    public partial class MaterialType
    {
        public MaterialType()
        {
            Material = new HashSet<Material>();
        }

        public long MaterialTypeId { get; set; }
        public string TypeName { get; set; }

        public ICollection<Material> Material { get; set; }
    }
}
