namespace UserGroupApi.Models
{
    public class UserGroupsModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<GroupModel> Groups { get; set; } = new List<GroupModel>();
    }
}
