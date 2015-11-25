using SiLabI.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel.Web;
using System.Text.RegularExpressions;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// A student data.
    /// </summary>
    [DataContract]
    public class Student : User
    {
        /// <summary>
        /// Creates a empty student.
        /// </summary>
        public Student() : base() { }

        /// <summary>
        /// Clones an user.
        /// </summary>
        /// <param name="user">The user to clone.</param>
        public Student(User user) : base(user) { }

        /// <summary>
        /// The username.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "username")]
        public override string Username
        {
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && !Regex.IsMatch(value, "^[0-9]+$"))
                {
                    throw new InvalidParameterException("username", "Ingrese únicamente caractéres numéricos");
                }
                _username = value;
            }
            get { return _username; }
        }

        /// <summary>
        /// Fill an Student object with the data provided in a DataRow.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="prefix">A string that will be prefixed to the column names of the row.</param>
        /// <returns>The Student.</returns>
        public static Student Parse(DataRow row, string prefix = "")
        {
            Student student = new Student(User.Parse(row, prefix));

            if (student.isEmpty())
            {
                student = null;
            }

            return student;
        }
    }
}