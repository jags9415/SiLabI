using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace SiLabI.Model.Query
{
    /// <summary>
    /// A field used in GET querys.
    /// A field is a property of a domain model object, for example an user gender or a group code.
    /// A field is represented using Dot Notation, for example group.code or group.professor.name
    /// </summary>
    public class Field
    {
        /// <summary>
        /// Creates a new Field.
        /// </summary>
        /// <param name="parent">The parent field.</param>
        /// <param name="children">The children fields.</param>
        /// <param name="name">The field name.</param>
        /// <param name="type">The field type.</param>
        private Field(Field parent, List<Field> children, string name, SqlDbType type)
        {
            this.Name = name;
            this.Type = type;
            this.Children = new List<Field>();
            this.Parent = parent;

            Field field;
            foreach (Field child in children)
            {
                field = new Field(child);
                field.Parent = this;
                this.Children.Add(field);
            }
        }

        /// <summary>
        /// Create a new Field
        /// </summary>
        /// <param name="name">The name.</param>
        public Field(string name, List<Field> children) : this(null, children, name, SqlDbType.Structured) { }

        /// <summary>
        /// Creates a new Field.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        public Field(string name, SqlDbType type) : this(null, new List<Field>(), name, type) { }

        /// <summary>
        /// Creates a new Field.
        /// </summary>
        /// <param name="field">The field.</param>
        public Field(Field field) : this(field.Parent, field.Children, field.Name, field.Type) { }

        /// <summary>
        /// The name of the field.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of the field.
        /// </summary>
        public SqlDbType Type { get; set; }

        /// <summary>
        /// The parent field.
        /// </summary>
        public Field Parent { get; set; }

        /// <summary>
        /// The children fields.
        /// </summary>
        public List<Field> Children { get; set; }

        /// <summary>
        /// Return the string representation of the field.
        /// This is the represented using Dot Notation in a recursive way.
        /// </summary>
        /// <returns>The field as string.</returns>
        /// <example>group.course.id</example>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(this.Name);
            Field field = this;

            while (field.Parent != null)
            {
                sb.Insert(0, ".");
                sb.Insert(0, field.Parent.Name);
                field = field.Parent;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Finds a field recursively on a list of fields.
        /// The field name is splitted by the '.' separator and then each subfield is searched
        /// in the list in a recursive way.
        /// 
        /// Example:
        /// Field.Find([Field{group}, Field{course}, Field{student}], "group.professor.name")
        /// 
        ///                list                       |           splitted field       |      result
        ///  =========================================|================================|==================
        ///  [Field{group}, Field{course}, ...]       | ["group", "professor", "name"] | Field{group}
        ///  Field{group}.Children                    | ["professor", "name"]          | Field{professor}
        ///  Field{professor}.Children                | ["name"]                       | Field{name}
        ///  
        /// => Field{name}
        /// </summary>
        /// <param name="fields">The list.</param>
        /// <param name="name">The field name.</param>
        /// <returns>The field is found, else null.</returns>
        public static Field Find(List<Field> fields, string name)
        {
            var splitted = name.Split(new[] { "." }, StringSplitOptions.None);
            Field field = null;

            foreach (var item in splitted)
            {
                field = fields.Find(element => element.Name == item);

                if (field == null)
                {
                    return null;
                }

                if (field.Children.Count > 0)
                {
                    fields = field.Children;
                }
            }

            return field;
        }

        /// <summary>
        /// Flatten a list of fields by recursively inserting the children fields into a new list. 
        /// </summary>
        /// <param name="fields">A list of fields.</param>
        /// <returns>The flatten list.</returns>
        public static List<Field> Flatten(List<Field> fields)
        {
            List<Field> result = new List<Field>();

            foreach (Field field in fields)
            {
                if (field.Type == SqlDbType.Structured)
                {
                    result.AddRange(Field.Flatten(field.Children));
                }
                else
                {
                    result.Add(field);
                }
            }

            return result;
        }
    }
}