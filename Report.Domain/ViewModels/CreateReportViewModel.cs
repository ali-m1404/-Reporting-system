using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public class CreateReportViewModel
{
    public int UserId { get; set; }
    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    [Required]
    [StringLength(1000)]
    public string Description { get; set; }

    [Required]
    public int ReportTypeId { get; set; }

    public IFormFile File { get; set; }

    // 👇 برای DropDown
    public List<SelectListItem> ReportTypes { get; set; } = new();
}
