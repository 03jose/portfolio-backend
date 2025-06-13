# Portafolio Backend - José Ricardo Martínez

Minimal API con .NET 8 para manejar el formulario de contacto.

## Endpoints

- `POST /api/contact`: Recibe { name, email, message } y envía un correo vía SendGrid.

## Variables de entorno

- `SENDGRID_API_KEY`
- `SENDER_EMAIL`
- `RECEIVER_EMAIL`

## Ejecutar localmente

```bash
dotnet run
