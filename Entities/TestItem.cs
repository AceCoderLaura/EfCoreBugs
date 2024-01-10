using System.ComponentModel.DataAnnotations;

namespace EfCoreBugs.Entities;

public class TestItem
{
    [Key]
    public Guid Key { get; set; }
    
    [StringLength(120)]
    public string? TextOne { get; set; }
    
    [StringLength(120)]
    public string? TextTwo { get; set; }
}