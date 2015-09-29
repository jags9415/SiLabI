using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;

namespace SiLabI.Util
{
    /// <summary>
    /// Class used to validate user inputs.
    /// </summary>
    public class Validator
    {
        /// <summary>
        /// Validate an user identification.
        /// </summary>
        /// <param name="id">The user identification.</param>
        /// <returns>True if the input is valid.</returns>
        public static bool IsValidId(int? id)
        {
            return id == null || id > 0;
        }

        /// <summary>
        /// Validate an email address.
        /// </summary>
        /// <param name="email">The email address.</param>
        /// <returns>True if the input is valid.</returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return true;

            try
            {
                MailAddress m = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// Validate a gender.
        /// </summary>
        /// <param name="gender">The gender.</param>
        /// <returns>True if the input is valid.</returns>
        public static bool IsValidGender(string gender)
        {
            var valid = new[] { "Masculino", "Femenino" };
            return gender == null || valid.Any(item => item.Equals(gender, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Validate a state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>True if the input is valid.</returns>
        public static bool IsValidState(string state)
        {
            var valid = new[] { "Activo", "Inactivo" };
            return state == null || valid.Any(item => item.Equals(state, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Validate an user state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>True if the input is valid.</returns>
        public static bool IsValidUserState(string state)
        {
            var valid = new[] { "Activo", "Inactivo", "Bloqueado" };
            return state == null || valid.Any(item => item.Equals(state, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Validate an user type.
        /// </summary>
        /// <param name="period">The type.</param>
        /// <returns>True if the input is valid.</returns>
        public static bool IsValidUserType(string type)
        {
            var valid = new[] { "Estudiante", "Docente", "Operador", "Administrador" };
            return type == null || valid.Any(item => item.Equals(type, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Validate a username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>True if the input is valid.</returns>
        public static bool IsValidUsername(string username)
        {
            return username == null || Regex.IsMatch(username, "^[a-zA-Z0-9]+$");
        }

        /// <summary>
        /// Validate a student username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>True if the input is valid.</returns>
        public static bool IsValidStudentUsername(string username)
        {
            return username == null || Regex.IsMatch(username, "^[0-9]+$");
        }

        /// <summary>
        /// Validate a period name.
        /// </summary>
        /// <param name="period">The period name.</param>
        /// <returns>True if the input is valid.</returns>
        public static bool IsValidPeriod(string period)
        {
            var valid = new[] { "Bimestre", "Trimestre", "Cuatrimestre", "Semestre" };
            return period == null || valid.Any(item => item.Equals(period, StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsValidAppointmentDate(DateTime date)
        {
            // Check if date is later than today.
            if (DateTime.Now.CompareTo(date) > 0)
            {
                return false;
            }
            // Check that date is in the next 2 weeks.
            if (DateTime.Now.AddDays(14).Date.CompareTo(date.Date) < 0)
            {
                return false;
            }
            // Checks that day is not weekend.
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }
            // Check that hour is between 8:00 am and 5:00 pm.
            if (date.Hour < 8 || date.Hour > 17)
            {
                return false;
            }
            // Check that no minute, seconds or milliseconds are provided.
            if (date.Minute != 0 || date.Second != 0 || date.Millisecond != 0)
            {
                return false;
            }
            return true;
        }
    }
}