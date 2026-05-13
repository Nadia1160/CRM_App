# CRM App – ASP.NET Core 8 Web App

[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-blue)](https://dotnet.microsoft.com/)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-8.0-purple)](https://docs.microsoft.com/ef/core/)
[![SQLite](https://img.shields.io/badge/SQLite-3-blue)](https://www.sqlite.org/)

## Overview

This CRM application demonstrates solutions to **four real-world ASP.NET Core challenges**, plus extended CRM features inspired by Zoho CRM.

- ✅ **PDF Generation Configuration** – fixed QuestPDF licensing and file permissions.
- ✅ **EF Core Cascade Delete** – properly configured to delete orders when a customer is removed.
- ✅ **JavaScript Calculation Error** – accurate real-time total using `parseFloat` and `toFixed(2)`.
- ✅ **Filter on Razor Page List View** – server-side search by name or city.

## Additional CRM Features

- **Leads** – full CRUD with status lifecycle (New → Contacted → Qualified → Converted), conversion to Account, activity timeline.
- **Accounts** – company management.
- **Activities** – log calls, emails, meetings, notes.
- **Dashboard** – KPIs (customers, orders, leads, conversion rate) + bar & pie charts.
- **Export/Import** – leads CSV export & bulk import.
- **Theme Toggle** – light/dark mode with local storage persistence.

## Technology Stack

- .NET 8.0
- ASP.NET Core Razor Pages
- Entity Framework Core (SQLite)
- QuestPDF (PDF generation)
- Bootstrap 5 + Bootstrap Icons
- jQuery DataTables
- Chart.js
- AJAX modals for lead creation

## How to Run Locally

1. **Clone the repository**
   ```bash
   git clone https://github.com/Nadia1160/CRM_App.git
   cd CRM_App