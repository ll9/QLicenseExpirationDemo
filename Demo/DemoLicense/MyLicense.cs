using QLicense;
using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace DemoLicense
{
    public class MyLicense : QLicense.LicenseEntity
    {
        [DisplayName("Enable Feature 01")]
        [Category("License Options")]        
        [XmlElement("EnableFeature01")]
        [ShowInLicenseInfo(true, "Enable Feature 01", ShowInLicenseInfoAttribute.FormatType.String)]
        public bool EnableFeature01 { get; set; }

        [DisplayName("Enable Feature 02")]
        [Category("License Options")]        
        [XmlElement("EnableFeature02")]
        [ShowInLicenseInfo(true, "Enable Feature 02", ShowInLicenseInfoAttribute.FormatType.String)]
        public bool EnableFeature02 { get; set; }


        [DisplayName("Enable Feature 03")]
        [Category("License Options")]        
        [XmlElement("EnableFeature03")]
        [ShowInLicenseInfo(true, "Enable Feature 03", ShowInLicenseInfoAttribute.FormatType.String)]
        public bool EnableFeature03 { get; set; }

        [DisplayName("Expiration Date")]
        [Category("License Options")]
        [XmlElement("Expiration Date")]
        [ShowInLicenseInfo(true, "Expiration Date", ShowInLicenseInfoAttribute.FormatType.DateTime)]
        public DateTime ExpirationDate { get; set; }

        public MyLicense()
        {
            //Initialize app name for the license
            this.AppName = "DemoWinFormApp";
        }

        public override LicenseStatus DoExtraValidation(out string validationMsg)
        {
            LicenseStatus _licStatus = LicenseStatus.UNDEFINED;
            validationMsg = string.Empty;

            switch (this.Type)
            {
                case LicenseTypes.Single:
                    //For Single License, check whether UID is matched
                    string hash;
                    var firmenName = Microsoft.VisualBasic.Interaction.InputBox("Firmenname:", "Title", "ID (Firmenname");
                    // Create a SHA256   
                    using (SHA256 sha256Hash = SHA256.Create())
                    {
                        // ComputeHash - returns byte array  
                        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(firmenName));

                        // Convert byte array to a string   
                        StringBuilder builder = new StringBuilder();
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            builder.Append(bytes[i].ToString("x2"));
                        }
                        hash = builder.ToString();
                    }

                    if (hash != this.UID)
                    {
                        _licStatus = LicenseStatus.INVALID;
                    }
                    else
                    {
                        _licStatus = LicenseStatus.VALID;
                    }
                    break;
                case LicenseTypes.Volume:
                    //No UID checking for Volume License
                    _licStatus = LicenseStatus.VALID;
                    break;
                default:
                    validationMsg = "Invalid license";
                    _licStatus= LicenseStatus.INVALID;
                    break;
            }

            return _licStatus;
        }
    }
}
