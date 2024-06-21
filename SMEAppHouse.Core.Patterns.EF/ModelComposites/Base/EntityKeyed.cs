﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using SMEAppHouse.Core.Patterns.EF.ModelComposites.Interfaces;

namespace SMEAppHouse.Core.Patterns.EF.ModelComposites.Base;

/// <summary>
/// Why Datetime2? &gt; https://stackoverflow.com/a/1884088
/// </summary>
/// <typeparam name="TPk"></typeparam>
public class EntityKeyed<TPk> : IEntityKeyed<TPk>
    where TPk : struct
{
    private TPk _id = default;
    private bool? _IsActive = true;
    private DateTime _dateCreated = DateTime.UtcNow;
    private DateTime? _dateModified = DateTime.UtcNow;

    /// <summary>
    /// Primary key of this data model
    /// </summary>
    [Required]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(Order = 0)]
    public TPk Id
    {
        get => _id;
        set
        {
            if (value.Equals(_id)) return;
            _id = value;
        }
    }

    /// <summary>
    /// Used to indicate the model is active and can be used by the service operations.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(Order = 506)]
    public bool? IsActive
    {
        get => _IsActive;
        set
        {
            if (value == null || value.Equals(_IsActive)) return;
            _IsActive = value.Value;
        }
    }

    #region Debugging Aides

    /// <summary>
    /// Warns the developer if this object does not have
    /// a public property with the specified name. This
    /// method does not exist in a Release build.
    /// </summary>
    [Conditional("DEBUG")]
    [DebuggerStepThrough]
    public virtual void VerifyPropertyName(string propertyName)
    {
        // Verify that the property name matches a real,
        // public, instance property on this object.
        if (TypeDescriptor.GetProperties(this)[propertyName] != null) return;
        var msg = "Invalid property name: " + propertyName;

        if (this.ThrowOnInvalidPropertyName)
            throw new Exception(msg);
        else Debug.Fail(msg);
    }

    /// <summary>
    /// Date this record was created
    /// https://stackoverflow.com/a/1884088
    /// </summary>
    //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
    [Column(Order = 501, TypeName = "DateTime2")]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime DateCreated
    {
        get
        {
            return DateTime.SpecifyKind(_dateCreated, DateTimeKind.Utc);
        }
        set
        {
            if (value.Equals(_dateCreated)) return;
            _dateCreated = value.ToUniversalTime();
        }
    } 

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(Order = 508, TypeName = "DateTime2")]
    public DateTime? DateModified
    {
        get
        {
            if (_dateModified != null)
                return DateTime.SpecifyKind(_dateModified.Value, DateTimeKind.Utc);
            return null;
        }
        set
        {
            if (value.Equals(_dateModified)) return;
            if (value != null)
                _dateModified = value.Value.ToUniversalTime();
        }
    }

    /// <summary>
    /// Returns whether an exception is thrown, or if a Debug.Fail() is used
    /// when an invalid property name is passed to the VerifyPropertyName method.
    /// The default value is false, but subclasses used by unit tests might
    /// override this property's getter to return true.
    /// </summary>
    protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

    public static IEnumerable<Type> GetImplementors()
    {
        var type = typeof(IEntityKeyed<TPk>);
        var types = AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p));
        return types;
    }

    public Type GetEntityIdentificationType() => default(TPk).GetType();

    public override string ToString() => $"Id:{Id} Active:{_IsActive} created:{_dateCreated} revised:{_dateModified}";

    #endregion // Debugging Aides    
}
