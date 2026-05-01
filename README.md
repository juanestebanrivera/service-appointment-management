<h1 align="center">API de Gestión de citas de servicios</h1>

<div align="center">

![C#](https://img.shields.io/badge/-C%23-512bd4)
![.NET](https://img.shields.io/badge/-.NET_10-512BD4)
![Entity Framework](https://img.shields.io/badge/-Entity_Framework-512bd4)
![PostgreSQL](https://img.shields.io/badge/-PostgreSQL-4169E1?logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/-Docker-2496ED?logo=docker&logoColor=white)
![Clean Architecture](https://img.shields.io/badge/-Clean_Architecture-2C3E50)
![CQRS](https://img.shields.io/badge/-CQRS-2C3E50)
![DDD](https://img.shields.io/badge/-DDD-2C3E50)

</div>

<div align="center">
    <a href="#funcionalidades-principales">Funcionalidades</a>・
    <a href="#tecnologías-y-arquitectura">Tecnologías y arquitectura</a>・
    <a href="#guía-de-ejecución">Guía de ejecución</a>・
    <a href="#próximos-pasos">Próximos pasos</a>
</div>

## Descripción

API RESTful diseñada para la gestión eficiente de citas de servicios. Permite a los clientes agendar y administrar sus citas, mientras que proporciona a los administradores la posibilidad de gestionar la oferta de servicios y realizar un seguimiento de las citas programadas.

Este proyecto fue construido con un enfoque fuerte en **buenas prácticas, escalabilidad y código limpio**, sirviendo como una demostración técnica de patrones de diseño empresariales modernos.

## Funcionalidades principales

- **Gestión de clientes:** Agendamiento, modificación, confirmación, cancelación y consulta de citas programadas.
- **Administración de servicios:** Creación, edición y eliminación del catálogo de servicios.
- **Seguridad:** Autenticación y autorización basada en roles (JWT) para proteger los endpoints sensibles.

## Tecnologías y arquitectura

La aplicación está desarrollada en **.NET 10**, estructurada bajo los principios de **Clean Architecture** y **Screaming Architecture** para garantizar que el propósito del sistema sea evidente con solo ver la estructura de carpetas.

- **Dominio:** Núcleo de la aplicación. Aplica **DDD (Domain-Driven Design)** para encapsular las reglas de negocio y asegurar la integridad de las entidades.
- **Aplicación:** Orquesta los casos de uso utilizando el patrón **CQRS** (separación de comandos y consultas) y el patrón **Unit of Work** para transacciones atómicas.
- **Infraestructura:** Manejo de persistencia de datos mediante **Entity Framework Core** y conexiones con servicios externos.
- **API (Presentación):** Implementada como _Minimal APIs_. Incluye características como:
  - **API Versioning:** Para permitir evolución sin romper retrocompatibilidad.
  - **Output Caching & Rate Limiting:** Optimización de rendimiento y protección contra excesos de solicitudes.
  - **Problem Details:** Estandarización de respuestas de error (RFC 7807).
  - **Logging:** Monitoreo y trazabilidad del sistema.
- **Tests:** Implementación de pruebas unitarias utilizando **xUnit** para garantizar la fiabilidad de las reglas de negocio y los casos de uso específicos en las capas de Dominio y Aplicación.

## Guía de ejecución

La forma más sencilla de probar la aplicación localmente es utilizando Docker. Todo el entorno (API y Base de Datos) está contenerizado.

### Requisitos previos

- Docker y Docker Compose instalados.
- Clonar el repositorio del proyecto.

### Pasos

1. En la carpeta raíz del proyecto, ejecute el siguiente comando para iniciar los servicios:

```bash
docker-compose up -d
```

2. Para interactuar con la API, abra el navegador y acceda a `http://localhost:8080`.

3. Para detener los servicios, ejecute:

```bash
docker-compose down
```

## Próximos pasos

- Implementación de filtros y paginación para las consultas de citas y servicios.
- Desarrollo del Frontend utilizando Angular.
