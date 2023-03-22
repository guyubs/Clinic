using System.ComponentModel.DataAnnotations;

namespace ClinicWeb.Models;
public partial class Specialist
{
    [Key]
    public int Id { get; set; }

    public string? SpecialityName { get; set; }

    public string? Note { get; set; }

    public bool? Deleted { get; set; } = false;

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? CreateDateTime { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public virtual ICollection<DrName> DrNames { get; set; } = new List<DrName>();
}
