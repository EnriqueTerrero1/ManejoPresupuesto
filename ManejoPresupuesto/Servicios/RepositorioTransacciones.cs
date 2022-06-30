using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioTransacciones
    {
        Task Actualizar(Transaccion transaccion, decimal montoAnterior, int cuentaAnterior);
        Task Borrar(int id);
        Task Crear(Transaccion transaccion);
        Task<IEnumerable<Transaccion>> ObtenerPorCuentaId(ObtenerTransaccionesPorCuenta modelo);
        Task<Transaccion> ObtenerPorId(int id, int usuarioId);
        Task<IEnumerable<ResultadoObtenerPorSemana>> ObtenerPorSemana(ParametroObtenerTransaccionesPorUsuario modelo);
        Task<IEnumerable<Transaccion>> ObtenerPorUsuarioId(ParametroObtenerTransaccionesPorUsuario modelo);
    }
    public class RepositorioTransacciones:IRepositorioTransacciones
    {
        private readonly string connectionString;

        public RepositorioTransacciones(IConfiguration configuration)
        {
             connectionString = configuration.GetConnectionString("DefaultConnection");
        }

      
        public async Task Crear(Transaccion transaccion)
        {
            using var connection = new SqlConnection(connectionString);

            var id = await connection.QuerySingleAsync<int>("Transacciones_Insertar",
                new
                {
                    transaccion.UsuarioId,
                    transaccion.FechaTransaccion,
                    transaccion.Monto,
                    transaccion.CategoriaId,
                    transaccion.CuentaId,
                    transaccion.Nota
                }, commandType: System.Data.CommandType.StoredProcedure);
            transaccion.Id= id;

        }
        public async Task Actualizar(Transaccion transaccion,decimal montoAnterior,int cuentaAnteriorId)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("Transacciones_Actualizar", new
            {
                transaccion.Id,
                transaccion.FechaTransaccion,
                transaccion.Monto,
                transaccion.CategoriaId,
                transaccion.CuentaId,
                transaccion.Nota,
                montoAnterior,
                cuentaAnteriorId
            }, commandType: System.Data.CommandType.StoredProcedure);
        }
        public async Task<Transaccion>ObtenerPorId(int id, int usuarioId)
        {
            var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Transaccion>(@"Select transacciones.*,cat.tipooperacionid
                                                                            from transacciones
                                                                               inner join categorias cat 
                                                                                on cat.id = transacciones.categoriaId
                                                                                    where transacciones.id = @id and transacciones.usuarioid = @usuarioid", new { id, usuarioId });
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("Transacciones_Borrar", new { id }, commandType: System.Data.CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<Transaccion>> ObtenerPorCuentaId(ObtenerTransaccionesPorCuenta modelo)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<Transaccion>(@"Select t.id,t.Monto,t.fechatransaccion,c.nombre as categoria, cu.nombre as cuenta,c.tipooperacionid
                                                              from transacciones t
                                                               inner join categorias c
                                                                on c.id = t.categoriaId
                                                                 inner join cuentas cu
                                                                    on cu.id = t.cuentaid
                                                                    where t.cuentaId = @CuentaId and t.usuarioId = @usuarioId
                                                                    and fechatransaccion between @fechaInicio and @FechaFin",modelo);
        }

        public async Task<IEnumerable<Transaccion>> ObtenerPorUsuarioId(ParametroObtenerTransaccionesPorUsuario modelo)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<Transaccion>(@"Select t.id,t.Monto,t.fechatransaccion,c.nombre as categoria, cu.nombre as cuenta,c.tipooperacionid
                                                              from transacciones t
                                                               inner join categorias c
                                                                on c.id = t.categoriaId
                                                                 inner join cuentas cu
                                                                    on cu.id = t.cuentaid
                                                                    where  t.usuarioId = @usuarioId
                                                                    and fechatransaccion between @fechaInicio and @FechaFin
                                                                     Order By t.FechaTransaccion DESC", modelo);
        }
        public async Task<IEnumerable<ResultadoObtenerPorSemana>> ObtenerPorSemana(ParametroObtenerTransaccionesPorUsuario modelo)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<ResultadoObtenerPorSemana>(@"
                    Select datediff(d, @fechaInicio, FechaTransaccion) / 7 + 1 as Semana,
                    SUM(Monto) as Monto, cat.TipoOperacionId
                    FROM Transacciones
                    INNER JOIN Categorias cat
                    ON cat.Id = Transacciones.CategoriaId
                    WHERE Transacciones.UsuarioId = @usuarioId AND
                    FechaTransaccion BETWEEN @fechaInicio and @fechaFin
                    GROUP BY datediff(d, @fechaInicio, FechaTransaccion) / 7, cat.TipoOperacionId
                    ", modelo);

        }
    }
}
