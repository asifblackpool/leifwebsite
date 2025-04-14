using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zengenti.Contensis.Delivery;

using RazorPageLeifDemoWebsite.Helpers;
using RazorPageLeifDemoWebsite.Models;

namespace RazorPageLeifDemoWebsite.Pages;

public class BlogModel : PageModel
{
    private readonly ILogger<BlogModel> _logger;
    public Guid _entryId { get; set; }

    public BlogModel(ILogger<BlogModel> logger)
    {
        _logger = logger;
    }

    // Set the model
    public BlogPost? BlogPostModel { get; set; }


    public IActionResult OnGet()
    {
        // Connect to the Contensis delivery API
        // Connection details set in /Program.cs
        var client = ContensisClient.Create();
        string entryVersionStatus = HttpContext.Request.Headers.TryGetValue("x-entry-versionstatus", out var values) ? values.FirstOrDefault() ?? "published" : "published";

        client.DefaultVersionStatus = Enum.Parse<VersionStatus>(entryVersionStatus, true);

        string? path = HttpContext.Request.Path;
        path = (path == null) ? string.Empty : path.RemoveFileExtension(FILE_Extension.ASPX);

        var node = client.Nodes.GetByPath(path);
        if (node != null)
        {
            BlogPostModel = node.Entry<BlogPost>();
        }


        // return a 404 if BlogId is invalid
        if (BlogPostModel == null)
        {
            return NotFound();
        }



        // Set the page title to the blog title
        ViewData["Title"] = BlogPostModel.Title;

        return Page();
    }

    public IActionResult OnGetX(Guid entryId)
    {
        // Connect to the Contensis delivery API
        // Connection details set in /Program.cs
        var client = ContensisClient.Create();
        string entryVersionStatus = HttpContext.Request.Headers.TryGetValue("x-entry-versionstatus", out var values) ? values.FirstOrDefault() ?? "published" : "published";

        client.DefaultVersionStatus = Enum.Parse<VersionStatus>(entryVersionStatus, true);

    

        var node = client.Nodes.GetByPath(HttpContext.Request.Path);
        if (node != null)
        {
            BlogPostModel = node.Entry<BlogPost>();
        }



        /* entryId way of getting blog post
        
        var _entryId = entryId; //HttpContext.Request.Query["entryId"];
        if (!string.IsNullOrEmpty(_entryId.ToString()))
        {
            // Get the entries by the id
            BlogPostModel = client.Entries.Get<BlogPost>(entryId);
        }
        */

        // return a 404 if BlogId is invalid
        if (BlogPostModel == null)
        {
            return NotFound();
        }

  

        // Set the page title to the blog title
        ViewData["Title"] = BlogPostModel.Title;

        return Page();
    }
}
