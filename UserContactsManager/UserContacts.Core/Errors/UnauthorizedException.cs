﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UserContacts.Core.Errors;

public class UnauthorizedException : BaseException
{
    public UnauthorizedException() { }
    public UnauthorizedException(String message) : base(message) { }
    public UnauthorizedException(String message, Exception inner) : base(message, inner) { }
    protected UnauthorizedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}