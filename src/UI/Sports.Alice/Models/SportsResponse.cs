﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Alice.Sdk.Models;

namespace Sports.Alice.Models
{
    public class SportsResponse : AliceResponse<SportsSessionState, object>
    {
        public SportsResponse(SportsRequest request, string text, bool keepSessionState = true, bool keepUserState = true)
            : base(request, text, keepSessionState, keepUserState)
        {
        }

        public SportsResponse(SportsRequest request, string text, string tts, bool keepSessionState = true, bool keepUserState = true) 
            : base(request, text, tts, keepSessionState, keepUserState)
        {
        }

        public SportsResponse(SportsRequest request, string text, List<AliceButtonModel> buttons, bool keepSessionState = true, bool keepUserState = true)
            : base(request, text, buttons, keepSessionState, keepUserState)
        {
        }

        public SportsResponse(SportsRequest request, string text, string tts, List<AliceButtonModel> buttons, bool keepSessionState = true, bool keepUserState = true) 
            : base(request, text, tts, buttons, keepSessionState, keepUserState)
        {
        }
    }
}
