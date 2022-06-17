using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{


    public interface IRepositorioCuentas
    {
        Task Actualizar(CuentaCreacionViewModel cuenta);
        Task<IEnumerable<Cuenta>> Buscar(int usuarioId);
        Task Crear(Cuenta cuenta);
        Task<Cuenta> ObtenerPorId(int id, int usuarioId);
    }
    public class RepositorioCuentas : IRepositorioCuentas
    {
        private readonly string connectionString;
        public RepositorioCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Cuenta cuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERt INTO CUENTAS (NOMBRE,TIPOCUENTAID,DESCRIPCION,BALANCE) values (@Nombre,@TipoCuentaId,@Descripcion,@Balance);
                                                           Select SCOPE_IDENTITY();", cuenta);
            cuenta.Id = id;
        }

        public async Task <IEnumerable<Cuenta>> Buscar(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Cuenta>(@"select cuentas.id,cuentas.nombre,balance,TiposCuentas.Nombre as TipoCuenta
                                                        from cuentas
                                                        Inner JOIN TiposCuentas
                                                        on TiposCuentas.id = Cuentas.TipoCuentaId
                                                        where TiposCuentas.UsuarioId = @UsuarioId
                                                        ORDER  BY TiposCuentas.Orden", new { usuarioId });
        }

        public async Task<Cuenta> ObtenerPorId(int id,int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Cuenta>(@"select cuentas.id,cuentas.nombre,balance,descripcion,TipoCuentaId
                                                        from cuentas
                                                        Inner JOIN TiposCuentas
                                                        on TiposCuentas.id = Cuentas.TipoCuentaId
                                                        where TiposCuentas.usuarioId =@UsuarioId AND cuentas.id = @Id", new {id, usuarioId});
                                                        
        }

        public async Task Actualizar(CuentaCreacionViewModel cuenta)
        {
            using var connection = new SqlConnection(connectionString); 
            
            await connection.ExecuteAsync(@"Update cuentas
                                             SET NOMBRE =@Nombre,Balance=@Balance,Descripcion=@Descripcion,TipoCuentaId=@TipoCuentaId where Id=@id;",cuenta);
        }


    }
   
}
