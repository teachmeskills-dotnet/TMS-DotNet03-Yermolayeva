namespace HandiworkShop.DAL.Entities
{
    /// <summary>
    /// Entity UserTag.
    /// </summary>
    public class UserTag
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
        /// User identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Navigation to user.
        /// </summary>
        public ApplicationUser User { get; set; }
    }
}