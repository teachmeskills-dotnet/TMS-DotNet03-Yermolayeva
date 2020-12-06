using System;
using System.Collections.Generic;
using System.Text;

namespace HandiworkShop.Common.Constants
{
    /// <summary>
    /// Configuration constants.
    /// </summary>
    public static class ConfigurationConstants
    {
        public const string DateFormat = "date";
        public const string DecimalFormat = "decimal";
        public const string AvatarFormat = "varbinary(max)";
        //public const string TextFormat = "nvarchar(max)";

        /// <summary>
        /// Short length for a string field.
        /// </summary>
        public const int ShortLenghtForStringField = 64;

        /// <summary>
        /// Standart length a for string field.
        /// </summary>
        public const int StandartLenghtForStringField = 128;

        /// <summary>
        /// Max length for a string field.
        /// </summary>
        public const int LongLenghtForStringField = 256;

    }
}
