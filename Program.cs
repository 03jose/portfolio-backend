using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/api/contact", async (HttpContext http, IConfiguration config) =>
{
    var form = await http.Request.ReadFromJsonAsync<ContactForm>();
    if (form == null || string.IsNullOrEmpty(form.Name) || string.IsNullOrEmpty(form.Email) || string.IsNullOrEmpty(form.Message))
        return Results.BadRequest("Datos incompletos");

    var apiKey = config["SENDGRID_API_KEY"];
    var fromEmail = config["SENDER_EMAIL"];
    var toEmail = config["RECEIVER_EMAIL"];

    try
    {
        Console.WriteLine($"[SendGrid] API Key loaded: {apiKey?.Substring(0, 5)}...");
        Console.WriteLine($"[SendGrid] From: {fromEmail}, To: {toEmail}");

        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(fromEmail, "Portafolio Web");
        var to = new EmailAddress(toEmail);
        var subject = $"Mensaje de {form.Name}";
        var plainTextContent = $"Correo: {form.Email}\n\n{form.Message}";
        var htmlContent = $"<p><strong>Correo:</strong> {form.Email}</p><p>{form.Message}</p>";

        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        var response = await client.SendEmailAsync(msg);

        Console.WriteLine($"[SendGrid] Status Code: {response.StatusCode}");
        var body = await response.Body.ReadAsStringAsync();
        Console.WriteLine($"[SendGrid] Body: {body}");

        return response.IsSuccessStatusCode ? Results.Ok("Enviado") : Results.StatusCode(500);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[SendGrid ERROR] {ex.Message}");
        return Results.StatusCode(500);
    }
});

app.Run();

record ContactForm(string Name, string Email, string Message);
