# Project_Resume — Portfolio VS Code + Chatbot

Frontend de portfolio personal con interfaz inspirada en **Visual Studio Code** (tema **Solarized Dark**) y un **chatbot en terminal** conectado a una API .NET para responder preguntas sobre el currículum.

**Demo:** [https://RicardoL-2026.github.io/ChatBot-Frontend](https://RicardoL-2026.github.io/ChatBot-Frontend)

---

## Características

### Interfaz tipo VS Code

| Panel | Descripción |
|-------|-------------|
| **Explorer** | Árbol `Project_Resume/SRC/` con secciones del CV (`.tsx` / `.readme`) |
| **Search** | Busca texto en el contenido y abre el archivo correspondiente |
| **Source Control** | Tarjetas de proyectos GitHub (`github-projects.json`) |
| **Run and Debug** | Mini juego estilo dinosaurio offline |
| **Editor** | Pestañas, números de línea (laptop+) y vista de secciones |
| **Terminal** | Chatbot con estilo shell Linux en la parte inferior |

### Terminal / Chatbot

- Inicia la conversación única del portfolio vía API.
- Preguntas sobre el CV con **Enter** → respuesta en nueva línea.
- Comandos integrados (ver [Comandos de terminal](#comandos-de-terminal)).
- Indicador de carga animado mientras procesa.
- Clic en cualquier zona del panel enfoca el input.

### Contenido editable sin tocar componentes

Los textos del CV viven en `src/content/` como Markdown y JSON, cargados en build con `?raw`.

### Diseño responsive

Cuatro rangos en `src/styles/breakpoints.css`:

- **Mobile:** &lt; 640px  
- **Tablet:** 640px – 1023px  
- **Laptop:** 1024px – 1439px  
- **Monitor:** ≥ 1440px  

---

## Stack

- [React 19](https://react.dev/) + [TypeScript](https://www.typescriptlang.org/)
- [Vite 8](https://vite.dev/)
- CSS Modules + variables Solarized Dark
- `fetch` para la API del chatbot

---

## Inicio rápido

### Requisitos

- Node.js 18+
- API del chatbot en ejecución (por defecto `https://localhost:7195`)

### Instalación

```bash
npm install
```

### Variables de entorno

Crea un archivo `.env` en la raíz:

```env
VITE_CHATBOT_API_URL=https://localhost:7195/
```

> La URL debe terminar en `/`. Vite expone solo variables con prefijo `VITE_`.

### Desarrollo

```bash
npm run dev
```

Abre la URL que muestra Vite (normalmente `http://localhost:5173`).

### Producción

```bash
npm run build
npm run preview
```

El build se genera en la carpeta `docs/` (configurado en `vite.config.ts` para GitHub Pages).

### Despliegue (GitHub Pages)

```bash
npm run build
npm run deploy
```

`base` en Vite: `/ChatBot-Frontend/`. Asegúrate de que el script `deploy` apunte al mismo directorio de salida (`docs/`) si usas `gh-pages`.

---

## Estructura del proyecto

```
src/
├── content/                 # Textos del portfolio (Markdown + JSON)
├── portfolio/
│   ├── components/          # UI VS Code (Explorer, tabs, juego, etc.)
│   ├── sections/            # Secciones TSX del CV
│   ├── icons/               # Iconos SVG
│   └── utils/               # Markdown, búsqueda
├── chatbot/
│   ├── api.ts               # Llamadas a la API
│   ├── apiClient.ts         # Cliente HTTP genérico
│   ├── conversationSession.ts # ID de conversación en sesión
│   ├── hooks/useChatbot.ts  # Lógica de la terminal
│   └── TerminalChatbot.tsx  # UI de la terminal
└── styles/                  # Solarized + breakpoints
```

---

## Editar el contenido del portfolio

| Archivo | Sección en Explorer |
|---------|---------------------|
| `src/content/education.md` | `Education.tsx` |
| `src/content/experience.md` | `Experience.tsx` |
| `src/content/hardSkills.md` | `HardSkills.tsx` |
| `src/content/softSkills.md` | `SoftSkills.tsx` |
| `src/content/aboutMe.md` | `AboutMe.readme` |
| `src/content/hobbies.md` | `Hobbies.tsx` |
| `src/content/github-projects.json` | Panel Source Control |

Más detalles en [`src/content/README.md`](src/content/README.md).

---

## API del chatbot

Base: `VITE_CHATBOT_API_URL`

| Método | Endpoint | Uso |
|--------|----------|-----|
| `GET` | `/api/Conversation` | Obtiene la conversación del portfolio; guarda su `id` en sesión |
| `POST` | `/api/chatbot/ask` | Nueva pregunta |
| `POST` | `/api/chatbot/ask-replay` | Re-ejecutar mensaje (`!ID`) |
| `GET` | `/api/Message` | Historial de mensajes |
| `DELETE` | `/api/Message/{conversationId}/messages` | Borrar historial (`Clear-History`) |

**Body de pregunta:**

```json
{
  "messa": "¿Dónde estudió?",
  "type": "Request",
  "conversacionID": "<id de la conversación activa>"
}
```

El `conversacionID` se toma automáticamente tras `initConversation()` (no está hardcodeado en el cliente).

---

## Comandos de terminal

| Comando | Acción |
|---------|--------|
| `history` | Lista mensajes con su ID |
| `Clear-History` | Elimina todos los mensajes de la conversación |
| `!16` | Re-envía solo el mensaje con ID `16` (vía `ask-replay`) |

Cualquier otro texto se envía como pregunta al chatbot.

---

## Scripts npm

| Script | Descripción |
|--------|-------------|
| `npm run dev` | Servidor de desarrollo |
| `npm run build` | Compila TypeScript y genera build en `docs/` |
| `npm run preview` | Previsualiza el build |
| `npm run lint` | ESLint |
| `npm run deploy` | Publica en GitHub Pages (`gh-pages`) |

---

## Licencia

Proyecto privado — uso personal / portfolio.
