# Kanban Board with GUI

## Introduction
This project was given to us as a first year assignment in the course Introduction to Software Engineering, written by [Idan Saltzman](https://github.com/idsa0), [Idan Goldberg](https://github.com/Goldymen2002) and [Tomer Cohen](https://github.com/tomeraf).

This project delivers a fully functional Kanban board software with a graphical user interface (GUI) to streamline project management by organizing and prioritizing tasks. The tool supports agile development, helping teams visualize and manage their workflows effectively through boards, lists, and cards. Developed in C# using WPF (Windows Presentation Foundation), the project adheres to the N-tier architecture.

## Features
### User Management
- **Registration/Login/Logout**: Users can register, login, and logout securely.
- **Authentication**: Users are authenticated via email and password, ensuring secure access.

### Board Management
- **Create/Delete Boards**: Users can create and delete boards they own, providing flexibility in project organization.
- **Join/Leave Boards**: Users can join and leave boards created by others, facilitating collaboration.
- **Transfer Ownership**: Board owners can transfer ownership to another member, ensuring smooth transitions.

### Task Management
- **Task Attributes**: Tasks have a unique ID, creation time, due date, title, description, and assignee, allowing detailed task tracking.
- **Task Modification**: Tasks can be modified by their assignees, except for the creation time, ensuring accountability.
- **Task Assignment**: Tasks can be assigned and reassigned among board members, promoting efficient task distribution.

## Technical Specifications
### Architecture
- **N-tier Architecture**: The project is structured into three distinct layers (Service Layer, Business Layer, Data Access Layer) to promote separation of concerns and scalability.

### Persistence
- **Database**: The application uses SQLite for data persistence, ensuring data is stored and restored reliably upon program restart.

### Logging and Error Handling
- **Logging**: The system logs important events and errors using log4net, providing comprehensive insights into system operations.
- **Error Handling**: Robust error handling mechanisms ensure the application gracefully manages anomalies and invalid inputs.

### Usability
- **User-friendly GUI**: The WPF-based GUI is designed for intuitive interaction, featuring clear button labels, input validation, and accessible design.

### Additional Functionalities
- **Task Movement**: Tasks can be moved between columns ('backlog', 'in progress', 'done') as per their progress status.
- **Task Limits**: Columns can have task limits to manage workload effectively.

## Project Structure
- **Backend**: Contains the core logic split into ServiceLayer, BusinessLayer, and DataAccessLayer.
- **BackendTests**: A console project dedicated to testing backend functionalities.
- **Frontend**: A WPF project providing the graphical user interface.

## Installation and Setup
1. Clone the repository.
2. Navigate to the project directory and restore dependencies.
3. Build the solution to generate necessary binaries.
4. Run the application from the WPF project to start using the Kanban board.

## Usage
1. **Register**: Create a new user account.
2. **Login**: Access the system using your email and password.
3. **Create Board**: Start by creating a new board.
4. **Add Tasks**: Add tasks to your board and manage them across different columns.