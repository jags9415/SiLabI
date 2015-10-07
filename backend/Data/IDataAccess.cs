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
        /// <param name="requesterId">The identification number of the user who is making the request.</param>
        /// <param name="request">The query.</param>
        /// <returns>The amount of rows that satifies the query.</returns>
        int GetCount(object requesterId, QueryString request);

        /// <summary>
        /// Get all the rows that satisfies q query.
        /// </summary>
        /// <param name="requesterId">The identification number of the user who is making the request.</param>
        /// <param name="request">The query.</param>
        /// <returns>A DataTable that contains all the rows that satisfies the query.</returns>
        DataTable GetAll(object requesterId, QueryString request);

        /// <summary>
        /// Get a specific row.
        /// </summary>
        /// <param name="requesterId">The identification number of the user who is making the request.</param>
        /// <param name="id">The row identity.</param>
        /// <param name="request">The query.</param>
        /// <returns>The row.</returns>
        DataRow GetOne(object requesterId, int id, QueryString request);

        /// <summary>
        /// Insert a row.
        /// </summary>
        /// <param name="requesterId">The identification number of the user who is making the request.</param>
        /// <param name="obj">The row data.</param>
        /// <returns>The inserted row.</returns>
        DataRow Create(object requesterId, object obj);

        /// <summary>
        /// Updates a row.
        /// </summary>
        /// <param name="requesterId">The identification number of the user who is making the request.</param>
        /// <param name="id">The row identity.</param>
        /// <param name="obj">The row data.</param>
        /// <returns>The updated row.</returns>
        DataRow Update(object requesterId, int id, object obj);

        /// <summary>
        /// Delete a row.
        /// </summary>
        /// <param name="requesterId">The identification number of the user who is making the request.</param>
        /// <param name="id">The row identity.</param>
        void Delete(object requesterId, int id);
    }
}