using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CafeManagemnt
{
    public class UserManager
    {
        private const string ConnectionString = @"Data Source=MOHAMMAD-LOQ;Initial Catalog=CafeManagement_New;Integrated Security=True;Connect Timeout=30";

        // Get all available roles
        public static List<Role> GetAllRoles()
        {
            List<Role> roles = new List<Role>();

            try
            {
                string query = "SELECT role_id, role_name, is_active FROM UserRoles";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roles.Add(new Role
                            {
                                RoleId = Convert.ToInt32(reader["role_id"]),
                                RoleName = reader["role_name"].ToString(),
                                IsActive = Convert.ToBoolean(reader["is_active"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error or handle exception
                throw new Exception("Error retrieving roles: " + ex.Message);
            }

            return roles;
        }

        // Get all users with their roles
        public static List<UserWithRoles> GetAllUsers()
        {
            Dictionary<int, UserWithRoles> usersDict = new Dictionary<int, UserWithRoles>();

            try
            {
                string query = @"
                    SELECT u.user_id, u.username, u.status, u.created_date, u.last_login, 
                           r.role_id, r.role_name, ura.is_active AS role_active
                    FROM Users u
                    LEFT JOIN UserRoleAssignments ura ON u.user_id = ura.user_id
                    LEFT JOIN UserRoles r ON ura.role_id = r.role_id
                    ORDER BY u.username";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int userId = Convert.ToInt32(reader["user_id"]);

                            if (!usersDict.ContainsKey(userId))
                            {
                                usersDict.Add(userId, new UserWithRoles
                                {
                                    UserId = userId,
                                    Username = reader["username"].ToString(),
                                    Status = Convert.ToBoolean(reader["status"]),
                                    CreatedDate = reader["created_date"] != DBNull.Value
                                        ? Convert.ToDateTime(reader["created_date"])
                                        : (DateTime?)null,
                                    LastLogin = reader["last_login"] != DBNull.Value
                                        ? Convert.ToDateTime(reader["last_login"])
                                        : (DateTime?)null,
                                    Roles = new List<UserRole>()
                                });
                            }

                            // Add role if it exists
                            if (reader["role_id"] != DBNull.Value)
                            {
                                usersDict[userId].Roles.Add(new UserRole
                                {
                                    RoleId = Convert.ToInt32(reader["role_id"]),
                                    RoleName = reader["role_name"].ToString(),
                                    IsActive = Convert.ToBoolean(reader["role_active"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving users: " + ex.Message);
            }

            return new List<UserWithRoles>(usersDict.Values);
        }

        // Assign a role to a user
        public static bool AssignRoleToUser(int userId, int roleId, string assignedBy)
        {
            try
            {
                string query = @"
                    INSERT INTO UserRoleAssignments (user_id, role_id, assigned_date, assigned_by, is_active)
                    VALUES (@userId, @roleId, @assignedDate, @assignedBy, 1)";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@roleId", roleId);
                    command.Parameters.AddWithValue("@assignedDate", DateTime.Now);
                    command.Parameters.AddWithValue("@assignedBy", assignedBy);

                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        // Deactivate a specific role for a user
        public static bool DeactivateUserRole(int userId, int roleId)
        {
            try
            {
                string query = @"
                    UPDATE UserRoleAssignments 
                    SET is_active = 0
                    WHERE user_id = @userId AND role_id = @roleId";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@roleId", roleId);

                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        // Activate a specific role for a user
        public static bool ActivateUserRole(int userId, int roleId)
        {
            try
            {
                string query = @"
                    UPDATE UserRoleAssignments 
                    SET is_active = 1
                    WHERE user_id = @userId AND role_id = @roleId";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@roleId", roleId);

                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        // Activate or deactivate a user
        public static bool SetUserStatus(int userId, bool active)
        {
            try
            {
                string query = @"
                    UPDATE Users 
                    SET status = @status
                    WHERE user_id = @userId";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@status", active ? 1 : 0);

                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        // Get user by username
        public static UserWithRoles GetUserByUsername(string username)
        {
            UserWithRoles user = null;

            try
            {
                string query = @"
                    SELECT u.user_id, u.username, u.status, u.created_date, u.last_login
                    FROM Users u
                    WHERE u.username = @username";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Get user basic information
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new UserWithRoles
                                {
                                    UserId = Convert.ToInt32(reader["user_id"]),
                                    Username = reader["username"].ToString(),
                                    Status = Convert.ToBoolean(reader["status"]),
                                    CreatedDate = reader["created_date"] != DBNull.Value
                                        ? Convert.ToDateTime(reader["created_date"])
                                        : (DateTime?)null,
                                    LastLogin = reader["last_login"] != DBNull.Value
                                        ? Convert.ToDateTime(reader["last_login"])
                                        : (DateTime?)null,
                                    Roles = new List<UserRole>()
                                };
                            }
                        }
                    }

                    // If user exists, get their roles
                    if (user != null)
                    {
                        string roleQuery = @"
                            SELECT r.role_id, r.role_name, ura.is_active
                            FROM UserRoles r
                            JOIN UserRoleAssignments ura ON r.role_id = ura.role_id
                            WHERE ura.user_id = @userId";

                        using (SqlCommand command = new SqlCommand(roleQuery, connection))
                        {
                            command.Parameters.AddWithValue("@userId", user.UserId);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    user.Roles.Add(new UserRole
                                    {
                                        RoleId = Convert.ToInt32(reader["role_id"]),
                                        RoleName = reader["role_name"].ToString(),
                                        IsActive = Convert.ToBoolean(reader["is_active"])
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving user: " + ex.Message);
            }

            return user;
        }
    }

    // Helper classes for user and role management
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserRole
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserWithRoles
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public List<UserRole> Roles { get; set; }
    }
}