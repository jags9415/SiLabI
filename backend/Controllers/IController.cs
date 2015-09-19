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
        /// <returns>A GetResponse containing the object list and the pagination information.</returns>
        GetResponse<T> GetAll(QueryString request);

        /// <summary>
        /// Get a specific object.
        /// </summary>
        /// <param name="id">The object identity.</param>
        /// <param name="request">The query.</param>
        /// <returns>The object.</returns>
        T GetOne(int id, QueryString request);

        /// <summary>
        /// Create an object.
        /// </summary>
        /// <param name="request">The create request.</param>
        /// <returns>The created object.</returns>
        T Create(BaseRequest request);

        /// <summary>
        /// Update an object.
        /// </summary>
        /// <param name="id">The object identity.</param>
        /// <param name="request">The update request.</param>
        /// <returns>The updated object.</returns>
        T Update(int id, BaseRequest request);

        /// <summary>
        /// Delete an object.
        /// </summary>
        /// <param name="id">The object identity.</param>
        /// <param name="request">The delete request.</param>
        void Delete(int id, BaseRequest request);
    }
}