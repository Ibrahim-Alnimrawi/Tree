using Microsoft.AspNetCore.Mvc;

namespace Tree
{
    public class NodeModel
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public int? ParentId { get; set; }
    }
}
