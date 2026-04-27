Driving and Vehicle Licensing Department System (DVLD)
A comprehensive system designed to manage licensing processes, tests, and personal records. This project was built with a high focus on Software Architecture and Separation of Concerns to ensure maximum maintainability and scalability.

🚀 Features
The following core modules have been fully implemented with a robust back-end logic:

People Management: A centralized system for handling personal data, including Adding, Editing, and linking records to National IDs.

User Management: A secure authentication and access control system, linking users to their respective personal records.

Application Types Management: A full control panel to manage various application categories and their associated costs.

Test Types Management: A flexible engine to manage different types of tests (Theory, Practical, etc.).

Application System: Complete back-end logic for processing applications, with advanced UI for Add/Edit operations.

🛠️ Technologies
The project was built using "Vanilla" tools to demonstrate a deep mastery of development fundamentals:

Programming Language: C#

Framework: .NET Framework

User Interface: WinForms (Built manually without external libraries to ensure high performance and design consistency).

Database: SQL Server

Data Access: ADO.NET (Direct SQL queries for total control over performance and data mapping).

Version Control: Git.

💪 Strengths & Design Patterns
🏗️ 3-Layer Architecture
The project is strictly divided into three independent layers:

Data Access Layer (DAL): Exclusively responsible for communication with the SQL Server.

Business Logic Layer (BLL): The "brain" of the system, containing all validation rules and business logic.

Presentation Layer (UI): The user interface that communicates solely with the Business Layer.

🛡️ Business Layer Encapsulation
Strict Encapsulation is applied within the Business Layer. Data cannot be accessed or modified except through specific properties and methods that validate business rules before interacting with the data layer, ensuring system integrity and preventing invalid data entry.

🧠 Self-Aware Objects (Mode Pattern)
A smart logic was implemented allowing objects to track their own state via a Mode property:

AddNew Mode: The object recognizes it is new and executes insertion logic.

Update Mode: The object recognizes its existing state in the database and executes update logic accordingly.
This pattern significantly reduces code duplication (DRY Principle) and makes UI integration more intuitive.

📑 Version Control
The project is fully managed using Git, ensuring precise tracking of changes and efficient management of the various development stages.

Note: This project is currently under active development. Current focus is on enhancing the Listing interfaces and adding further functional modules.