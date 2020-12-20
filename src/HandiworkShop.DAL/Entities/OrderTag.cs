namespace HandiworkShop.DAL.Entities
{
    /// <summary>
    /// Entity OrderTag.
    /// </summary>
    public class OrderTag
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tag identifier.
        /// </summary>
        public int TagId { get; set; }

        /// <summary>
        /// Navigation to tag.
        /// </summary>
        public Tag Tag { get; set; }

        /// <summary>
        /// Order identifier.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Navigation to order.
        /// </summary>
        public Order Order { get; set; }
    }
}