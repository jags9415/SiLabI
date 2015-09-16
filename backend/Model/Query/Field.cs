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
    /// A field is a property of a domain model object. 
    /// For example an user gender or a group code.
    /// 
    /// Is contained in the ?fields={fields} subquery.
    /// </summary>
    public class Field
    {
        /// <summary>
        /// Creates a new Field.
        /// </summary>
        /// <param name="children">The children fields.</param>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        private Field(Field parent, List<Field> children, string name, SqlDbType type)
        {
            this.Name = name;
            this.Type = type;
            this.Children = children;
            this.Parent = parent;

            foreach (Field child in children)
            {
                child.Parent = this;
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
        /// The type of this field.
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
    }
}