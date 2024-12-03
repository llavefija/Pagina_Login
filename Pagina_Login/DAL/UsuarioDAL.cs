using Pagina_Login.Models;
using Microsoft.Data.SqlClient;
using System;
using Pagina_Login.Tools;

namespace Pagina_Login.DAL
{
    public class UsuarioDAL
    {
        // Cadena de conexión
        private string _connectionString = "Server=85.208.21.117,54321;Database=AbelLogin;User Id=sa;Password=Sql#123456789;TrustServerCertificate=True;";

        // Método para obtener el usuario por login
        public UsuarioModel GetUsuarioLogin(string userName, string passwd)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(@"SELECT UserName, PasswordHash, PasswordSalt,
                                        Apellido, Email, FechaNacimiento, Telefono, Direccion,
                                        Ciudad, Estado, CodigoPostal, FechaRegistro, Activo
                                        FROM Usuario
                                        WHERE UserName = @UserName", connection);
                command.Parameters.AddWithValue("@UserName", userName);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var passwordHash = (byte[])reader["passwordHash"];
                        var passwordSalt = (byte[])reader["passwordSalt"];

                        // Verificar contraseña
                        if (PasswordHelper.VerifityPasswordHash(passwd, passwordHash, passwordSalt))
                        {
                            return new UsuarioModel { };
                        }
                    }
                }
                return null;
            }
        }


        // Método para crear un nuevo usuario
        public void CreateUsuario(UsuarioModel usuario, string password)
        {
           
            PasswordHelper.CreatePasswordHash(password, out byte[] passwordHash, out  byte[] passwordSalt);

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(@"INSERT INTO Usuario
                                        (UserName, PasswordHash, PasswordSalt)
                                        VALUES (@UserName, @PasswordHash, @PasswordSalt)",
                                                connection);

                command.Parameters.AddWithValue("@UserName", usuario.UserName);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                command.Parameters.AddWithValue("@PasswordSalt", passwordSalt);

                connection.Open ();
                command.ExecuteNonQuery();
            }
        }
    }
}
