using System.ComponentModel.DataAnnotations;
using System.Net;

namespace WebApp.DTOs;

public class ApiErrorResponse
{
    [Required]
    [StringLength(128)]
    public string ErrorMessage { get; set; } = default!;

    public HttpStatusCode StatusCode { get; set; } = default!;
}

