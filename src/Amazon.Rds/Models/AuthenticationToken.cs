﻿using System;

namespace Amazon.Rds;

public sealed class AuthenticationToken
{
    public AuthenticationToken(
        string value!!,
        DateTime issued,
        DateTime expires)
    {
        Value = value;
        Issued = issued;
        Expires = expires;
    }

    public string Value { get; }

    public DateTime Issued { get; }

    public DateTime Expires { get; }
}