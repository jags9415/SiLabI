using SiLabI.Model.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SiLabI.Data
{
    /// <summary>
    /// An interface to perform CRUD operations on a intermediate database table.
    /// </summary>
    public interface IDataAccessIntermediate
    {
        /// <summary>
        /// Get all the rows that satisfies the query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>A DataTable that contains all the rows that satisfies the query.</returns>
        DataTable GetAll(object requesterId, int id, QueryString request);

        /// <summary>
        /// Insert one or more relationships in the intermediate table.
        /// </summary>
        /// <param name="id">The primary foreign key.</param>
        /// <param name="obj">The data to insert.</param>
        /// <returns>The inserted row.</returns>
        void Create(object requesterId, int id, object obj);

        /// <summary>
        /// Update the relationships in the intermediate table.
        /// </summary>
        /// <param name="id">The primary foreign key.</param>
        /// <param name="obj">The new data.</param>
        /// <returns>The updated row.</returns>
        void Update(object requesterId, int id, object obj);

        /// <summary>
        /// Delete one or more relationships in the intermediate table.
        /// </summary>
        /// <param name="id">The primary foreign key.</param>
        /// <param name="obj">The data to delete.</param>
        void Delete(object requesterId, int id, object obj);
    }
}