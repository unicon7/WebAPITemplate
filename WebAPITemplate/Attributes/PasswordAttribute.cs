using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
//unicon7: https://github.com/microsoft/referencesource/blob/master/System.ComponentModel.DataAnnotations/DataAnnotations/CompareAttribute.cs 참고
namespace WebAPITemplate.Attributes
{
    public class AuthPasswordAttribute : ValidationAttribute
    {

        public AuthPasswordAttribute(string otherProperty)
        {
            if (otherProperty == null)
            {
                throw new ArgumentNullException("otherProperty");
            }

            OtherProperty = otherProperty;
        }

        public string OtherProperty { get; private set; }
        public string OtherPropertyDisplayName { get; internal set; }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, OtherPropertyDisplayName ?? OtherProperty);
        }


        public override bool RequiresValidationContext
        {
            get
            {
                return true;
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
            if (otherPropertyInfo == null)
            {
                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "Unknown Property", OtherProperty));
            }

            object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
           
            if (!ValidatePassword(value, otherPropertyValue))
            {
                if (OtherPropertyDisplayName == null)
                {
                    OtherPropertyDisplayName = GetDisplayNameForProperty(validationContext.ObjectType, OtherProperty);
                }
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }

        private bool ValidatePassword(object propertyValue, object otherPropertyValue)
        {
            if (Equals(propertyValue, otherPropertyValue))
            {
                return false;
            }

            return true;
        }

        private static string GetDisplayNameForProperty(Type containerType, string propertyName)
        {
            ICustomTypeDescriptor typeDescriptor = GetTypeDescriptor(containerType);
            PropertyDescriptor property = typeDescriptor.GetProperties().Find(propertyName, true);
            if (property == null)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture,
                    "Property not found", containerType.FullName, propertyName));
            }
            IEnumerable<Attribute> attributes = property.Attributes.Cast<Attribute>();
            DisplayAttribute display = attributes.OfType<DisplayAttribute>().FirstOrDefault();
            if (display != null)
            {
                return display.GetName();
            }
            DisplayNameAttribute displayName = attributes.OfType<DisplayNameAttribute>().FirstOrDefault();
            if (displayName != null)
            {
                return displayName.DisplayName;
            }
            return propertyName;
        }

        private static ICustomTypeDescriptor GetTypeDescriptor(Type type)
        {
            return new AssociatedMetadataTypeTypeDescriptionProvider(type).GetTypeDescriptor(type);
        }
    }
}