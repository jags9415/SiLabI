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
        private static List<DateTime> _holidays;

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
        /// <param name="message">The output error message.</param>
        /// <returns>True if the input is valid.</returns>
        public static bool IsValidAppointmentDate(DateTime date, out string message)
        {
            // Check that date is in the next 2 weeks.
            if (DateTime.Now.AddDays(14).Date.CompareTo(date.Date) < 0)
            {
                message = "Ingrese un día anterior a dos semanas.";
                return false;
            }
            // Checks that day is not weekend.
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                message = "No se permiten citas durante fines de semana.";
                return false;
            }
            // Check that day is not a holiday.
            if (Holidays.Contains(date))
            {
                message = "No se permiten citas durante feriados.";
                return false;
            }
            // Check that hour is between 8:00 am and 5:00 pm.
            if (date.Hour < 8 || date.Hour > 17)
            {
                message = "Ingrese una hora entre 8:00 am y 5:00 pm.";
                return false;
            }
            // Check that no minute, seconds or milliseconds are provided.
            if (date.Minute != 0 || date.Second != 0 || date.Millisecond != 0)
            {
                message = "Ingrese unicamente horas exactas.";
                return false;
            }
            message = string.Empty;
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
        /// <param name="message">The output error message.</param>
        /// <returns>True if the input is valid.</returns>
        public static bool IsValidReservationDate(DateTime date, out string message)
        {
            // Check if date is later than today.
            if (DateTime.Now.CompareTo(date) > 0)
            {
                message = "Ingrese un día posterior a la fecha actual.";
                return false;
            }
            // Checks that day is not weekend.
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                message = "No se permiten reservaciones durante fines de semana.";
                return false;
            }
            // Check that day is not a holiday.
            if (Holidays.Contains(date))
            {
                message = "No se permiten reservaciones durante feriados.";
                return false;
            }
            // Check that hour is between 8:00 am and 6:00 pm.
            if (date.Hour < 8 || date.Hour > 18)
            {
                message = "Ingrese una hora entre 8:00 am y 6:00 pm.";
                return false;
            }
            // Check that no minute, seconds or milliseconds are provided.
            if (date.Minute != 0 || date.Second != 0 || date.Millisecond != 0)
            {
                message = "Ingrese unicamente horas exactas.";
                return false;
            }
            message = string.Empty;
            return true;
        }

        /// <summary>
        /// Validate a reservation start and end times.
        /// A valid range must meet the following:
        ///     1. The end time must be after the start time.
        ///     2. The start time and the end time must be in the same day.
        /// </summary>
        /// <param name="startTime">The reservation start time.</param>
        /// <param name="endTime">The reservation end time.</param>
        /// <param name="message">The output error message.</param>
        /// <returns></returns>
        public static bool IsValidReservationTimeRange(DateTime startTime, DateTime endTime, out string message)
        {
            if (startTime.CompareTo(endTime) >= 0)
            {
                message = "La hora final debe ser posterior a la hora de inicio.";
                return false;
            }
            if (startTime.DayOfYear != endTime.DayOfYear)
            {
                message = "El dia de inicio debe ser igual al dia final.";
                return false;
            }
            message = string.Empty;
            return true;
        }

        /// <summary>
        /// The list of holidays in Costa Rica.
        /// </summary>
        private static List<DateTime> Holidays
        {
            get
            {
                if (_holidays == null)
                {
                    _holidays = new List<DateTime>();
                    int year = DateTime.Now.Year;

                    _holidays.Add(new DateTime(year, 1, 1));   // 1 Enero.
                    _holidays.Add(new DateTime(year, 4, 11));  // 11 Abril.
                    _holidays.Add(new DateTime(year, 5, 1));   // 5 Mayo.
                    _holidays.Add(new DateTime(year, 7, 25));  // 25 Julio.
                    _holidays.Add(new DateTime(year, 8, 2));   // 2 Agosto.
                    _holidays.Add(new DateTime(year, 8, 15));  // 15 Agosto.
                    _holidays.Add(new DateTime(year, 9, 15));  // 15 Septiembre.
                    _holidays.Add(new DateTime(year, 10, 12)); // 12 Octubre.
                    _holidays.Add(new DateTime(year, 12, 25)); // 25 Diciembre.
                }
                return _holidays;
            }
        }
    }
}