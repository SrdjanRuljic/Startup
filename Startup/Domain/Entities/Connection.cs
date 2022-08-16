namespace Domain.Entities
{
    public class Connection
    {
        public Group Group { get; set; }
        public string GroupName { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }

        public Connection()
        {
        }

        public Connection(string id, string userName)
        {
            Id = id;
            UserName = userName;
        }

        public Connection(string id, string userName, string groupName)
        {
            Id = id;
            UserName = userName;
            GroupName = groupName;
        }
    }
}