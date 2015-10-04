using SiLabI.Model.Query;
using SiLabI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiLabI.Controllers
{
    /// <summary>
    /// Perform CRUD operations for a type of object.
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    public interface IController<T>
    {
        /// <summary>
        /// Get all objects that satisfies a query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <param name="payload">The token payload.</param>
        /// <returns>A GetResponse containing the object list and the pagination information.</returns>
        GetResponse<T> GetAll(QueryString request, Dictionary<string, object> payload);

        /// <summary>
        /// Get a specific object.
        /// </summary>
        /// <param name="id">The object identity.</param>
        /// <param name="request">The query.</param>
        /// <param name="payload">The token payload.</param>
        /// <returns>The object.</returns>
        T GetOne(int id, QueryString request, Dictionary<string, object> payload);

        /// <summary>
        /// Create an object.
        /// </summary>
        /// <param name="request">The create request.</param>
        /// <param name="payload">The token payload.</param>
        /// <returns>The created object.</returns>
        T Create(BaseRequest request, Dictionary<string, object> payload);

        /// <summary>
        /// Update an object.
        /// </summary>
        /// <param name="id">The object identity.</param>
        /// <param name="request">The update request.</param>
        /// <param name="payload">The token payload.</param>
        /// <returns>The updated object.</returns>
        T Update(int id, BaseRequest request, Dictionary<string, object> payload);

        /// <summary>
        /// Delete an object.
        /// </summary>
        /// <param name="id">The object identity.</param>
        /// <param name="request">The delete request.</param>
        /// <param name="payload">The token payload.</param>
        void Delete(int id, BaseRequest request, Dictionary<string, object> payload);
    }
}