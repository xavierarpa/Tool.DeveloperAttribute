using System;
public enum DeveloperMessage : byte
{
    None,
    [Obsolete("Aquí hay un Error, solo notifica")] Error,
    Info,
    Warning
}