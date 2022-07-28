namespace Tree
{
    public class NodeEntity
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public virtual NodeEntity Parent { get; set; }
        public int? ParentId { get; set; }

        public virtual ICollection<NodeEntity> Children { get; set; }
    }
}
