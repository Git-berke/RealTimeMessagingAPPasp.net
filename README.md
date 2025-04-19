DoctorPatientChat Project
This project is a web application that enables real-time messaging between doctors and patients. It is developed using SignalR, ASP.NET, and a Microsoft Access database.

Requirements
Visual Studio
.NET Framework: 4.8
Microsoft Access Database Engine: For OLEDB support
Database: DoctorPatientChat.accdb file
Installation Steps
Download or Clone the Project
Copy the project folder to your computer.
Check the Database File
Ensure the DoctorPatientChat.accdb file is located at the following path:
DoctorPatientChat\DoctorPatientChat\App_Data\DoctorPatientChat.accdb
If the file is missing, create a new database using Microsoft Access and set up the required table structure.
Verify the Connection String
Check the connection string in the Web.config file:
xml

Kopyala
<connectionStrings>
  <add name="ChatConnectionString"
       connectionString="Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\DoctorPatientChat.accdb;Persist Security Info=False;"
       providerName="System.Data.OleDb" />
</connectionStrings>
|DataDirectory| automatically points to the App_Data folder.
Set Database File Properties
In Visual Studio Solution Explorer, right-click on the DoctorPatientChat.accdb file.
Set the "Copy to Output Directory" property to "Copy if newer" or "Copy always."
Install NuGet Packages
Required NuGet packages:
Microsoft.AspNet.SignalR
jQuery
Bootstrap
To install missing packages:
In Visual Studio: Tools > NuGet Package Manager > Restore NuGet Packages
Run the Project
Open the project in Visual Studio.
Press F5 or click the "Start" button to launch the project.
Log in through the page that opens in the browser to start using the system.
Features
Real-Time Messaging: Instant chat using SignalR.
Read Receipt Indicator: Double blue ticks and time color change for read messages.
Message Search: Search and highlight keywords in message history.
Unread Message Notification: Badge for unread messages.
Multi-User Support: Chat between different users.
Secure Session Management: Login and logout operations; user information is encrypted.
User Profile Management: Ability to manage user profiles.
Known Issues and Limitations
Database File Path: If the DoctorPatientChat.accdb file is not in the App_Data folder or the connection string is incorrect, a "file not found" error may occur.
Multi-Session Testing: To test multiple users simultaneously, use different browsers, incognito tabs, or devices.
Access File Locking: In multi-user scenarios, the Access database may lock. It is not recommended for high-traffic environments.
Backup: The Access database may become corrupted; regular backups are recommended.
Mobile Compatibility: Although responsive design is implemented with Bootstrap, minor visual issues may occur on mobile devices.
Security: Advanced security measures such as encryption, CAPTCHA, or brute-force protection are not implemented.
