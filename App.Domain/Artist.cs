using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Artist : BaseEntity
{
    [MaxLength(64)] public string StageName { get; set; } = default!;
    
    public DateTime BirthDate { get; set; }
    
    public bool IsSolo { get; set; }
    
    
}