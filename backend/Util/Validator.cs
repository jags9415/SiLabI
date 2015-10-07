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
        /// A valid user indentification can be null or an integer greater than zero.
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
        /// A gender can be "Masculino" or "Femenino".
        /// </summary>
        /// <param name="gender">The gender.</param>
        /// <returns>True if the input is valid.</returns>
        public static bool IsValidGender(string gender)
        {
            var valid = new[] { "Masculino", "Femenino" };
            return gender == null || valid.Any(item => item.Equals(gender, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Validate an user state.
        /// An user state can be "Activo", "Inactivo" or "Bloqueado".
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
        /// An user type can be "Estudiante", "Docente", "Operador" or "Administrador".
        /// </summary>
        /// <param name="period">The type.</param>
        /// <returns>True if the input is valid.</returns>
        public static bool IsValidUserType(string type)
        {
            var valid = new[] { "Estudiante", "Docente", "Operador", "Administrador" };
            return type == null || valid.Any(item => item.Equals(type, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Validate an username.
        /// A valid username is a non-empty string containing only letters and numbers.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>True if the input is valid.</returns>
        public static bool IsValidUsername(string username)
        {
            return username == null || Regex.IsMatch(username, "^[a-zA-Z0-9]+$");
        }

        /// <summary>
        /// Validate a student username.
        /// A valid student username is a non-empty string containing only numbers.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>True if the input is valid.</returns>
        public static bool IsValidStudentUsername(string username)
        {
            return username == null || Regex.IsMatch(username, "^[0-9]+$");
        }

        /// <summary>
        /// Validate a period name.
        /// A period type can be "Bimestre", "Trimestre", "Cuatrimestre" or "Semestre".
        /// </summary>
        /// <param name="period">The period name.</param>
        /// <returns>True if the input is valid.</returns>
        public static bool IsValidPeriod(string period)
        {
            var valid = new[] { "Bimestre", "Trimestre", "Cuatrimestre", "Semestre" };
            return period == null || valid.Any(item => item.Equals(period, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Validate an appointment date.
        /// A valid date must meet the following:
        ///     1. The date must be after the current date.
        ///     2. The date must be prior to two weeks.
        ///     3. The day is not a weekend.
        ///     4. The hour is between 8:00 am and 5:00 pm.
        ///     5. Only Year, Month, Day and Hour should be provided.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>True if the input is valid.</returns>
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

        /// <summary>
        /// Validate a reservation date.
        /// A valid date must meet the following:
        ///     1. The date must be after the current date.
        ///     3. The day is not a weekend.
        ///     4. The hour is between 8:00 am and 6:00 pm.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>True if the input is valid.</returns>
        public static bool IsValidReservationDate(DateTime date)
        {
            // Check if date is later than today.
            if (DateTime.Now.CompareTo(date) > 0)
            {
                return false;
            }
            // Checks that day is not weekend.
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }
            // Check that hour is between 8:00 am and 6:00 pm.
            if (date.Hour < 8 || date.Hour > 18)
            {
                return false;
            }
            return true;
        }
    }
}