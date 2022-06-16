using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace aspnetcore_example.Pages;

[IgnoreAntiforgeryToken]
public class SuccessModel : PageModel
{
    private readonly ILogger<SuccessModel> _logger;
    public string Message
    {
        get;
        set;
    }

    public SuccessModel(ILogger<SuccessModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        Message = "This is a GET Request.";
    }

    public void OnPost()
    {
        Message = "This is a POST Request.";
    }

}

