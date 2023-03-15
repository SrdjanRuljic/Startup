namespace Domain.Entities
{
    public class Connection
    {
        public Group Group { get; set; }
        public string GroupName { get; set; }
        public string Id { get; set; }
        public string UserId { get; set; }

        public Connection()
        {
        }

        public Connection(string id, string userId)
        {
            Id = id;
            UserId = userId;
        }
    }
}