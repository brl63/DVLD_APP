# DVLD Management System

## 📌 Overview
The **DVLD (Drivers and Vehicles License Department) Management System** is a comprehensive desktop solution designed to manage all aspects of driving license administration. Built using a strict **3-Tier Architecture**, the system handles the issuance, renewal, and replacement of driving licenses, including international permits. It ensures regulatory compliance, data integrity, and driver competence through automated validation pipelines and testing lifecycles.

---

##  Demo / Testing Credentials
You can use the following default account to sign in and explore all system modules:
* **Username:** `user4`
* **Password:** `1234`

---

##  Key Services
* **New Driving License:** Apply for a new driving license across multiple license categories with dynamic fee structures.
* **Renew License:** Renew an existing driving license, ensuring records are up-to-date, valid, and previous licenses are deactivated.
* **Replace Lost License:** Issue a replacement for a misplaced license with proper validation to prevent duplicate active records.
* **Replace Damaged License:** Replace a physically damaged license with identical expiration dates.
* **Release Detained Licenses:** Release an impounded license by settling required service fees and any associated fines.
* **Issuance of an International License:** Apply for an International Driving Permit, exclusively available to holders of valid local licenses (Class 3).
* **Re-examination Service:** Request test retakes with automated fee calculation for failed test stages.

---

##  System Management
* **User Management:**  
  * Add, view, update, delete, and manage active/inactive states for operator accounts.  
  * Secure credential management, login authentication, and password change modules.  
  * Role and audit tracking (`CreatedByUserID`) across all departmental actions.
* **Person Management:**  
  * Centralized registry enforcing unique national identification numbers.  
  * Manage comprehensive profile details: full name, date of birth, contact details, address, and personal photo path.
* **Request / Application Management:**  
  * Track and search applications by ID or applicant national number.  
  * Live status tracking across lifecycle stages (`New`, `Cancelled`, `Completed`).
* **Test Management:**  
  * Complete 3-stage testing pipeline: **Vision Test**, **Written (Theory) Test**, and **Practical (Street) Driving Test**.  
  * Manage test appointment scheduling, lock past records, and enforce sequential passing requirements.
* **License Category Management:**  
  * Configure license categories including minimum age requirements, default validity periods, and class fees.
* **Detained Licenses Management:**  
  * Track custody logs, detention dates, fine assessments, and releasing authorization.

---

##  License Categories
* **Class 1:** Small Motorcycle License
* **Class 2:** Heavy Motorcycle License
* **Class 3:** Ordinary Driving License
* **Class 4:** Commercial Vehicle License
* **Class 5:** Agricultural Vehicle License
* **Class 6:** Small and Medium Bus License
* **Class 7:** Truck and Heavy Vehicle License

---

##  Technologies Used
* **Architecture:** 3-Tier Architecture (`Data Access Layer`, `Business Logic Layer`, `Presentation Layer`)
* **Framework / Language:** C# (.NET Framework)
* **User Interface:** Windows Forms (WinForms) with custom modular `UserControls`
* **Database & ORM:** Microsoft SQL Server using pure ADO.NET (`SqlConnection`, `SqlCommand`, `SqlDataReader`, `SqlDataAdapter`)

---

## How to Run the Project Locally

* Make sure to have ssms

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/brl63/DVLD_APP.git
   ```

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

4. **Build & Run:**
   * Open `DVLD.sln` in **Visual Studio**.
   * Set `DVLD_APP` as the **StartUp Project**.
   * Press <kbd>Ctrl</kbd> + <kbd>Shift</kbd> + <kbd>B</kbd> to build the solution, then press <kbd>F5</kbd> to run.

---

##  Database Schema & Core Entities

<img src="https://github.com/user-attachments/assets/86d09a86-56ae-4498-8967-2d2c18cfc2f7" width="800" />
---

##  Reference Enums & ID Mappings

### 1. Application Types (`enApplicationType`)
* **ID 1 (`NewDrivingLicense`):** First-time application for a local driving license.
* **ID 2 (`RenewDrivingLicense`):** Application to renew an expired license.
* **ID 3 (`ReplaceLostDrivingLicense`):** Issuing a replacement copy for a lost license.
* **ID 4 (`ReplaceDamagedDrivingLicense`):** Issuing a replacement copy for a damaged license.
* **ID 5 (`ReleaseDetainedDrivingLicense`):** Formal request to pay fines and retrieve a detained license.
* **ID 6 (`NewInternationalLicense`):** Application for an International Driving Permit.
* **ID 7 (`RetakeTest`):** Auto-generated application when scheduling a re-test after failing.

### 2. Application Status (`enApplicationStatus`)
* **ID 1 (`New`):** The application is active and currently in progress.
* **ID 2 (`Cancelled`):** The application was cancelled by the user or operator.
* **ID 3 (`Completed`):** The full process is completed (license issued or released).

### 3. Test Types (`enTestType`)
* **ID 1 (`VisionTest`):** Eye and visual examination (Stage 1).
* **ID 2 (`WrittenTest`):** Theory rules and road signs test (Stage 2).
* **ID 3 (`StreetTest`):** Practical vehicle handling and road test (Stage 3).

### 4. License Classes Specifications
* **Class 1 (Small Motorcycle):** Minimum Age: `18` \| Validity: `5 Years`
* **Class 2 (Heavy Motorcycle License):** Minimum Age: `21` \| Validity: `5 Years`
* **Class 3 (Ordinary driving license):** Minimum Age: `18` \| Validity: `10 Years`
* **Class 4 (Commercial):** Minimum Age: `21` \| Validity: `10 Years`
* **Class 5 (Agriculture):** Minimum Age: `18` \| Validity: `10 Years`
* **Class 6 (Small and medium bus):** Minimum Age: `21` \| Validity: `10 Years`
* **Class 7 (Truck and heavy vehicle):** Minimum Age: `21` \| Validity: `10 Years`

### 5. Issue Reasons (`enIssueReason`)
* **ID 1 (`FirstTime`):** License issued after passing all testing stages.
* **ID 2 (`Renew`):** License reissued upon expiration.
* **ID 3 (`ReplacementForDamaged`):** License reissued due to physical damage.
* **ID 4 (`ReplacementForLost`):** License reissued due to loss.
