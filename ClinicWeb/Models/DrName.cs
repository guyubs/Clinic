using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicWeb.Models;

public partial class DrName
{
    [Key]
    public int Id { get; set; }

    [DisplayName("Last Name")]
    public string? LastName { get; set; }

    [DisplayName("First Name")]
    public string? FirstName { get; set; }

    [ForeignKey("Specialist")]
    public int? SpecialityId { get; set; }

    [ForeignKey("DrAddress")]
    public int? DrAddrId { get; set; }

    [ForeignKey("Title")]
    public int? TitleId { get; set; }

    public string? Note { get; set; }

    public bool? Deleted { get; set; } = false; // 默认false

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? CreateDateTime { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public virtual DrAddress? DrAddr { get; set; }

    public virtual Specialist? Speciality { get; set; }

    public virtual Title? Title { get; set; }
}
