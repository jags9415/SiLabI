using SiLabI.Exceptions;
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
    public class User : DatabaseObject
    {
        protected string _name;
        protected string _lastname1;
        protected string _lastname2;
        protected string _fullname;
        protected string _username;
        protected string _password;
        protected string _gender;
        protected string _email;
        protected string _phone;
        protected string _type;

        /// <summary>
        /// Creates a empty user.
        /// </summary>
        public User() { }

        /// <summary>
        /// Clones an user.
        /// </summary>
        /// <param name="user">The user to clone.</param>
        public User(User user)
        {
            if (user != null)
            {
                this.Id = user.Id;
                this.Name = user.Name;
                this.LastName1 = user.LastName1;
                this.LastName2 = user.LastName2;
                this.FullName = user.FullName;
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
        }

        /// <summary>
        /// The name.
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
        /// The full name.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "full_name")]
        public virtual string FullName
        {
            set { _fullname = value; }
            get { return _fullname; }
        }

        /// <summary>
        /// The username.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "username")]
        public virtual string Username
        {
            set
            {
                if (!Validator.IsValidUsername(value))
                {
                    throw new InvalidParameterException("username", "Ingrese únicamente caractéres alfanuméricos");
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
                if (!Validator.IsValidEmail(value))
                {
                    throw new InvalidParameterException("email");
                }
                _email = value;
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
                if (!Validator.IsValidGender(value))
                {
                    throw new InvalidParameterException("gender", "Ingrese 'Masculino' o 'Femenino'");
                }
                _gender = value; 
            }
            get { return _gender; }
        }

        /// <summary>
        /// The state.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "state")]
        public override string State
        {
            set
            {
                if (!Validator.IsValidUserState(value))
                {
                    throw new InvalidParameterException("state", "Ingrese 'Activo', 'Inactivo' o 'Bloqueado'");
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
            set
            {
                if (!Validator.IsValidUserType(value))
                {
                    throw new InvalidParameterException("type", "Ingrese 'Estudiante', 'Docente', 'Operador' o 'Administrador'");
                }
                _type = value;
            }
            get { return _type; }
        }

        /// <summary>
        /// Check if the object properties are valid for a create operation.
        /// </summary>
        /// <returns>True if the object properties are valid for a create operation.</returns>
        public override bool IsValidForCreate()
        {
            bool valid = true;

            valid &= !string.IsNullOrWhiteSpace(Name);
            valid &= !string.IsNullOrWhiteSpace(LastName1);
            valid &= !string.IsNullOrWhiteSpace(Gender);
            valid &= !string.IsNullOrWhiteSpace(Username);
            valid &= !string.IsNullOrWhiteSpace(Password);

            return valid;
        }

        /// <summary>
        /// Check if the object properties are valid for an update operation.
        /// </summary>
        /// <returns>True if the object properties are valid for an update operation.</returns>
        public override bool IsValidForUpdate()
        {
            bool valid = true;

            if (Name != null) valid &= !string.IsNullOrWhiteSpace(Name);
            if (LastName1 != null) valid &= !string.IsNullOrWhiteSpace(LastName1);
            if (Gender != null) valid &= !string.IsNullOrWhiteSpace(Gender);
            if (Username != null) valid &= !string.IsNullOrWhiteSpace(Username);
            if (Password != null) valid &= !string.IsNullOrWhiteSpace(Password);

            return valid;
        }

        /// <summary>
        /// Fill an User object with the data provided in a DataRow.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="prefix">A string that will be prefixed to the column names of the row.</param>
        /// <returns>The User.</returns>
        public static User Parse(DataRow row, string prefix = "")
        {
            prefix = prefix.Trim();
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                prefix += ".";
            }

            User user = new User();

            user.Id = row.Table.Columns.Contains(prefix + "id") ? Converter.ToNullableInt32(row[prefix + "id"]) : null;
            user.Name = row.Table.Columns.Contains(prefix + "name") ? Converter.ToString(row[prefix + "name"]) : null;
            user.LastName1 = row.Table.Columns.Contains(prefix + "last_name_1") ? Converter.ToString(row[prefix + "last_name_1"]) : null;
            user.LastName2 = row.Table.Columns.Contains(prefix + "last_name_2") ? Converter.ToString(row[prefix + "last_name_2"]) : null;
            user.FullName = row.Table.Columns.Contains(prefix + "full_name") ? Converter.ToString(row[prefix + "full_name"]) : null;
            user.Gender = row.Table.Columns.Contains(prefix + "gender") ? Converter.ToString(row[prefix + "gender"]) : null;
            user.Email = row.Table.Columns.Contains(prefix + "email") ? Converter.ToString(row[prefix + "email"]) : null;
            user.Phone = row.Table.Columns.Contains(prefix + "phone") ? Converter.ToString(row[prefix + "phone"]) : null;
            user.Username = row.Table.Columns.Contains(prefix + "username") ? Converter.ToString(row[prefix + "username"]) : null;
            user.CreatedAt = row.Table.Columns.Contains(prefix + "created_at") ? Converter.ToDateTime(row[prefix + "created_at"]) : null;
            user.UpdatedAt = row.Table.Columns.Contains(prefix + "updated_at") ? Converter.ToDateTime(row[prefix + "updated_at"]) : null;
            user.State = row.Table.Columns.Contains(prefix + "state") ? Converter.ToString(row[prefix + "state"]) : null;
            user.Type = row.Table.Columns.Contains(prefix + "type") ? Converter.ToString(row[prefix + "type"]) : null;

            if (user.isEmpty())
            {
                user = null;
            }

            return user;
        }
    }
}