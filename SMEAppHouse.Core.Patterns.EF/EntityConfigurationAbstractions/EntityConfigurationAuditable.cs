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

public abstract class EntityConfigurationAuditable<TEntity, TPk> : IEntityConfiguration<TEntity, TPk>
    where TEntity : class, IKeyedAuditableEntity<TPk>
    where TPk : struct
{
    private readonly string _altTableName;
    private readonly bool _prefixEntityNameToId;
    private readonly bool _prefixAltTblNameToEntity;
    private readonly string _preferredPKeyId;
    private readonly bool _pluralizeTblName;

    private ModelBuilder _modelBuilder;

    public string Schema { get; private set; }
    public bool Auditable { get; set; }
    public Expression<Func<TEntity, object>>[] FieldsToIgnore { get; set; }

    public virtual void OnModelCreating(EntityTypeBuilder<TEntity> entityBuilder)
    {
        SetupConventionalFields<TEntity>(entityBuilder, FieldsToIgnore, Auditable);
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
    protected EntityConfigurationAuditable(ModelBuilder builder,
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

    protected EntityConfigurationAuditable(
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
    protected EntityConfigurationAuditable(string pluralTableName = ""
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
    protected EntityConfigurationAuditable(string pluralTableName = ""
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
    private EntityConfigurationAuditable(string pluralTableName = ""
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
            tblName += typeof(TEntity).Name;

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

        builder
            .Property(i => i.IsActive)
            .HasDefaultValue(true)
            .IsRequired(false);

        OnModelCreating(builder);
        OnModelCreating(_modelBuilder);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entityBuilder"></param>
    /// <param name="conventionFieldsToIgnore"></param>
    /// <param name="auditable"></param>
    private static void SetupConventionalFields<T>(EntityTypeBuilder<TEntity> entityBuilder, Expression<Func<TEntity, object>>[] conventionFieldsToIgnore, bool auditable = false)
        where T : class, IKeyedAuditableEntity<TPk>
    {
        var fieldsToIgnore = new List<Expression<Func<TEntity, object>>>(conventionFieldsToIgnore ?? new List<Expression<Func<TEntity, object>>>().ToArray());

        if (!auditable)
        {
            fieldsToIgnore.Add(e => e.IsArchived);
            fieldsToIgnore.Add(e => e.ReasonArchived);
            fieldsToIgnore.Add(e => e.DateArchived);
        }

        foreach (var exprssn in fieldsToIgnore)
            entityBuilder.Ignore(exprssn);

        var fieldsIgnoreList = fieldsToIgnore.ToArray().ToListOfFields();

        // add the rest not ignored
        entityBuilder.RegisterConventionalField(fieldsIgnoreList, entity => entity.DateCreated)?
            .HasColumnName("_dateCreated").HasDefaultValue(DateTime.Now).IsRequired(true);

        entityBuilder.RegisterConventionalField(fieldsIgnoreList, entity => entity.DateModified)?
            .HasColumnName("_dateModified").IsRequired(false);

        entityBuilder.RegisterConventionalField(fieldsIgnoreList, entity => entity.IsArchived)?
            .HasColumnName("_isArchived").IsRequired(false);

        entityBuilder.RegisterConventionalField(fieldsIgnoreList, entity => entity.DateArchived)?
            .HasColumnName("_dateArchived").IsRequired(false);

        entityBuilder.RegisterConventionalField(fieldsIgnoreList, entity => entity.ReasonArchived)?
            .HasColumnName("_reasonArchived").IsRequired(false);

        // UsePropertyAccessMode(PropertyAccessMode.Property) :: https://stackoverflow.com/a/50776738/3796898
        // fix to error related to "Expression of type 'System.Nullable`1[System.Boolean]' cannot be used for assignment to type 'System.Boolean'"
        /*entityBuilder.RegisterConventionalField(fieldsIgnoreList, entity => entity.IsNotActive)?
            .HasColumnName("_isNotActive").HasDefaultValue(false).IsRequired(false)
            .UsePropertyAccessMode(PropertyAccessMode.Property);

        entityBuilder.RegisterConventionalField(fieldsIgnoreList, entity => entity.DateCreated)?
            .HasColumnName("_dateCreated").HasDefaultValue(DateTime.Now).IsRequired(true);

        entityBuilder.RegisterConventionalField(fieldsIgnoreList, entity => entity.DateRevised)?
            .HasColumnName("_dateRevised").IsRequired(false);

        entityBuilder.RegisterConventionalField(fieldsIgnoreList, entity => entity.CreatedBy)?
            .HasColumnName("_createdBy").IsRequired(false);

        entityBuilder.RegisterConventionalField(fieldsIgnoreList, entity => entity.RevisedBy)?
            .HasColumnName("_revisedBy").IsRequired(false);

        
        entityBuilder.RegisterConventionalField(fieldsIgnoreList, entity => entity.ArchivedBy)?
            .HasColumnName("_archivedBy").IsRequired(false);
        
        */
    }

    //private static void SetupConventionalFields_BAK<T>(EntityTypeBuilder<TEntity> entityBuilder, Expression<Func<TEntity, object>>[] conventionFieldsToIgnore, bool archivable = false)
    //    where T : class, IKeyedEntity<TPk>
    //{
    //    var fieldsToIgnore = new List<Expression<Func<TEntity, object>>>(conventionFieldsToIgnore ?? new List<Expression<Func<TEntity, object>>>().ToArray());

    //    if (!archivable)
    //    {
    //        fieldsToIgnore.Add(e => e.IsArchived);
    //        fieldsToIgnore.Add(e => e.ReasonArchived);
    //        fieldsToIgnore.Add(e => e.DateArchived);
    //    }

    //    foreach (var exprssn in fieldsToIgnore)
    //        entityBuilder.Ignore(exprssn);

    //    var fieldsIgnoreList = fieldsToIgnore.ToArray().ToListOfFields();

    //    // add the rest not ignored
    //    //entityBuilder.RegisterConventionalField(fieldsIgnoreList, entity => entity.Ordinal)?
    //    //    .HasColumnName("_ordinal").HasDefaultValue(0).IsRequired(false);

    //    // UsePropertyAccessMode(PropertyAccessMode.Property) :: https://stackoverflow.com/a/50776738/3796898
    //    // fix to error related to "Expression of type 'System.Nullable`1[System.Boolean]' cannot be used for assignment to type 'System.Boolean'"
    //    /*entityBuilder.RegisterConventionalField(fieldsIgnoreList, entity => entity.IsNotActive)?
    //        .HasColumnName("_isNotActive").HasDefaultValue(false).IsRequired(false)
    //        .UsePropertyAccessMode(PropertyAccessMode.Property);

    //    entityBuilder.RegisterConventionalField(fieldsIgnoreList, entity => entity.DateCreated)?
    //        .HasColumnName("_dateCreated").HasDefaultValue(DateTime.Now).IsRequired(true);

    //    entityBuilder.RegisterConventionalField(fieldsIgnoreList, entity => entity.DateRevised)?
    //        .HasColumnName("_dateRevised").IsRequired(false);

    //    entityBuilder.RegisterConventionalField(fieldsIgnoreList, entity => entity.CreatedBy)?
    //        .HasColumnName("_createdBy").IsRequired(false);

    //    entityBuilder.RegisterConventionalField(fieldsIgnoreList, entity => entity.RevisedBy)?
    //        .HasColumnName("_revisedBy").IsRequired(false);

    //    entityBuilder.RegisterConventionalField(fieldsIgnoreList, entity => entity.IsArchived)?
    //        .HasColumnName("_isArchived").IsRequired(false);
    //    entityBuilder.RegisterConventionalField(fieldsIgnoreList, entity => entity.ArchivedBy)?
    //        .HasColumnName("_archivedBy").IsRequired(false);
    //    entityBuilder.RegisterConventionalField(fieldsIgnoreList, entity => entity.ReasonArchived)?
    //        .HasColumnName("_reasonArchived").IsRequired(false);
    //    entityBuilder.RegisterConventionalField(fieldsIgnoreList, entity => entity.DateArchived)?
    //        .HasColumnName("_dateArchived").IsRequired(false);*/
    //}
}