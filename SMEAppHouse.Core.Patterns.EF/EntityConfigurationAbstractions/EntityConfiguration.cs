using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMEAppHouse.Core.CodeKits.Helpers;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces;
using SMEAppHouse.Core.Patterns.EF.Helpers;

/*
 
    DELETE all tables in db based on schema:

    NEW:
    ------------------------------------------------------------------------------

    DECLARE @Sql NVARCHAR(500) DECLARE @Cursor CURSOR

    SET @Cursor = CURSOR FAST_FORWARD FOR
    SELECT DISTINCT sql = 'ALTER TABLE [' + tc2.TABLE_SCHEMA + '].[' +  tc2.TABLE_NAME + '] DROP [' + rc1.CONSTRAINT_NAME + '];'
    FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc1
    LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc2 ON tc2.CONSTRAINT_NAME =rc1.CONSTRAINT_NAME

    OPEN @Cursor FETCH NEXT FROM @Cursor INTO @Sql

    WHILE (@@FETCH_STATUS = 0)
    BEGIN
    Exec sp_executesql @Sql
    FETCH NEXT FROM @Cursor INTO @Sql
    END

    CLOSE @Cursor DEALLOCATE @Cursor
    GO

    EXEC sp_MSforeachtable 'DROP TABLE ?'
    GO

    OLD:
    ------------------------------------------------------------------------------
    
    DECLARE @SqlStatement NVARCHAR(MAX)
    SELECT @SqlStatement = 
        COALESCE(@SqlStatement, N'') + N'DROP TABLE [pow].' + QUOTENAME(TABLE_NAME) + N';' + CHAR(13)
    FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_SCHEMA = 'pow' and TABLE_TYPE = 'BASE TABLE'

    PRINT @SqlStatement
     
     */

namespace SMEAppHouse.Core.Patterns.EF.EntityConfigurationAbstractions;

public abstract class EntityConfiguration<TEntity, TPk> : IEntityConfiguration<TEntity, TPk>
    where TEntity : class, IKeyedEntity<TPk>
    where TPk : struct
{
    private readonly string _altTableName;
    private readonly bool _prefixEntityNameToId;
    private readonly bool _prefixAltTblNameToEntity;
    private readonly string _preferredPKeyId;
    private readonly bool _pluralizeTblName;

    private readonly ModelBuilder _modelBuilder;

    public string Schema { get; private set; }
    public Expression<Func<TEntity, object>>[] FieldsToIgnore { get; set; }

    public virtual void OnModelCreating(EntityTypeBuilder<TEntity> entityBuilder)
    {
        SetupConventionalFields<TEntity>(entityBuilder, FieldsToIgnore);
    }

    public virtual void OnModelCreating(ModelBuilder modelBuilder)
    {

    }

    #region constructors

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="prefixEntityNameToId"></param>
    /// <param name="prefixAltTblNameToEntity"></param>
    /// <param name="schema"></param>
    /// <param name="pluralizeTblName"></param>
    protected EntityConfiguration(ModelBuilder builder,
        bool prefixEntityNameToId = false,
        bool prefixAltTblNameToEntity = false,
        string schema = "dbo",
        bool pluralizeTblName = true)
        : this("", prefixEntityNameToId
            , prefixAltTblNameToEntity
            , schema
            , pluralizeTblName)
    {
        _modelBuilder = builder;
    }

    protected EntityConfiguration(
        bool prefixEntityNameToId = false,
        bool prefixAltTblNameToEntity = false,
        string schema = "dbo",
        bool pluralizeTblName = true)
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
    protected EntityConfiguration(string pluralTableName = ""
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
    protected EntityConfiguration(string pluralTableName = ""
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
    private EntityConfiguration(string pluralTableName = ""
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

    public void Configure(EntityTypeBuilder<TEntity> entityBuilder)
    {
        var tblName = string.IsNullOrEmpty(_altTableName) ? typeof(TEntity).Name : _altTableName;

        if (_prefixAltTblNameToEntity && !string.IsNullOrEmpty(_altTableName))
            tblName += typeof(TEntity).Name;

        var pKeyId = (_prefixEntityNameToId ? typeof(TEntity).Name : "") + _preferredPKeyId;

        entityBuilder
            //.ToTable(_schema + $"{(!string.IsNullOrEmpty(_schema) ? "." : "")}{(_pluralizeTblName ? tblName.Pluralize() : tblName)}", _schema)
            .ToTable($"{(_pluralizeTblName ? tblName.Pluralize() : tblName)}", Schema)
            .HasKey(i => i.Id);

        entityBuilder
            .Property(i => i.Id)
            .ValueGeneratedOnAdd()
            //.HasAnnotation("DatabaseGenerationOption", "Identity")
            .HasColumnName(pKeyId)
            .IsRequired();

        entityBuilder
            .Property(i => i.IsActive)
            .HasDefaultValue(true)
            .IsRequired(false);

        OnModelCreating(entityBuilder);
        OnModelCreating(_modelBuilder);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entityBuilder"></param>
    /// <param name="conventionFieldsToIgnore"></param>
    /// <param name="archivable"></param>
    private static void SetupConventionalFields<T>(EntityTypeBuilder<TEntity> entityBuilder, Expression<Func<TEntity, object>>[] conventionFieldsToIgnore)
        where T : class, IKeyedEntity<TPk>
    {
        var fieldsToIgnore = new List<Expression<Func<TEntity, object>>>(conventionFieldsToIgnore ?? []);

        foreach (var exprssn in fieldsToIgnore)
            entityBuilder.Ignore(exprssn);

        var fieldsIgnoreList = fieldsToIgnore.ToArray().ToListOfFields();

        // add the rest not ignored
        entityBuilder.RegisterConventionalField(fieldsIgnoreList, entity => entity.DateCreated)?
            .HasColumnName("_dateCreated").HasDefaultValue(DateTime.UtcNow).IsRequired(true);

        entityBuilder.RegisterConventionalField(fieldsIgnoreList, entity => entity.DateModified)?
            .HasColumnName("_dateModified").IsRequired(false);
    }
}