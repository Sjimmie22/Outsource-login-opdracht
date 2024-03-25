using Interfaces;
using System.Data.SqlClient;
using Dapper;
using System.Text;
using System.Data.Sql;
using System.Security.Cryptography;

namespace LoginDAL
{
    public class UserRepository : IUserRepository
    {
        private readonly string connectionString;

        public UserRepository() //de connection string naar de database
        {
            connectionString = @"Server=(localdb)\mssqllocaldb;Database=Testlogindata;Integrated Security = true";
        }

        public UserDTO Hashpassword(UserDTO userdto) //de methode om het wachtwoord gehashed te hebben.
        {
            byte[] salt = Encoding.UTF8.GetBytes("9X5uY2f#E$@wGcBz"); //gefixte salt (kan ook in de database staan bij elke user maar omdat het maar een user is staat die hier in) 
            int iterations = 1000; // nummer van iteraties voor de key derivation functie
            int hashByteSize = 64; // output groote in bytes (512 bits)

            var pbkdf2 = new Rfc2898DeriveBytes(userdto.PasswordSalt, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(hashByteSize);

            // combineer salt en hash in een aparte string.
            byte[] hashBytes = new byte[salt.Length + hash.Length];
            Array.Copy(salt, 0, hashBytes, 0, salt.Length);
            Array.Copy(hash, 0, hashBytes, salt.Length, hash.Length);
            userdto.PasswordHash = Convert.ToBase64String(hashBytes);

            
            return userdto;
        }

        public bool ValidateUser(UserDTO userdto) //roept de hashpassword aan en vergelijkt het met het wachtwoord in de database.
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                if (userdto != null)
                {
                    Hashpassword(userdto);

                    string selectquery = "SELECT * FROM [User] WHERE Email = @Email And Password = @Password";

                    using (SqlCommand command = new SqlCommand(selectquery, connection))
                    {
                        command.Parameters.AddWithValue("Email", userdto.Email);
                        command.Parameters.AddWithValue("Password", userdto.PasswordHash);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read() == true)
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    return false;
                }
                
                return false;
            }
        }
    }
}