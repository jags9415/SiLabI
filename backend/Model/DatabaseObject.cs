using SiLabI.Exceptions;
using SiLabI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// A database object.
    /// </summary>
    [DataContract]
    public abstract class DatabaseObject : BaseObject
    {
        protected int? _id;
        protected DateTime? _createdAt;
        protected DateTime? _updatedAt;
        protected string _state;

        /// <summary>
        /// The user identification number.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "id")]
        public virtual int? Id
        {
            set
            {
                if (!Validator.IsValidId(value))
                {
                    throw new InvalidParameterException("id", "Ingrese un número > 0");
                }
                _id = value;
            }
            get { return _id; }
        }

        /// <summary>
        /// The state.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "state")]
        public virtual string State
        {
            set { _state = value; }
            get { return _state; }
        }

        /// <summary>
        /// The creation date.
        /// </summary>
        public virtual DateTime? CreatedAt
        {
            set
            {
                _createdAt = value;
                CreatedAt_ISO8601 = value.HasValue ? value.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") : null;
            }
            get { return _createdAt; }
        }

        /// <summary>
        /// The last update date.
        /// </summary>
        public virtual DateTime? UpdatedAt
        {
            set
            {
                _updatedAt = value;
                UpdatedAt_ISO8601 = value.HasValue ? value.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") : null;
            }
            get { return _updatedAt; }
        }

        /// <summary>
        /// The creation date in ISO-8601 format.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "created_at")]
        private string CreatedAt_ISO8601 { get; set; }

        /// <summary>
        /// The last update date in ISO-8601 format.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "updated_at")]
        private string UpdatedAt_ISO8601 { get; set; }

        /// <summary>
        /// Check if the object properties are valid for a create operation.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValidForCreate()
        {
            return true;
        }

        /// <summary>
        /// Check if the object properties are valid for an update operation.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValidForUpdate()
        {
            return true;
        }
    }
}