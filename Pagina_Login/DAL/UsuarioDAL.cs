using Pagina_Login.Models;
using Microsoft.Data.SqlClient;
using System;

namespace Pagina_Login.DAL
{
    public class UsuarioDAL
    {
        // Cadena de conexión
        private string _connectionString = "Server=85.208.21.117,54321;Database=AbelLogin;User Id=sa;Password=Sql#123456789;TrustServerCertificate=True;";

        // Método para obtener el usuario por login
        public UsuarioModel GetUsuarioLogin(string userName, string passwd)
        {
            // Crear la conexión con la base de datos
            using (var connection = new SqlConnection(_connectionString))
            {
                // Crear el comando SQL
                var command = new SqlCommand(
                    @"SELECT * FROM Usuario
              WHERE UserName = @UserName AND Passwd = @Passwd",
                    connection
                );

                // Añadir los parámetros al comando
                command.Parameters.AddWithValue("@UserName", userName);
                command.Parameters.AddWithValue("@Passwd", passwd);

                // Abrir la conexión
                connection.Open();

                // Ejecutar el comando y leer los resultados
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Mapear los datos al modelo UsuarioModel
                        return new UsuarioModel
                        {
                            IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                            UserName = reader["UserName"] as string ?? string.Empty,
                            Passwd = reader["Passwd"] as string ?? string.Empty,
                            Apellido = reader["Apellido"] as string ?? string.Empty,
                            Email = reader["Email"] as string ?? string.Empty,
                            FechaNacimiento = reader["FechaNacimiento"] as DateTime?,
                            Telefono = reader["Telefono"] as string ?? string.Empty,
                            Direccion = reader["Direccion"] as string ?? string.Empty,
                            Ciudad = reader["Ciudad"] as string ?? string.Empty,
                            Estado = reader["Estado"] as string ?? string.Empty,
                            CodigoPostal = reader["CodigoPostal"] as string ?? string.Empty,
                            FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"]),
                            Activo = Convert.ToBoolean(reader["Activo"])
                        };
                    }

                    // Retornar null si no se encuentra el usuario
                    return null;
                }
            }
        }


        // Método para crear un nuevo usuario
        public void CreateUsuario(UsuarioModel usuario)
        {
            // Crear la conexión con la base de datos
            using (var connection = new SqlConnection(_connectionString))
            {
                // Crear el comando SQL
                var command = new SqlCommand(
                    @"INSERT INTO Usuario 
                      (UserName, Passwd, Apellido, Email, FechaNacimiento, Telefono, Direccion, Ciudad, Estado, CodigoPostal, FechaRegistro, Activo)
                      VALUES
                      (@UserName, @Passwd, @Apellido, @Email, @FechaNacimiento, @Telefono, @Direccion, @Ciudad, @Estado, @CodigoPostal, @FechaRegistro, @Activo)",
                    connection
                );

                // Añadir los parámetros al comando
                command.Parameters.AddWithValue("@UserName", usuario.UserName);
                command.Parameters.AddWithValue("@Passwd", usuario.Passwd);
                command.Parameters.AddWithValue("@Apellido", usuario.Apellido ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Email", usuario.Email ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FechaNacimiento", usuario.FechaNacimiento ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Telefono", usuario.Telefono ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Direccion", usuario.Direccion ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Ciudad", usuario.Ciudad ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Estado", usuario.Estado ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CodigoPostal", usuario.CodigoPostal ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FechaRegistro", DateTime.Now); // Fecha actual para FechaRegistro
                command.Parameters.AddWithValue("@Activo", true); // Asumimos que el usuario estará activo por defecto

                // Abrir la conexión y ejecutar el comando
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
