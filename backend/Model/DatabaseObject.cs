using SiLabI.Exceptions;
using SiLabI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// A database object.
    /// All database objects contains four common fields: id, created_at, updated_at and state.
    /// </summary>
    [DataContract]
    public abstract class DatabaseObject : BaseObject
    {
        protected int? _id;
        protected DateTimeOffset? _createdAt;
        protected DateTimeOffset? _updatedAt;
        protected string _state;

        /// <summary>
        /// The identification number.
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
        public virtual DateTimeOffset? CreatedAt
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
        public virtual DateTimeOffset? UpdatedAt
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
        /// This private field is used to send the date in a pretty format.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "created_at")]
        private string CreatedAt_ISO8601 { get; set; }

        /// <summary>
        /// The last update date in ISO-8601 format.
        /// This private field is used to send the date in a pretty format.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "updated_at")]
        private string UpdatedAt_ISO8601 { get; set; }

        /// <summary>
        /// Check if the object properties are valid for a create operation.
        /// </summary>
        /// <returns>True if the object properties are valid for a create operation.</returns>
        public virtual bool IsValidForCreate()
        {
            return true;
        }

        /// <summary>
        /// Check if the object properties are valid for an update operation.
        /// </summary>
        /// <returns>True if the object properties are valid for an update operation.</returns>
        public virtual bool IsValidForUpdate()
        {
            return true;
        }

        public static void Parse(DatabaseObject obj, DataRow row, string prefix = "")
        {
            obj.Id = row.Table.Columns.Contains(prefix + "id") ? Converter.ToNullableInt32(row[prefix + "id"]) : null;
            obj.CreatedAt = row.Table.Columns.Contains(prefix + "created_at") ? Converter.ToNullableDateTimeOffset(row[prefix + "created_at"]) : null;
            obj.UpdatedAt = row.Table.Columns.Contains(prefix + "updated_at") ? Converter.ToNullableDateTimeOffset(row[prefix + "updated_at"]) : null;
            obj.State = row.Table.Columns.Contains(prefix + "state") ? Converter.ToString(row[prefix + "state"]) : null;
        }
    }
}