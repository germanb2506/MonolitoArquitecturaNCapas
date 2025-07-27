using System.Linq.Expressions;

namespace App.Interfaces.Repos.Generic
{
    /// <summary>
    /// Interfaz genérica para repositorios, que define operaciones CRUD comunes y manejo de transacciones.
    /// </summary>
    /// <typeparam name="T">El tipo de entidad que maneja el repositorio.</typeparam>
    public interface IGenericRepo<T> where T : class
    {
        /// <summary>
        /// Crea una nueva entidad en la base de datos.
        /// </summary>
        Task Crear(T entidad);

        /// <summary>
        /// Obtiene una única entidad que cumple con un filtro.
        /// </summary>
        Task<T> Obtener(Expression<Func<T, bool>> filtro = null, bool tracked = true);

        /// <summary>
        /// Obtiene todas las entidades que cumplen con un filtro opcional.
        /// </summary>
        Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null);

        /// <summary>
        /// Elimina una entidad de la base de datos.
        /// </summary>
        Task Remover(T entidad);

        /// <summary>
        /// Guarda los cambios pendientes en el contexto de la base de datos.
        /// </summary>
        Task Grabar();

        /// <summary>
        /// Inicia una transacción explícita.
        /// </summary>
        Task IniciarTransaccion();

        /// <summary>
        /// Confirma los cambios dentro de la transacción actual.
        /// </summary>
        Task ConfirmarTransaccion();

        /// <summary>
        /// Revierte todos los cambios realizados dentro de la transacción actual.
        /// </summary>
        Task RevertirTransaccion();

        /// <summary>
        /// Obtiene una proyección específica de una entidad que cumple con un filtro.
        /// </summary>
        Task<TResult> GetProy<TResult>(Expression<Func<T, bool>> filtro, Expression<Func<T, TResult>> selector);

        /// <summary>
        /// Obtiene una lista proyectada de entidades que cumplen con un filtro.
        /// </summary>
        Task<List<TResult>> GetAllProy<TResult>(Expression<Func<T, bool>>? filtro, Expression<Func<T, TResult>> selector);

        /// <summary>
        /// Recupera una colección consultable (queryable) de entidades desde la base de datos.
        /// </summary>
        /// <param name="tracked">
        /// Valor booleano que indica si las entidades deben ser rastreadas por el contexto.
        /// Especifica <see langword="true"/> para habilitar el tracking o <see langword="false"/> para deshabilitarlo.
        /// </param>
        /// <returns>
        /// Un <see cref="IQueryable{T}"/> que representa la colección de entidades.
        /// Si <paramref name="tracked"/> es <see langword="false"/>, la consulta no rastreará los cambios en las entidades.
        /// </returns>
        IQueryable<T> Query(bool tracked = false);

        /// <summary>
        /// Transmite entidades desde la base de datos como una secuencia asincrónica enumerable.
        /// </summary>
        /// <param name="filtro">
        /// Expresión de filtro opcional que se aplica a la consulta. Si es <see langword="null"/>, no se aplica ningún filtro.
        /// </param>
        /// <returns>
        /// Un <see cref="IAsyncEnumerable{T}"/> que enumera asincrónicamente las entidades que cumplen con el filtro especificado.
        /// </returns>
        IAsyncEnumerable<T> Stream(Expression<Func<T, bool>>? filtro = null);
    }
}
