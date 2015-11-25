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
        /// Get the amount of rows that satifies a query.
        /// </summary>
        /// <param name="payload">The token payload of the user who is making the request.</param>
        /// <param name="request">The query.</param>
        /// <returns>The amount of rows that satifies the query.</returns>
        int GetCount(Dictionary<string, object> payload, QueryString request);

        /// <summary>
        /// Get all the rows that satisfies a query.
        /// </summary>
        /// <param name="payload">The token payload of the user who is making the request.</param>
        /// <param name="request">The query.</param>
        /// <returns>A DataTable that contains all the rows that satisfies the query.</returns>
        DataTable GetAll(Dictionary<string, object> payload, QueryString request);

        /// <summary>
        /// Get a specific row.
        /// </summary>
        /// <param name="payload">The token payload of the user who is making the request.</param>
        /// <param name="id">The row identity.</param>
        /// <param name="request">The query.</param>
        /// <returns>The row.</returns>
        DataRow GetOne(Dictionary<string, object> payload, int id, QueryString request);

        /// <summary>
        /// Insert a row.
        /// </summary>
        /// <param name="payload">The token payload of the user who is making the request.</param>
        /// <param name="obj">The row data.</param>
        /// <returns>The inserted row.</returns>
        DataRow Create(Dictionary<string, object> payload, object obj);

        /// <summary>
        /// Updates a row.
        /// </summary>
        /// <param name="payload">The token payload of the user who is making the request.</param>
        /// <param name="id">The row identity.</param>
        /// <param name="obj">The row data.</param>
        /// <returns>The updated row.</returns>
        DataRow Update(Dictionary<string, object> payload, int id, object obj);

        /// <summary>
        /// Delete a row.
        /// </summary>
        /// <param name="payload">The token payload of the user who is making the request.</param>
        /// <param name="id">The row identity.</param>
        void Delete(Dictionary<string, object> payload, int id);
    }
}