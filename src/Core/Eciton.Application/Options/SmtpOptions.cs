﻿namespace Eciton.Application.Options;
public class SmtpOptions
{
    public const string Position = "SmtpOptions";
    public string Host { get; set; }
    public int Port { get; set; }
    public string Sender { get; set; }
    public string Password { get; set; }
}
