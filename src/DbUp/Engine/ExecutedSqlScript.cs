using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbUp.Engine
{
    /// <summary>
    /// Represents a SQL Server script that comes from database journal. 
    /// </summary>
    public class ExecutedSqlScript
    {
        private readonly string name;
        private readonly string hash;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutedSqlScript"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="hash">The script hash.</param>
        public ExecutedSqlScript(string name, string hash)
        {
            this.name = name;
            this.hash = hash;
        }

        /// <summary>
        /// Gets the name of the script.
        /// </summary>
        /// <value></value>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the hash of the script.
        /// </summary>
        public string Hash
        {
            get
            {
                return hash;
            }
        }
    }
}
