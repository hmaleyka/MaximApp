using MaximApp.Models.Common;

namespace MaximApp.Models
{
    public class Services : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}
