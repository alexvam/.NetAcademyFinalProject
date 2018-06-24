using System;
using System.Collections.Generic;

namespace Crowdfunding.Models
{
    public partial class Material
    {
        public long MaterialId { get; set; }
        public long ProjectId { get; set; }
        public long? MaterialTypeId { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }

        public MaterialType MaterialType { get; set; }
        public Project Project { get; set; }
    }
}
