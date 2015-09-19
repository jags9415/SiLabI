using SiLabI.Model.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SiLabI.Data
{
    /// <summary>
    /// An interface to perform CRUD operations on a database table.
    /// </summary>
    public interface IDataAccess
    {
        /// <summary>
        /// Get the amount of rows that satifies q query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>The amount of rows that satifies the query.</returns>
        int GetCount(QueryString request);

        /// <summary>
        /// Get all the rows that satisfies q query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>A DataTable that contains all the rows that satisfies the query.</returns>
        DataTable GetAll(QueryString request);

        /// <summary>
        /// Get a specific row.
        /// </summary>
        /// <param name="id">The row identity.</param>
        /// <param name="request">The query.</param>
        /// <returns>The row.</returns>
        DataRow GetOne(int id, QueryString request);

        /// <summary>
        /// Insert a row.
        /// </summary>
        /// <param name="obj">The row data.</param>
        /// <returns>The inserted row.</returns>
        DataRow Create(object obj);

        /// <summary>
        /// Updates a row.
        /// </summary>
        /// <param name="id">The row identity.</param>
        /// <param name="obj">The row data.</param>
        /// <returns>The updated row.</returns>
        DataRow Update(int id, object obj);

        /// <summary>
        /// Delete a row.
        /// </summary>
        /// <param name="id">The row identity.</param>
        void Delete(int id);
    }
}