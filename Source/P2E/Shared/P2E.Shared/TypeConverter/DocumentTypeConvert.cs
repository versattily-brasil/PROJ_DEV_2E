using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using P2E.Shared.ValuesObject;
using System.Linq;

namespace P2E.Shared.TypeConverter
{
    class DocumentTypeConvert : System.ComponentModel.TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }
        
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string stringValue;
            object result;

            result = null;
            stringValue = value as string;

            if (!string.IsNullOrEmpty(stringValue))
            {
                result = new Document(stringValue);
                //int nonDigitIndex;

                //nonDigitIndex = stringValue.IndexOf(stringValue.FirstOrDefault(char.IsLetter));

                //if (nonDigitIndex > 0)
                //{
                //    result = new Document(stringValue);
                //}


            }

            return result ?? base.ConvertFrom(context, culture, value);
        }
    }
}
