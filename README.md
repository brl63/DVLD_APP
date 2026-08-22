# DVLD Management System

The **DVLD (Drivers and Vehicles License Department) Management System** is a comprehensive desktop solution designed to manage all aspects of driving license administration. Built using a strict **3-Tier Architecture**, the system handles the issuance, renewal, and replacement of driving licenses, including international permits. It ensures regulatory compliance, data integrity, and driver competence through automated validation pipelines and testing lifecycles.

---

##  Demo 
You can use the following default account to sign in and explore all the system :
* **Username:** `user4`
* **Password:** `1234`

---

##  Key Services
* **New Driving License:** Apply for a new driving license across multiple license categories with dynamic fee structures
* **Renew License:** Renew an existing driving license, ensuring records are up-to-date, valid, and previous licenses are deactivated
* **Replace Lost License:** Issue a replacement for a misplaced license with proper validation to prevent duplicate active records
* **Replace Damaged License:** Replace a physically damaged license with identical expiration dates
* **Release Detained Licenses:** Release an impounded license by settling required service fees and any associated fines
* **Issuance of an International License:** Apply for an International Driving Permit, exclusively available to holders of valid local licenses (Class 3)
* **Re-examination Service:** Request test retakes 

---


##  Technologies Used
* **Architecture:** 3-Tier Architecture (`Data Access Layer`, `Business Logic Layer`, `Presentation Layer`)
* **Framework / Language:** C# (.NET Framework) / windows Forms / sqlServer / ado.net




## How to Run the Project Locally

* Make sure to have ssms

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/brl63/DVLD_APP.git
   ```
   Make sure that the 3 projects be cloned if not add them manually Add exciting project

2. **Database Setup:**
   * Open **SQL Server Management Studio (SSMS)**.
   * Create a new database named `DVLD`.
   * Open and execute the script located at `Database/DVLD_DB_Script.sql` against your `DVLD` database to build all tables, views, and seed data.

3. **Configure Connection String:**
   * In the **Data Access Layer** project, open `clsDataAccessSetting.cs`.
   * Verify or update the connection string to match your local SQL Server instance:
     ```csharp
     public static string _connectionString = "Server=.;Database=DVLD;Integrated Security=True;";
     ```
    and run but make sure that the startup project is on DVLD_APP



##  Database Schema & Core Entities

<img src="https://github.com/user-attachments/assets/86d09a86-56ae-4498-8967-2d2c18cfc2f7" width="800" />
---

##  Important Note Regarding Profile
* **Sample Data Image Paths:** The seeded database records contain absolute file paths referencing local image directories used during development so  what will happen is When running the project on a new machine, existing sample profiles may not display photos or may trigger image loading warnings due to mismatched local directories. This is completely normal and does not affect application functionality.
* **Adding New Data:** Creating new people or updating existing records via the application UI using your local image picker will save and display images correctly on your system.
* Also there is a problem actually whilke searching about a person in ctrlPersonCardWithFilter that showed the search is wrong : its not wrong actually u just have to search with the opposite thing u wanna search about ( if u wanna search for a person by personid choose the national num ) and as that "I was too lazy to solve that"

---
