using System;

namespace UniClubManagementSystem
{
    public static class UserSession
    {
        public static int UserID { get; set; }
        public static string FullName { get; set; }
        public static string Role { get; set; }

        public static void ClearSession()
        {
            UserID = 0;
            FullName = null;
            Role = null;
        }
    }
}