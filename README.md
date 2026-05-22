# Backend_ChatBot

Backend API desarrollada en **ASP.NET 10 + Entity Framework Core + MySQL** para un chatbot contextual basado en IA.

El sistema permite:

- Gestionar conversaciones y mensajes.
- Subir archivos PDF/TXT.
- Extraer texto automáticamente desde los documentos.
- Almacenar archivos en MySQL.
- Utilizar Gemini API para responder preguntas usando únicamente el contexto almacenado.
- Exponer endpoints REST

---

# Tecnologías Utilizadas

- ASP.NET 10
- Entity Framework Core
- MySQL
- AutoMapper
- Railway
- Gemini API
- PdfPig
- Scalar/OpenAPI (solo desarrollo)

# Funcionalidades

## Conversaciones

- Crear conversaciones
- Obtener conversaciones
- Eliminar mensajes de una conversación

---

## Chatbot IA

- Guarda preguntas del usuario
- Utiliza el contenido de los resumes como contexto
- Responde únicamente usando información contextual
- Realiza análisis básicos:
  - fortalezas técnicas
  - posibles roles
  - compatibilidad laboral
  - análisis de skills

---

## Upload de Archivos

- Soporta:
  - PDF
  - TXT
- Extrae texto automáticamente
- Guarda:
  - metadata
  - archivo binario
  - texto extraído

---

# Variables de Entorno

## appsettings.Development.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=chatbot;user=root;password=1234;"
  },

  "GEMINI_API_KEY": "TU_API_KEY",

  "APP_URL": "https://..."
}
```

---

# IMPORTANTE

La variable:

```json
"APP_URL"
```

es necesaria para que OpenAPI/Scalar genere correctamente las URLs HTTPS en producción y evitar problemas relacionados con:

- CORS
- HTTP/HTTPS mismatch
- Scalar/OpenAPI requests incorrectas

---

# Instalación

## 1. Clonar repositorio

```bash
git clone <repo-url>
```

---

## 2. Instalar dependencias

```bash
dotnet restore
```

---

## 3. Ejecutar migraciones

```bash
dotnet ef database update
```

---

## 4. Ejecutar proyecto

```bash
dotnet run
```

---

# Docker

## Dockerfile incluido

El proyecto está preparado para deployment usando Docker y Railway.

---

# Deployment Railway

## Variables requeridas

```text
ConnectionStrings__DefaultConnection
GEMINI_API_KEY
APP_URL
ASPNETCORE_ENVIRONMENT
ASPNETCORE_URLS
```

---

## ASPNETCORE_URLS

```text
http://+:${PORT}
```

---

# Scalar / OpenAPI

Scalar se encuentra habilitado únicamente en ambiente de desarrollo:

```csharp
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options.WithOpenApiRoutePattern("/openapi/v1.json");
    });
}
```

---

# Endpoints Principales

## Chatbot

```http
POST /api/chatbot
```

---

## Conversations

```http
GET    /api/conversation
POST   /api/conversation
DELETE /api/conversation/{id}/messages
```

---

## Resume Upload

```http
POST /api/resume
```

Content-Type:

```text
multipart/form-data
```

---

# Extracción de Texto

Los PDFs son procesados automáticamente usando:

```text
UglyToad.PdfPig
```

El texto extraído se almacena en:

```text
Resume.ExtractedText
```

para evitar reprocesar documentos constantemente.

---

# Notas

- El proyecto está orientado como MVP/portfolio project.
- Actualmente soporta un único contexto principal de usuario.
---
