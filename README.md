# SharpLogShield

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![issues - sharplogshield](https://img.shields.io/github/issues/engineering87/SharpLogShield)](https://github.com/engineering87/SharpLogShield/issues)
[![stars - sharplogshield](https://img.shields.io/github/stars/engineering87/SharpLogShield?style=social)](https://github.com/engineering87/SharpLogShield)

**SharpLogShield** is a simple and extensible logging wrapper for .NET that automatically masks sensitive data such as email addresses, credit cards, phone numbers, and fiscal codes in application logs. It integrates seamlessly with Microsoft.Extensions.Logging.

### How to use it
To integrate SharpLogShield into your .NET application, simply register the logger using the provided extension method.

```csharp
// Add Logging
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddConsole();
    // Add SharpLogShield
    loggingBuilder.AddSharpLogShieldLogging();
});
```

## Supported Data Masking Patterns

SharpLogShield currently applies automatic masking to several types of sensitive data commonly used in applications, with a focus on Italian data formats.

The following patterns are detected and partially masked to protect sensitive information while preserving some readability:

| Data Type        | Description                                                                 | Example (Original)                   | Example (Masked)              |
|------------------|-----------------------------------------------------------------------------|--------------------------------------|-------------------------------|
| **Email**        | Standard email addresses                                                    | `mario.rossi@example.com`            | `m***@example.com`            |
| **Credit Card**  | 13–16 digit credit card numbers                                             | `4111 1111 1111 1234`                | `4111 **** **** 1234`         |
| **Fiscal Code**  | Italian Codice Fiscale (16-character tax code)                              | `RSSMRA85M01H501U`                   | `RSS***85M**H***U`            |
| **Phone Number** | Italian-style phone numbers, with or without country code                   | `+39 333-123-4567`                   | `+39 ***-***-4567`            |
| **Address**      | Italian address formats starting with common street types                   | `Via Roma, Milano`                   | `Via ****, Milano`            |

### Localization

> 🟡 **Note:** Masking logic is currently optimized for Italian patterns (e.g., Codice Fiscale, address formats like `Via`, `Viale`, etc.).  
> Future updates may introduce support for additional locales and data formats (e.g., US SSN, IBAN, etc.).

All masking is done automatically through regular expressions and applied at logging level using a decorator around the default .NET `ILogger`.

## Contributing
Thank you for considering to help out with the source code!
If you'd like to contribute, please fork, fix, commit and send a pull request for the maintainers to review and merge into the main code base.

 * [Setting up Git](https://docs.github.com/en/get-started/getting-started-with-git/set-up-git)
 * [Fork the repository](https://docs.github.com/en/pull-requests/collaborating-with-pull-requests/working-with-forks/fork-a-repo)
 * [Open an issue](https://github.com/engineering87/SharpLogShield/issues) if you encounter a bug or have a suggestion for improvements/features

### Licensee
SharpLogShield source code is available under MIT License, see license in the source.

### Contact
Please contact at francesco.delre[at]protonmail.com for any details.
