# BtgFundsApi

API REST en .NET 8 para la gestión de fondos de inversión, autenticación con JWT y MongoDB Atlas, desplegada en AWS Elastic Beanstalk mediante Docker y administrada con AWS CloudFormation.

---

## 🚀 Tecnologías utilizadas

- **.NET 8 Web API**
- **MongoDB Atlas**
- **JWT Authentication**
- **Docker**
- **AWS Elastic Beanstalk**
- **AWS CloudFormation**
- **Swagger**

---

## 📦 Estructura del proyecto

- `Controllers/`: Endpoints organizados por entidad (`UsersController`, `FundsController`, etc).
- `Services/`: Lógica de negocio desacoplada.
- `Configurations/`: Configuración de MongoDB y JWT.
- `Program.cs`: Configuración de servicios, Swagger y middleware.
- `Dockerfile`: Imagen de despliegue.
- `btgfundsapi-stack.yaml`: Plantilla de infraestructura en AWS CloudFormation.
- `Dockerrun.aws.json`: Archivo de despliegue en Elastic Beanstalk.

---

## 🛠️ Configuración local

1️⃣ **Requisitos:**
- .NET 8 SDK
- Docker
- MongoDB Atlas URI
- JWT Secret

2️⃣ **Configura `appsettings.json`:**

```json
{
  "MongoDB": {
    "ConnectionString": "mongodb+srv://<usuario>:<password>@cluster0.hmlgof9.mongodb.net/btg_funds_db",
    "DatabaseName": "btg_funds_db"
  },
  "Jwt": {
    "Key": "<tu_clave_jwt>",
    "Issuer": "BtgFundsApi",
    "Audience": "BtgFundsApiClient"
  }
}
