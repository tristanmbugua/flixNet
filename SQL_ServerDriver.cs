using Microsoft.Data.SqlClient;
using Dapper;

namespace _301141338_Mbugua__LabThree
{
    public class SQL_ServerDriver
    {
        private string connectionString = @"
            Data Source=lab-three-data.cz6q0ucq2m4n.us-east-2.rds.amazonaws.com;
            Initial Catalog=lab_three;
            User ID=admin;Password=Centennial;Encrypt=False;";
        SqlConnection connection;

        public SQL_ServerDriver()
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        public void addUser(String username, String password)
        {
            connection.Query($"INSERT INTO [DBO].[users] VALUES (\'{username}\',\'{password}\')");
        }

        public Boolean authenticateUser() {
            List<dynamic> data = connection.Query("SELECT * FROM Users").ToList();
            foreach (dynamic item in data)
            {
                string username_ = item.username;
                string password_ = item.password;

                username_ = trimString(username_);
                password_ = trimString(password_);

                if (username_.Equals(UserAccountDriver.username) && password_.Equals(UserAccountDriver.password))
                    return true;
            }
            return false;
        }
        private String trimString(String _string)
        {
            String newString = "";
            Char[] stringArray = _string.ToArray();
            foreach (char n in stringArray)
            {
                if (n == ' ')
                {
                    continue;
                }
                else
                {
                    newString += n.ToString();
                }
            }
            return newString;
        }

    }
}
