using GK.Template.Attributes;
using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;

namespace GK.Template.Methods
{
    [Category("Hash")]
    [FormatMethod(Name = "HashSha512",
        Aliasses = "Sha512",
        Example = "{0:Sha512()}")]
    [Description("applies: Sha512 Hashing and a Base64.")]
    public class MethodHash512 : MethodBase
    {
        public override string Apply(string value)
        {
            return HashSha512(value);
        }

        private string HashSha512(string input)
        {
            SHA512Managed sHA512Managed = new SHA512Managed();
            byte[] inArray = new byte[input.Length];
            inArray = sHA512Managed.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(inArray);
        }
    }
}