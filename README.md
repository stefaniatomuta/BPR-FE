# BPR-FE

Bachelor project that aims to quantify code analysis and software architecture in terms of technical debt.

Functional requirements:

- [x] As a User, I want to define an architecture model and the components within it so that I can have a skeleton of a model
- [x] As a User, I want to define dependencies between different architecture components in a model so that I can have some dependency rules to have a solution be analysed against
- [x] As a User, I want to give an architecture model and its architecture components names so that I can easily distinguish between different architecture models and architecture components
- [x] As a User, I want to have an overview of my collection of architecture models so that I can see which architecture models I can perform analysis against
- [x] As a User, I want to select an architecture model I want the source code to be analysed against so that my solution has a model to be compared against
- [x] As a User, I want to define a mapping between a namespace and an architecture component so that the system can analyse the dependencies in regard to the selected namespace and architecture component
- [x] As a User, I want to upload the source code of a solution so that the system is able to analyse the violations causing technical debt
- [x] As a User, I want to analyse a source code's overall classification of the amount of technical debt of a source code so that I can get a quick overview of the status of a solution in regard to technical debt
- [x] As a User, I want to analyse a source code's project dependencies against a selected architecture model so that I can get an overview of the dependency violations in regard to the selected architecture
- [x] As a User, I want to view a description of each violation of a completed analysis so that I am aware of the exact cause of the violation
- [x] As a User, I want to view a comprehensive analysis based on the rules I selected so that I can have a full overview of my system
- [x] As a User, I want to have an overview of analyses I have started displaying the name, status, start date and end date so that I can easily see which analyses are complete
- [x] As a User, I want to view the result of an analysis based on the previously selected rule types, so that I have all the details of the finished analysis displayed
- [x] As a User, I want to configure the state of a dependency between two architecture components to be open or closed so that I can model my architecture with open and close layers when validating for dependencies
- [x] As a User, I want the system to automatically define the mappings between namespaces and architecture components based on name so that I do not spend time on manually mapping the namespaces myself
- [x] As a User, I want to select the rules I want the source code to be analysed against so that the result will be based only on the criteria I need
- [x] As a User, I want to give an analysis a name so that I am able to easily distinguish between different analysis results
- [x] As a User, I want to analyse a source code's number of files, classes, interfaces, methods, inheritance declarations and using directives so that easily see the distribution between these solution metrics
- [x] As a User, I want to analyse a source code's class coupling so that I can easily get an idea of which classes might need to be refactored
- [x] As a User, I want to analyse a source code's number of lines of code per file so that I can easily get an idea of which classes might need to be refactored to reduce the size
- [x] As a User, I want to analyse a source code's referenced NuGet packages and versions as well their usage throughout the solution, so that I can get an overview of which external packages the solution is most dependent on
- [x] As a User, I want to analyse a source code's projects' frameworks and the frameworks' status so that I can easily see if the solution contains unsupported frameworks that need to be updated
- [x] As a User, I want to analyse a source code's code similarity of files in the solution so that I can get an overview of which files could be candidates for refactorization
- [x] As a User, I want to analyse a source code's namespaces so that I can get an overview of the namespace violations across the solution files
- [x] As a User, I want to view the total number of violations of a completed analysis so that I am aware of the exact number of violations in the uploaded solution
- [x] As a User, I want to view the code that represents a violation so that I am aware of the exact location of the violation
- [x] As a User, I want to download the results so that I have easy access to the analysis in the future
- [x] As a User, I want to delete an architecture model so that my collection of architectures is updated only with the ones I am planning to use
- [x] As a User, I want to edit an architecture model including components and dependencies so that I can match an existing architecture to my current ideal of structure
- [x] As a User, I want to see a diagram of the selected architecture model so that I have an overview of all dependencies in an ideal solution
- [x] As a User, I want the system to display a diagram of the architecture model the dependency analysis is based so that I can visualise the highlighted dependency violations on a component level
- [x] As a User, I want to filter the the result of an analysis based on the rules the analysis is based on so that I can have a visual representation of only the rule types I choose
- [x] As a User, I want to delete the result of the analysis so that I can manage the overall results I am interested in
- [x] As a User, I want to analyse a source code's number of lines of comments per file so that I can get an overview of the files that might need to be investigated in regard to commented code
- [x] As a User, I want to analyse a source code's distribution of the conditional statements if, for, foreach and while so that I keep track of the conditional statements and loops across the solution
- [x] As a User, I want to analyse a source code's distribution of number of lines of code and number of lines of comments so that I have an overview of how many comments are across the solution compared to the code lines

How to set up the project:
 - Make sure you have DOTNET 6.0 SDK installed on your PC
 - Navigate to the root of the project where the docker-compose.yml file is
 - Build up the docker compose file by runnning this command
    ``` docker compose up ```
 - Add the config/Rules.json as a 'Rules' collection in the MongoDB or you can use MongoCompass to do so
    ![image](https://github.com/stefaniatomuta/BPR-FE/assets/72012292/bd1e056a-4cf4-454b-a26c-87c2ac0a2d11)

Important information:
 - This repository has an external integration to a Python application that can be found [BPR-MAL]([https://link-url-here.org](https://github.com/stefaniatomuta/BPR-MAL)https://github.com/stefaniatomuta/BPR-MAL)

Contributors:
- Adriana Grecea - 304149
- Ioan Vlad Dirlea - 304182
- Morten Hansen - 304668
- Stefania Tomuta - 304173





