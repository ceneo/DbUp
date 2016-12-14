
using System;
using System.IO;
using System.Text;
using DbUp.Engine.Hashers;

namespace DbUp.Engine
{
    /// <summary>
    /// Represents a SQL Server script that comes from an embedded resource in an assembly. 
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Name}")]
    public class SqlScript 
    {
        private readonly string contents;
        private readonly string name;
        private string hash;
        private readonly IHasher hasher;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlScript"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="contents">The contents.</param>
        /// <param name="hasher">The hasher implementations</param>
        public SqlScript(string name, string contents, IHasher hasher)
        {
            this.name = name;
            this.contents = contents;
            this.hasher = hasher;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlScript"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="contents">The contents.</param>
        public SqlScript(string name, string contents):this(name, contents, new Md5WithMarkHasher())
        {}

        /// <summary>
        /// Gets the hash of the script.
        /// </summary>
        public string Hash
        {
            get
            {
                if (string.IsNullOrEmpty(hash))
                {
                    hash = GenerateHash();
                } 

                return hash;
            }
        }

        /// <summary>
        /// Generate hash from current instance
        /// </summary>
        /// <returns></returns>
        protected virtual string GenerateHash()
        {
            return GenerateHash(name + contents);
        }

        /// <summary>
        /// Generates hash from provided input using hasher
        /// </summary>
        /// <returns></returns>
        protected string GenerateHash(string input)
        {
            return hasher.GenerateHash(input);
        }

        /// <summary>
        /// Gets the contents of the script.
        /// </summary>
        /// <value></value>
        public virtual string Contents
        {
            get { return contents; }
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
        /// Create a SqlScript from a file using Default encoding
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static SqlScript FromFile(string path)
        {
            return FromFile(path, Encoding.Default);
        }

        /// <summary>
        /// Create a SqlScript from a file using specified encoding
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static SqlScript FromFile(string path, Encoding encoding)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var fileName = new FileInfo(path).Name;
                return FromStream(fileName, fileStream, encoding);
            }
        }

        /// <summary>
        /// Create a SqlScript from a stream using Default encoding
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static SqlScript FromStream(string scriptName, Stream stream)
        {
            return FromStream(scriptName, stream, Encoding.Default);
        }

        /// <summary>
        /// Create a SqlScript from a stream using specified encoding
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static SqlScript FromStream(string scriptName, Stream stream, Encoding encoding)
        {
            using (var resourceStreamReader = new StreamReader(stream, encoding, true))
            {
                string c = resourceStreamReader.ReadToEnd();
                return new SqlScript(scriptName, c);
            }
        }

        /// <summary>
        /// Check if current sql script is the same as provided executed script
        /// </summary>
        /// <param name="executedSqlScript"></param>
        /// <returns></returns>
        public bool MatchTo(ExecutedSqlScript executedSqlScript)
        {
            if (Name != executedSqlScript.Name)
                return false;

            if (string.IsNullOrEmpty(executedSqlScript.Hash))
                return true;

            if(!hasher.Verify(executedSqlScript.Hash))
                throw new NotSupportedException("Wrong hasher implementation. You should use the same hasher to avoid executing scripts again.");

            return executedSqlScript.Hash == Hash;
        }
    }
}