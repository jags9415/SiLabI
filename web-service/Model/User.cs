using SiLabI.Model.Query;
using SiLabI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel.Web;
using System.Text.RegularExpressions;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// An user data.
    /// </summary>
    [DataContract]
    public class User
    {
        protected int _id;
        protected string _name;
        protected string _lastname1;
        protected string _lastname2;
        protected string _username;
        protected string _password;
        protected string _gender;
        protected string _email;
        protected string _phone;
        protected string _state;
        protected string _type;
        protected DateTime? _createdAt;
        protected DateTime? _updatedAt;

        /// <summary>
        /// Creates a new empty user.
        /// </summary>
        public User() { }

        /// <summary>
        /// Clones an user.
        /// </summary>
        /// <param name="user">The user to clone.</param>
        public User(User user)
        {
            this.Id = user.Id;
            this.Name = user.Name;
            this.LastName1 = user.LastName1;
            this.LastName2 = user.LastName2;
            this.Gender = user.Gender;
            this.Username = user.Username;
            this.Password = user.Password;
            this.Email = user.Email;
            this.Phone = user.Phone;
            this.State = user.State;
            this.Type = user.Type;
            this.CreatedAt = user.CreatedAt;
            this.UpdatedAt = user.UpdatedAt;
        }

        /// <summary>
        /// The user identification number.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "id")]
        public virtual int Id
        {
            set
            {
                if (value < 0)
                {
                    ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "InvalidParameter", "Id inválido. Ingrese un número > 0.");
                    throw new WebFaultException<ErrorResponse>(error, error.Code);
                }
                _id = value;
            }
            get { return _id; }
        }

        /// <summary>
        /// The first name.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "name")]
        public virtual string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        /// <summary>
        /// The first lastname.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "last_name_1")]
        public virtual string LastName1
        {
            set { _lastname1 = value; }
            get { return _lastname1; }
        }

        /// <summary>
        /// The second lastname.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "last_name_2")]
        public virtual string LastName2
        {
            set { _lastname2 = value; }
            get { return _lastname2; }
        }

        /// <summary>
        /// The username.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "username")]
        public virtual string Username
        {
            set
            {
                if (value != null && !Regex.IsMatch(value, "^[a-zA-Z0-9]+$"))
                {
                    ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "InvalidParameter", "Username inválido. Ingrese únicamente caractéres alfanuméricos.");
                    throw new WebFaultException<ErrorResponse>(error, error.Code);
                }
                _username = value;
            }
            get { return _username;  }
        }

        /// <summary>
        /// The password.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "password")]
        public virtual string Password
        {
            set { _password = value; }
            get { return _password; }
        }

        /// <summary>
        /// The email address.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "email")]
        public virtual string Email
        {
            set
            {
                if (value != null)
                {
                    try
                    {
                        MailAddress m = new MailAddress(value);
                        _email = value;
                    }
                    catch (FormatException)
                    {
                        ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "InvalidParameter", "Email inválido.");
                        throw new WebFaultException<ErrorResponse>(error, error.Code);
                    }
                }
                else
                {
                    _email = null;
                }
            }
            get { return _email; }
        }

        /// <summary>
        /// The phone number.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "phone")]
        public virtual string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }

        /// <summary>
        /// The gender.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "gender")]
        public virtual string Gender
        {
            set
            {
                var valid = new[] { "Masculino", "Femenino" };
                if (value != null && !valid.Any(item => item.Equals(value)))
                {
                    ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "InvalidParameter", "Género inválido. Ingrese 'Masculino' o 'Femenino'.");
                    throw new WebFaultException<ErrorResponse>(error, error.Code);
                }
                _gender = value; 
            }
            get { return _gender; }
        }

        /// <summary>
        /// The creation date.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "created_at")]
        public virtual DateTime? CreatedAt
        {
            set { _createdAt = value; }
            get { return _createdAt; }
        }

        /// <summary>
        /// The last update date.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "updated_at")]
        public virtual DateTime? UpdatedAt
        {
            set { _updatedAt = value; }
            get { return _updatedAt; }
        }

        /// <summary>
        /// The state.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "state")]
        public virtual string State
        {
            set
            {
                var valid = new[] { "active", "disabled", "blocked" };
                if (value != null && !valid.Any(item => item.Equals(value)))
                {
                    ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "InvalidParameter", "Estado inválido. Ingrese 'active', 'disabled' o 'blocked'.");
                    throw new WebFaultException<ErrorResponse>(error, error.Code);
                }
                _state = value;
            }
            get { return _state; }
        }

        /// <summary>
        /// The user type.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "type")]
        public virtual string Type
        {
            set {
                var valid = new[] { "student", "professor", "operator", "administrator" };
                if (value != null && !valid.Any(item => item.Equals(value)))
                {
                    ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "InvalidParameter", "Tipo de usuario inválido. Ingrese 'student', 'professor', 'operator' o 'administrator'.");
                    throw new WebFaultException<ErrorResponse>(error, error.Code);
                }
                _type = value;
            }
            get { return _type; }
        }

        /// <summary>
        /// Check if the object properties are valid for a create operation.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValidForCreate()
        {
            bool valid = true;

            valid &= !string.IsNullOrWhiteSpace(Name);
            valid &= !string.IsNullOrWhiteSpace(LastName1);
            valid &= !string.IsNullOrWhiteSpace(Gender);
            valid &= !string.IsNullOrWhiteSpace(Username);
            valid &= !string.IsNullOrWhiteSpace(Password);
            if (LastName2 != null) valid &= !string.IsNullOrWhiteSpace(LastName2);
            if (Phone != null) valid &= !string.IsNullOrWhiteSpace(Phone);

            return valid;
        }

        /// <summary>
        /// Check if the object properties are valid for an update operation.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValidForUpdate()
        {
            bool valid = true;

            if (Name != null) valid &= !string.IsNullOrWhiteSpace(Name);
            if (LastName1 != null) valid &= !string.IsNullOrWhiteSpace(LastName1);
            if (LastName2 != null) valid &= !string.IsNullOrWhiteSpace(LastName2);
            if (Password != null) valid &= !string.IsNullOrWhiteSpace(Password);
            if (Phone != null) valid &= !string.IsNullOrWhiteSpace(Phone);

            return valid;
        }

        /// <summary>
        /// Fill an User object with the data provided in a DataRow.
        /// </summary>
        /// <param name="user">The user.</param>
        public static User Parse(DataRow row)
        {
            User user = new User();

            user.Id = row.Table.Columns.Contains("id") ? Converter.ToInt32(row["id"]) : 0;
            user.Name = row.Table.Columns.Contains("name") ? Converter.ToString(row["name"]) : null;
            user.LastName1 = row.Table.Columns.Contains("last_name_1") ? Converter.ToString(row["last_name_1"]) : null;
            user.LastName2 = row.Table.Columns.Contains("last_name_2") ? Converter.ToString(row["last_name_2"]) : null;
            user.Gender = row.Table.Columns.Contains("gender") ? Converter.ToString(row["gender"]) : null;
            user.Email = row.Table.Columns.Contains("email") ? Converter.ToString(row["email"]) : null;
            user.Phone = row.Table.Columns.Contains("phone") ? Converter.ToString(row["phone"]) : null;
            user.Username = row.Table.Columns.Contains("username") ? Converter.ToString(row["username"]) : null;
            user.CreatedAt = row.Table.Columns.Contains("created_at") ? Converter.ToDateTime(row["created_at"]) : null;
            user.UpdatedAt = row.Table.Columns.Contains("updated_at") ? Converter.ToDateTime(row["updated_at"]) : null;
            user.State = row.Table.Columns.Contains("state") ? Converter.ToString(row["state"]) : null;
            user.Type = row.Table.Columns.Contains("type") ? Converter.ToString(row["type"]) : null;

            return user;
        }
    }
}