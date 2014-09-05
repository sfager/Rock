using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rock.Client
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Person
    {
        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public virtual string FullName
        {
            get
            {
                var fullName = new StringBuilder();

                fullName.AppendFormat( "{0} {1}", NickName, LastName );

                return fullName.ToString();
            }
        }

        /// <summary>
        /// Gets the primary alias.
        /// </summary>
        /// <value>
        /// The primary alias.
        /// </value>
        public virtual PersonAlias PrimaryAlias
        {
            get
            {
                return Aliases.FirstOrDefault( a => a.AliasPersonId == Id );
            }
        }

        /// <summary>
        /// Gets the primary alias identifier.
        /// </summary>
        /// <value>
        /// The primary alias identifier.
        /// </value>
        public virtual int? PrimaryAliasId
        {
            get
            {
                var primaryAlias = PrimaryAlias;
                if ( primaryAlias != null )
                {
                    return primaryAlias.Id;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets the aliases.
        /// </summary>
        /// <value>
        /// The aliases.
        /// </value>
        public virtual List<PersonAlias> Aliases { get; set; }
    }
}
