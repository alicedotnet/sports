using System;
using System.Collections.Generic;
using System.Text;
using Yandex.Alice.Sdk.Models;

namespace Yandex.Alice.Sdk.Helpers
{
    public static class AliceHelper
    {
        public const string DefaultReducedStringEnding = "...";

        public static string PrepareGalleryCardItemTitle(string title, string ending = DefaultReducedStringEnding)
        {
            return ReduceString(title, AliceGalleryCardItem.MaxTitleLength, ending);
        }

        public static string ReduceString(string value, int maxLenght, string ending)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (value.Length <= maxLenght)
            {
                return value;
            }
            if (ending == null)
            {
                throw new ArgumentNullException(nameof(ending));
            }
            int maxReducedStringLength = maxLenght - ending.Length;
            string reducedString = value.Substring(0, maxReducedStringLength);
            int lastWhitespaceIndex = reducedString.LastIndexOf(" ", StringComparison.OrdinalIgnoreCase);
            reducedString = reducedString.Substring(0, lastWhitespaceIndex);
            return reducedString.TrimEnd('.', ',', '-', ':') + ending;
        }
    }
}
