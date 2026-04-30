# MiniCourt
[cite_start]Here is the consolidated technical specification for the **MiniCourt** project[cite: 103]. [cite_start]You can save this content as **MiniCourt-Spec.md** to keep as your development roadmap[cite: 104].

# MiniCourt: Backend Technical Specification

## 1. Project Overview
[cite_start]**MiniCourt** is a distributed sports platform designed to manage user profiles, check court availability, and securely book events[cite: 105]. [cite_start]The system enforces strict business rules, such as preventing double-bookings, and utilizes background workers for gamification and automated communications[cite: 106].

---

## 2. Identity & Security Module
[cite_start]This module manages onboarding and ensures only authenticated users can access booking features[cite: 107].

| API Call | Method | Endpoint | Description |
| :--- | :--- | :--- | :--- |
| **Register** | `POST` | `/api/auth/register` | [cite_start]Creates a new user[cite: 109]. [cite_start]Passwords **MUST** be hashed using **HMACSHA512** with a unique **Salt**[cite: 110]. |
| **Login** | `POST` | `/api/auth/login` | [cite_start]Validates credentials and returns a **JWT (JSON Web Token)**[cite: 111]. |
| **Get Profile** | `GET` | `/api/users/me` | [cite_start]Returns current user details[cite: 112]. [cite_start]**DTOs** must be used to ensure `PasswordHash` and `PasswordSalt` are never exposed[cite: 112]. |

---

## 3. Court Management Module
[cite_start]Allows users to browse facilities and check real-time availability[cite: 114].

| API Call | Method | Endpoint | Description |
| :--- | :--- | :--- | :--- |
| **List Courts** | `GET` | `/api/courts` | [cite_start]Returns all available courts (Tennis, Basketball, etc.) with hourly rates[cite: 116]. |
| **Court Details** | `GET` | `/api/courts/{id}` | [cite_start]Returns specific details like location and surface type[cite: 117]. |
| **Availability** | `GET` | `/api/courts/{id}/slots` | [cite_start]Takes a `date` parameter and returns free time slots[cite: 118]. |

---

## 4. The Booking Engine (Core Feature)
[cite_start]The heart of the application, utilizing **MediatR** to separate **Commands** (writes) from **Queries** (reads)[cite: 119].

| API Call | Method | Endpoint | Description |
| :--- | :--- | :--- | :--- |
| **Book Court** | `POST` | `/api/bookings` | [cite_start]**Command:** Validates business rules in the **Domain** layer and commits to the database via **EF Core**[cite: 121]. |
| **My Bookings** | `GET` | `/api/bookings/me` | [cite_start]**Query:** Lists all upcoming and past reservations for the authorized user[cite: 122]. |
| **Cancel Booking** | `DELETE` | `/api/bookings/{id}` | [cite_start]Removes the reservation and frees the time slot[cite: 123]. |

---

## 5. Gamification & Background Tasks
[cite_start]MiniCourt uses an **Event-Driven Architecture** to process non-urgent tasks without slowing down the user experience[cite: 124].

| API Call | Method | Endpoint | Description |
| :--- | :--- | :--- | :--- |
| **User Stats** | `GET` | `/api/gamification/points` | [cite_start]Returns total loyalty points earned through successful bookings[cite: 126]. |

* [cite_start]**Pub/Sub Logic:** Upon a successful booking, the API publishes a message to **Azure Service Bus**[cite: 127].
* [cite_start]**Background Worker:** An **Azure Function (QueueTrigger)** processes the message to calculate points and send confirmation emails[cite: 128].

---

## 6. Backend Execution Flow
[cite_start]Following **Clean Architecture** principles, every request (like "Book Court") must travel through these layers[cite: 129]:

1.  [cite_start]**Presentation (Web API):** Validates JSON and routes the command to **MediatR**[cite: 129].
2.  [cite_start]**Application:** The Handler orchestrates data flow using **Repositories**[cite: 130].
3.  [cite_start]**Domain:** Pure C# entities enforce business rules (e.g., checking court capacity) with no external dependencies[cite: 131].
4.  [cite_start]**Infrastructure (Persistence):** **EF Core** saves the data to the SQL database[cite: 132].
5.  [cite_start]**Infrastructure (Azure):** Publishes events to the **Service Bus** for background processing[cite: 133].

> [cite_start]**Pro Tip:** Use **Rider's** built-in **Database tool window (DataGrip)** to monitor your SQL tables in real-time while testing these endpoints in **Postman**[cite: 134].
