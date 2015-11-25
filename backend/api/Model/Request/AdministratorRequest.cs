using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiLabI.Model.Request
{
    /// <summary>
    /// A POST request body to the endpoints /administrators
    /// </summary>
    public class AdministratorRequest : BaseRequest
    {
        private int _id;

        /// <summary>
        /// The user identity.
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
    }
}