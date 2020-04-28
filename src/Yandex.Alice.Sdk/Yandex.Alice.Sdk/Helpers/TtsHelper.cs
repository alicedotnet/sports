using System;
using System.Collections.Generic;
using System.Text;

namespace Yandex.Alice.Sdk.Helpers
{
    public static class TtsHelper
    {
        public static string GetSilenceString(long milliseconds)
        {
            return $"sil <[{milliseconds}]>";
        }
    }
}
