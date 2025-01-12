using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMEAppHouse.Core.CodeKits.Helpers;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces;
using SMEAppHouse.Core.Patterns.EF.Helpers;

/*
 
    DELETE all tables in db based on schema:

    DECLARE @SqlStatement NVARCHAR(MAX)
SELECT @SqlStatement = 
    COALESCE(@SqlStatement, N'') + N'DROP TABLE [pow].' + QUOTENAME(TABLE_NAME) + N';' + CHAR(13)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'pow' and TABLE_TYPE = 'BASE TABLE'

PRINT @SqlStatement
     
     */

namespace SMEAppHouse.Core.Patterns.EF.EntityConfigurationAbstractions
{
    public abstract class EntityConfigurationBase<TEntity, TPk> : IEntityConfiguration<TEntity>
        where TEntity : class, IKeyedEntity<TPk>
        where TPk : struct
    {
        private readonly string _altTableName;
        private readonly bool _prefixEntityNameToId;
        private readonly bool _prefixAltTblNameToEntity;
        private readonly string _preferredPKeyId;
        private readonly bool _pluralizeTblName;

        public string Schema { get; private set; }
        public Expression<Func<TEntity, object>>[] FieldsToIgnore { get; set; }

        public virtual void Map(EntityTypeBuilder<TEntity> entityBuilder)
        {
            SetupConventions<TEntity>(entityBuilder, FieldsToIgnore);
        }

        #region constructors

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="prefixEntityNameToId"></param>
        /// <param name="prefixAltTblNameToEntity"></param>
        /// <param name="schema"></param>
        /// <param name="pluralizeTblName"></param>
        protected EntityConfigurationBase(bool prefixEntityNameToId = false
                                            , bool prefixAltTblNameToEntity = false
                                            , string schema = "dbo"
                                            , bool pluralizeTblName = true)
                                            : this("", prefixEntityNameToId
                                                  , prefixAltTblNameToEntity
                                                  , schema
                                                  , pluralizeTblName)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="pluralTableName"></param>
        /// <param name="prefixEntityNameToId"></param>
        /// <param name="prefixAltTblNameToEntity"></param>
        /// <param name="schema"></param>
        /// <param name="pluralizeTblName"></param>
        protected EntityConfigurationBase(string pluralTableName = ""
                                        , bool prefixEntityNameToId = false
                                        , bool prefixAltTblNameToEntity = false
                                        , string schema = "dbo"
                                        , bool pluralizeTblName = true)
                                        : this(pluralTableName
                                              , prefixEntityNameToId
                                              , prefixAltTblNameToEntity
                                              , "Id"
                                              , schema
                                            , pluralizeTblName)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="pluralTableName"></param>
        /// <param name="prefixAltTblNameToEntity"></param>
        /// <param name="preferredPKeyId"></param>
        /// <param name="schema"></param>
        /// <param name="pluralizeTblName"></param>
        protected EntityConfigurationBase(string pluralTableName = ""
                                        , bool prefixAltTblNameToEntity = false
                                        , string preferredPKeyId = "Id"
                                        , string schema = "dbo"
                                        , bool pluralizeTblName = true)
            : this(pluralTableName
                , false
                , prefixAltTblNameToEntity
                , preferredPKeyId
                , schema
                , pluralizeTblName)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pluralTableName"></param>
        /// <param name="prefixEntityNameToId"></param>
        /// <param name="prefixAltTblNameToEntity"></param>
        /// <param name="preferredPKeyId"></param>
        /// <param name="schema"></param>
        /// <param name="pluralizeTblName"></param>
        private EntityConfigurationBase(string pluralTableName = ""
                                        , bool prefixEntityNameToId = false
                                        , bool prefixAltTblNameToEntity = false
                                        , string preferredPKeyId = "Id"
                                        , string schema = "dbo"
                                        , bool pluralizeTblName = true)
        {
            Schema = schema;

            _altTableName = pluralTableName;
            _prefixEntityNameToId = prefixEntityNameToId;
            _prefixAltTblNameToEntity = prefixAltTblNameToEntity;
            _preferredPKeyId = preferredPKeyId;
            _pluralizeTblName = pluralizeTblName;
        }

        #endregion

        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            var tblName = string.IsNullOrEmpty(_altTableName) ? typeof(TEntity).Name : _altTableName;

            if (_prefixAltTblNameToEntity && !string.IsNullOrEmpty(_altTableName))
                tblName = tblName + typeof(TEntity).Name;

            var pKeyId = (_prefixEntityNameToId ? typeof(TEntity).Name : "") + _preferredPKeyId;

            builder
                //.ToTable(_schema + $"{(!string.IsNullOrEmpty(_schema) ? "." : "")}{(_pluralizeTblName ? tblName.Pluralize() : tblName)}", _schema)
                .ToTable($"{(_pluralizeTblName ? tblName.Pluralize() : tblName)}", Schema)
                .HasKey(i => i.Id);

            builder
                .Property(i => i.Id)
                .HasAnnotation("DatabaseGenerationOption", "Identity")
                .HasColumnName(pKeyId)
                .IsRequired();

            Map(builder);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPk"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityBuilder"></param>
        /// <param name="conventionFieldsToIgnore"></param>
        private static void SetupConventions<T>(EntityTypeBuilder<TEntity> entityBuilder, Expression<Func<TEntity, object>>[] conventionFieldsToIgnore)
            where T : class, IKeyedEntity<TPk>
        {
            if (conventionFieldsToIgnore != null && conventionFieldsToIgnore.Any())
            {
                foreach (var exprssn in conventionFieldsToIgnore)
                {
                    entityBuilder.Ignore(exprssn);
                }
            }

            var fieldsIgnoreList = conventionFieldsToIgnore.ToListOfFields();
            // add the rest not ignored
            entityBuilder.RegisterConventionalField<TEntity>(fieldsIgnoreList, entity => entity.Ordinal)?
                .HasColumnName("ordinal").HasDefaultValue(0).IsRequired(false);

            // UsePropertyAccessMode(PropertyAccessMode.Property) :: https://stackoverflow.com/a/50776738/3796898
            // fix to error related to "Expression of type 'System.Nullable`1[System.Boolean]' cannot be used for assignment to type 'System.Boolean'"
            entityBuilder.RegisterConventionalField<TEntity>(fieldsIgnoreList, entity => entity.IsNotActive)?
                .HasColumnName("IsNotActive").HasDefaultValue(false).IsRequired(false)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            entityBuilder.RegisterConventionalField<TEntity>(fieldsIgnoreList, entity => entity.DateCreated)?
                .HasColumnName("dateCreated").HasDefaultValue(DateTime.Now).IsRequired(true);

            entityBuilder.RegisterConventionalField<TEntity>(fieldsIgnoreList, entity => entity.DateRevised)?
                .HasColumnName("dateRevised").IsRequired(false);

            entityBuilder.RegisterConventionalField<TEntity>(fieldsIgnoreList, entity => entity.CreatedBy)?
                .HasColumnName("createdBy").IsRequired(false);

            entityBuilder.RegisterConventionalField<TEntity>(fieldsIgnoreList, entity => entity.RevisedBy)?
                .HasColumnName("revisedBy").IsRequired(false);
        }

    }


}