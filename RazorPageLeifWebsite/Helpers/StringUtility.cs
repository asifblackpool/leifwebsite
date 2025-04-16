using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Zengenti.Contensis.Delivery;


namespace RazorPageLeifDemoWebsite.Helpers;

//Add Enum File Extension
public enum FILE_Extension
{
    [Display(Name = "aspx page", Description = ".aspx")]
    ASPX = 1,
    [Display(Name = "html page", Description = ".html")]
    HTML = 2,
    [Display(Name = "jpg image", Description = ".jpg")]
    JPEG = 3
}

public static class EnumerationExtension
{
    public static string Description(this Enum value)
    {
        // get attributes  
        var field = value.GetType().GetField(value.ToString());
        var attributes = field.GetCustomAttributes(false);

        // Description is in a hidden Attribute class called DisplayAttribute
        // Not to be confused with DisplayNameAttribute
        dynamic displayAttribute = null;

        if (attributes.Any())
        {
            displayAttribute = attributes.ElementAt(0);
        }

        // return description
        return displayAttribute?.Description ?? "Description Not Found";
    }
}


public static class StringUtility
{

    public static string? RemoveFileExtension(this string path, FILE_Extension extensiontype)
    {
        string description = extensiontype.Description();
        string temp = (path.Contains(description)) ? path.Replace(description, string.Empty) : path;
        return temp.Trim().ToLower();
    }
}
