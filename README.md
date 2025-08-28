[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/557gZ8MG)


ABC Retail – POE Project

Overview

The ABC Retail POE project is a .NET 7 ASP.NET Core MVC web application designed to simulate an e-commerce environment. The project integrates with Azure Storage Services (Tables, Queues, and Blobs) to manage retail operations including Customers, Products, Orders, Media (Images), and Contracts.

This project was developed as part of a Portfolio of Evidence (POE) to demonstrate knowledge of cloud computing, secure data handling, and modern web development practices.


Features

Customer Management – Add, view, and manage customers.

Product Management – Add, update, list, and delete products. Supports image storage in Azure Blob Storage.

Orders Management – Create and track customer orders.

Contracts – Store and retrieve uploaded contracts.

Media (Images) – Upload, display, and manage product images.

Azure Integration:

Azure Table Storage → Products, Orders, Customers

Azure Blob Storage → Images and Contracts

Azure Queue Storage → Background tasks / async processing

 Tech Stack

Frontend: Razor Pages, Bootstrap 5

Backend: ASP.NET Core MVC (C#)

Database/Storage: Azure Table Storage, Blob Storage, Queue Storage

Cloud: Microsoft Azure

Other: Entity-like data handling with Azure SDK

Project Structure 
ABCRetailPOE/
Controllers/       # MVC controllers (Products, Customers, Orders, Media, Contracts)
Models/            # Entity models for storage mapping
Services/          # Azure Storage services (Blob, Table, Queue)
Views/             # Razor Views (UI)
wwwroot/           # Static files (CSS, JS, images)
appsettings.json   # Configuration (Azure storage connection strings, etc.)
Program.cs         # Entry point & service registration

Setup & Installation

Clone the repository

git clone https://github.com/VCDN-2025/cldv6212-poe-part-1-CalibLaaitonFrank 
cd ABCRetailPOE

Run the application
dotnet restore
dotnet build
dotnet run


Open in browser:
https://st10451026.azurewebsites.net 

Testing

Navigate to /Products → Manage products

Navigate to /Customers → Manage customers

Navigate to /Orders → View/manage orders

Navigate to /Media → Upload and view images

Navigate to /Contracts → Upload and test contract files

Learning Outcomes

Understanding cloud integration with Azure Storage.

Building scalable and modular ASP.NET Core MVC applications.

Implementing CRUD operations with cloud services.

Designing an e-commerce simulation with real-world business logic.

Author

Developed by Calib Frank as part of a Portfolio of Evidence.
