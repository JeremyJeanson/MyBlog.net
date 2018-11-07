using MyBlog.Engine.Data.Models;
using System;
using System.Data.Entity;

namespace MyBlog.Engine.Data
{
    public sealed class DataContext: DbContext
    {
        #region Declarations

        private const String ConnectionStringName = "Entities";

        #endregion

        #region Constructors

        public DataContext() : base(ConnectionStringName)
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// Utilisateurs XML RPC
        /// </summary>
        public DbSet<Publisher> Publishers { get; set; }

        /// <summary>
        /// Posts
        /// </summary>
        public DbSet<Post> Posts { get; set; }

        /// <summary>
        /// Categories
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Users
        /// </summary>
        public DbSet<UserProfile> Users { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        public DbSet<Comment> Comments { get; set; }

        #endregion

        #region Methodes

        #endregion
    }
}
