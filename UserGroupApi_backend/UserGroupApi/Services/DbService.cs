using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using UserGroupApi.Models;

namespace UserGroupApi.Services
{
    public class DbService
    {
        private readonly string _connectionString;

        public DbService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task AddUserAsync(UserModel userModel)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("AddUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserName", userModel.UserName);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task AddGroupAsync(GroupModel groupModel)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("AddGroup", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@GroupName", groupModel.GroupName);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<GroupModel>> GetAvailableGroupsForUserAsync(int userId)
        {
            var groups = new List<GroupModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("GetAvailableGroupsForUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            groups.Add(new GroupModel
                            {
                                GroupId = reader.GetInt32(reader.GetOrdinal("GroupId")),
                                GroupName = reader.GetString(reader.GetOrdinal("GroupName"))
                            });
                        }
                    }
                }
            }

            return groups;
        }
        public async Task AddUserToGroupAsync(int userId, int groupId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("AddUserToGroup", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@GroupId", groupId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<UserGroupsModel>> GetUsersWithGroupsAsync()
        {
            var usersWithGroupsJson = string.Empty;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("GetUsersWithGroupsJson", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            usersWithGroupsJson = reader.GetString(0);
                        }
                    }
                }
            }

            return JsonConvert.DeserializeObject<List<UserGroupsModel>>(usersWithGroupsJson) 
                ?? new List<UserGroupsModel>();
        }
    }
}
