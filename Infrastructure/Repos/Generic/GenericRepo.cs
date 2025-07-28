using App.Interfaces.Repos.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Infrastructure.Repos.Generic
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {  /// <summary>
       /// Contexto de la base de datos.
       /// </summary>
        private readonly DbContexto _context;
        private readonly DbSet<T> dbSet;
        private IDbContextTransaction? _currentTransaction;
        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="context"></param>
        public GenericRepo(DbContexto context)
        {
            _context = context;
            dbSet = context.Set<T>();
        }

        /// <summary>
        /// Inicia una transacción explícita.
        /// </summary>
        public async Task IniciarTransaccion()
        {
            if (_currentTransaction != null) return; // No se permite anidar transacciones.
            _currentTransaction = await _context.Database.BeginTransactionAsync();
        }
        /// <summary>
        /// Guarda los cambios pendientes en el contexto de la base de datos.
        /// </summary>
        /// <returns></returns>
        public async Task Grabar()
        {
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Confirma los cambios dentro de la transacción actual.
        /// </summary>
        public async Task ConfirmarTransaccion()
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("No hay una transacción activa para confirmar.");

            await Grabar(); // Guarda los cambios en la base de datos.
            await _currentTransaction.CommitAsync();
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }

        /// <summary>
        /// Revierte todos los cambios realizados dentro de la transacción actual.
        /// </summary>
        public async Task RevertirTransaccion()
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("No hay una transacción activa para revertir.");

            await _currentTransaction.RollbackAsync();
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }

        /// <summary>
        /// Crea una nueva entidad en la base de datos.
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public async Task Crear(T entidad)
        {
            await dbSet.AddAsync(entidad);
            await Grabar();
        }

        /// <summary>
        /// Obtiene una única entidad que cumple con un filtro.
        /// </summary>
        /// <param name="filtro"></param>
        /// <param name="tracked"></param>
        /// <returns></returns>
        public async Task<T> Obtener(Expression<Func<T, bool>>? filtro = null, bool tracked = true)
        {
            IQueryable<T> query = tracked ? dbSet : dbSet.AsNoTracking();
            if (filtro != null)
            {
                query = query.Where(filtro);
            }
            return await query.FirstOrDefaultAsync();
        }
        /// <summary>
        /// Obtiene todas las entidades que cumplen con un filtro opcional.
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public async Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);
            }
            return await query.ToListAsync();
        }
        /// <summary>
        /// Elimina una entidad de la base de datos.
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public async Task Remover(T entidad)
        {
            dbSet.Remove(entidad);
            await Grabar();
        }
        /// <summary>
        /// Obtiene una proyección específica de una entidad que cumple con un filtro.
        /// </summary>
        public async Task<TResult> GetProy<TResult>(Expression<Func<T, bool>> filtro, Expression<Func<T, TResult>> selector)
        {
            IQueryable<T> query = dbSet.AsNoTracking(); // No rastrear para reducir la sobrecarga.
            if (filtro != null)
            {
                query = query.Where(filtro);
            }
            return await query.Select(selector).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Obtiene una lista proyectada de entidades que cumplen con un filtro.
        /// </summary>
        public async Task<List<TResult>> GetAllProy<TResult>(Expression<Func<T, bool>>? filtro, Expression<Func<T, TResult>> selector)
        {
            IQueryable<T> query = dbSet.AsNoTracking(); // No rastrear para reducir la sobrecarga.
            if (filtro != null)
            {
                query = query.Where(filtro);
            }
            return await query.Select(selector).ToListAsync();
        }
        /// <summary>
        /// Recupera una colección consultable (queryable) de entidades desde la base de datos.
        /// </summary>
        /// <remarks>
        /// Usa seguimiento (tracking) cuando necesites modificar entidades y persistir los cambios en la base de datos.
        /// Para operaciones de solo lectura, deshabilitar el tracking puede mejorar el rendimiento.
        /// </remarks>
        /// <param name="tracked">
        /// Valor booleano que indica si las entidades deben ser rastreadas por el contexto.
        /// Especifica <see langword="true"/> para habilitar el tracking o <see langword="false"/> para deshabilitarlo.
        /// </param>
        /// <returns>
        /// Un <see cref="IQueryable{T}"/> que representa la colección de entidades.
        /// Si <paramref name="tracked"/> es <see langword="false"/>, la consulta no rastreará los cambios en las entidades.
        /// </returns>

        public IQueryable<T> Query(bool tracked = false)
        {
            return tracked ? dbSet : dbSet.AsNoTracking();
        }
        /// <summary>
        /// Transmite entidades desde la base de datos como una secuencia asincrónica enumerable.
        /// </summary>
        /// <remarks>
        /// La secuencia retornada se evalúa de forma diferida (lazy) y no realiza seguimiento (tracking) de los cambios en las entidades.
        /// Utiliza este método para procesar conjuntos de datos grandes de manera eficiente o para transmitir resultados en tiempo real.
        /// </remarks>
        /// <param name="filtro">
        /// Expresión de filtro opcional que se aplica a la consulta. Si es <see langword="null"/>, no se aplica ningún filtro.
        /// </param>
        /// <returns>
        /// Un <see cref="IAsyncEnumerable{T}"/> que enumera asincrónicamente las entidades que cumplen con el filtro especificado.
        /// </returns>

        public IAsyncEnumerable<T> Stream(Expression<Func<T, bool>>? filtro = null)
        {
            IQueryable<T> query = dbSet.AsNoTracking();
            if (filtro != null)
            {
                query = query.Where(filtro);
            }
            return query.AsAsyncEnumerable();
        }

    }
}
