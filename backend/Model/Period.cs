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
    /// Represent a period.
    /// </summary>
    /// <example>1 Semestre 2015</example>
    [DataContract]
    public class Period
    {
        private int? _value;
        private string _type;
        private int? _year;

        /// <summary>
        /// The period value.
        /// </summary>
        /// <example>1</example>
        [DataMember(Name = "value", Order = 1)]
        public int? Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// The period type.
        /// </summary>
        /// <example>Semestre</example>
        [DataMember(Name = "type", Order = 2)]
        public string Type
        {
            set
            {
                if (!Validator.IsValidPeriod(value))
                {
                    throw new InvalidParameterException("period.type", "Ingrese 'Bimestre', 'Trimestre', 'Cuatrimestre' o 'Semestre'");
                }

                _type = value;
            }
            get { return _type; }
        }

        /// <summary>
        /// The period year.
        /// </summary>
        /// <example>2015</example>
        [DataMember(Name = "year", Order = 3)]
        public int? Year
        {
            get { return _year; }
            set { _year = value; }
        }
    }
}