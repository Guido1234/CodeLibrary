using GK.Template.Attributes;
using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;

namespace GK.Template.Methods
{
    [Category("Hash")]
    [FormatMethod(Name = "HashSha256",
        Aliasses = "Sha256",
        Example = "{0:Sha256()}")]
    [Description("applies: Sha256 Hashing and a Base64.")]
    public class MethodHash256 : MethodBase
    {
        public override string Apply(string value)
        {
            return HashSha256(value);
        }

        private string HashSha256(string input)
        {
            SHA256Managed shaManaged = new SHA256Managed();
            byte[] inArray = new byte[input.Length];
            inArray = shaManaged.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(inArray);
        }
    }
}