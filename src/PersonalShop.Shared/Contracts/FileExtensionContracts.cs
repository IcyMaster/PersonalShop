using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalShop.Shared.Contracts;

public static class FileExtensionContracts
{
    public const string Jpg = ".jpg";
    public const string Jpge = ".jpge";
    public const string Png = ".png";

    public static List<string> SafeExtensions
    {
        get
        {
            return new List<string> {
                Jpg, Jpge, Png
            };
        }
    }
}
