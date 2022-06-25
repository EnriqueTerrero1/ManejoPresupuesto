using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{

    public interface IRepositorioCategorias
    {
        Task Actualizar(Categoria categoria);
        Task Borrar(int id);
        Task Crear(Categoria categoria);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId, TipoOperacion tipoOperacionId);
        Task<Categoria> ObtenerPorId(int id, int usuarioId);
    }
    public class RepositorioCategorias : IRepositorioCategorias
    {

        private readonly string connectionString;

        public RepositorioCategorias(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Categoria categoria)
        {

            using var connection = new SqlConnection(connectionString);
            await connection.QuerySingleAsync<int>(@"insert into Categorias(Nombre, TipoOperacionId, usuarioId)
                                        values(@Nombre, @TipoOperacionid, @UsuarioId)  SELECT SCOPE_Identity();", categoria);


        }

        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId)
        {

            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Categoria>(
                "Select * from categorias where usuarioId =@usuarioId", new
                {
                    usuarioId
                }
                );
        }
        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId,TipoOperacion tipoOperacionId)
        {

            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Categoria>(
                @"Select * from categorias where usuarioId =@usuarioId AND TipoOperacionId =@tipoOperacionId", new
                {
                    usuarioId,tipoOperacionId
                }
                );
        }

        public async Task<Categoria> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Categoria>(@"Select * from categorias where Id =@id and usuarioId =@usuarioId", new { id, usuarioId });
        }

        public async Task Actualizar(Categoria categoria)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync(@"UPDATE categorias 
                                            set nombre = @nombre,TipoOperacionId=@TipoOperacionID WHere id =@id", categoria);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE CATEGORIAS WHERE ID =@Id", new { id });
        }
    }
        
}

