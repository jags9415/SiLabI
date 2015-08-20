using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiLabI.Util
{
    /// <summary>
    /// Defines comparation operators or relationships.
    /// </summary>
    public enum Relationship { EQ, NE, GE, LE, GT, LT, LIKE }

    /// <summary>
    /// Provides utilities to handle Relationships.
    /// </summary>
    public class RelationshipUtils
    {
        /// <summary>
        /// Get the string representation of a relationship.
        /// </summary>
        /// <param name="relationship">The relationship.</param>
        /// <returns>The string representation.</returns>
        public static string ToString(Relationship relationship)
        {
            switch (relationship)
            {
                case Relationship.EQ:
                    return "=";
                case Relationship.GE:
                    return ">=";
                case Relationship.GT:
                    return ">";
                case Relationship.LE:
                    return "<=";
                case Relationship.LT:
                    return "<";
                case Relationship.NE:
                    return "!=";
                case Relationship.LIKE:
                    return "LIKE";
                default:
                    return null;
            }
        }

        /// <summary>
        /// Try to convert a string into a relationship.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="relationship">The relationship.</param>
        /// <returns>True if the conversion was successful</returns>
        public static bool TryParse(string str, out Relationship relationship)
        {
            switch (str.ToLower())
            {
                case "eq":
                    relationship = Relationship.EQ;
                    return true;
                case "ne":
                    relationship = Relationship.NE;
                    return true;
                case "gt":
                    relationship = Relationship.GT;
                    return true;
                case "lt":
                    relationship = Relationship.LT;
                    return true;
                case "ge":
                    relationship = Relationship.GE;
                    return true;
                case "le":
                    relationship = Relationship.LE;
                    return true;
                case "like":
                    relationship = Relationship.LIKE;
                    return true;
                default:
                    relationship = Relationship.EQ;
                    return false;
            }
        }
    }
}