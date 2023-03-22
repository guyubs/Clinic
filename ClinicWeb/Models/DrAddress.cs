using System.ComponentModel.DataAnnotations;

namespace ClinicWeb.Models;
public partial class DrAddress
{
    [Key]
    public int Id { get; set; }

    public string? Street1 { get; set; }

    public string? Street2 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Zip { get; set; }

    public string? Tel { get; set; }

    public string? Fax { get; set; }

    public bool? Deleted { get; set; } = false;

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? CreateDateTime { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public virtual ICollection<DrName> DrNames { get; set; } = new List<DrName>();
}

