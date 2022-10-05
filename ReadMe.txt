As I assumed this project was to be read by a developer, the “entering” point of the project was done through the unit tests developed in the Test project. Each test describes one of the situations mentioned to be solved in the requirements of the project.
Even though I tried to keep it simple and not over complicate the solution, the layers of the solution are:
UI(ish) – TeacherComputerRetrievalUI, TeacherComputerRetrieval class, that is to be the foundation/helper of some more developed UI, whatever that might be: a windows application, API, Web-application, etc
Service – Services project, which is composed for the moment of only PathsService that implements an interface, which among other advantages, it can easily be replaced later on, eventually by dependency injection mechanism
Common – which hold the common classes needed for moving data and interfaces between layers.
