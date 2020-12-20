namespace HandiworkShop.DAL.Entities
{
    /// <summary>
    /// Entity Task.
    /// </summary>
    public class Task : TaskBase
    {
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