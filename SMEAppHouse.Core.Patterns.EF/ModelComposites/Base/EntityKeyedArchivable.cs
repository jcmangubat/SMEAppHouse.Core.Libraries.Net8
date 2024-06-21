using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SMEAppHouse.Core.Patterns.EF.ModelComposites.Interfaces;

namespace SMEAppHouse.Core.Patterns.EF.ModelComposites.Base;

public class EntityKeyedArchivable<TPk> : EntityKeyed<TPk>, IEntityKeyedArchivable<TPk>
    where TPk : struct
{
    private bool? _isArchived = false;
    private DateTime? _dateArchived = null;
    private string? _reasonArchived = string.Empty;

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(Order = 510)]
    public bool? IsArchived
    {
        get => _isArchived;
        set
        {
            if (value == null || value.Equals(_isArchived)) return;
            _isArchived = value.Value;
        }
    }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(Order = 511, TypeName = "DateTime2")]
    public DateTime? DateArchived
    {
        get => _dateArchived;
        set
        {
            if (value == null || value.Equals(_dateArchived)) return;
            _dateArchived = value.Value;
        }
    }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(Order = 511)]
    [DataType(DataType.Text)]
    [StringLength(132)]
    public string ReasonArchived
    {
        get => _reasonArchived;
        set
        {
            if (value.Equals(_reasonArchived)) return;
            _reasonArchived = value;
        }
    }

    public override string ToString() => $"{base.ToString()} archived: {_isArchived} dateArchived:{_dateArchived}";
}
