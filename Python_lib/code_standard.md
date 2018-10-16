# Code Standard
==============================================
Key Operating Guidelines
Code using Object Oriented design principles implementing SOLID 
Focus on re-usability wherever possible: keep your code DRY (Don't Repeat Yourself)
Take a microservice approach
Refactor early, Refactor often
Iterations:
Make it Work
Make it Right
Make it Fast
Maintain a high level of code coverage with unit tests (strive for 100% coverage)
Characteristics of Software Quality1
Software has both external and internal qualities. External qualities are qualities that the client or software user would be aware of. Internal qualities are only apparent from the code (or perhaps data) itself. Of course, the line between these two is blurry, because internal qualities (e.g. no testing) will usually result in external qualities (poor reliability).

Here is a list of external qualities of a piece of software:

Cost - How much did it cost to create and maintain?
Correct - Does the software do what it was supposed to do?
Usable - Is the software easy to use? There are many tradeoffs even within usability; what makes software easy for new users may conflict with what is easy for power users.
Performant - Does the software make good use of system resource such as memory, disk space, network, process time?
Reliable - Is the software bug-free?
Secure - Does the software keep hackers and unauthorized users out of it?
Adaptable - Can the system be altered to work without modification in different environments; usually this means that it is appropriately configurable.
Robust - Does the system continue to work in the presence of invalid input or stressful environmental conditions.
Here is a list of internal qualities of a piece of software:

Clarity - Can you easily tell what a piece of code is supposed to do? Can you look at it and predict its external qualities?
Maintainable - How easy is it to keep the software working over time and to include new features?
Portable - How easy is it to port the software to other environments (e.g. from Linux to Windows)
Reusable - Can we save time and money on other projects by re-using the code?
Readable - Is the code easy for developers to read and understand?
Testability - Is it constructed in a way that the pieces can be easily and independently tested?
Tested - Are there automated tests that document how the code should work, and allow us to easily test whether it does?
Commenting and Documentation
All class and functions should be documented to explain:
The purpose of the class or function
What the expected inputs and outputs are
If the code is difficult to understand, comments should be made within the function to explain what is happening
If the project has been imported as a library or package, the function comments that were made can be referenced from intellisense
Avoid obvious comments
If possible, attempt to find a library or plugin that can assist with building documentation, for example, in python, there is Sphinx
Any api rest end points should be documented, also if possible, attempt to find a library or plugin that can assist with this, for example, Swagger
TODO Comments:
A comment with the form of TODO, should mark future development, or note that it can be improved with future releases of packages or frameworks
If the code notated by the TODO should be fixed in the current iteration of the code, due to code that is not to par, it should fixed prior to submitting for code review
If code requires further improvement or work, it should be noted with a TODO, not standard commenting, as this can help with assessing technical debt
Project README's are required for each project and should explicitly state:
The intended purpose and usage of the project
How to build/run the project
The project's code versions / specifications (e.g. Node.js 8.10.0, Grails 3.2.9, Gradle 4.9, Java 1.8_0_091)
Information Pages, for example, if Swagger is implemented
Any other projects that the code relies upon, for example, if this is a UI and it requires several web services, these web services should be enumerated
Any required environment variables
Any static analysis tools used, (Code Narc, EsLint, SonarLint)
How to run the tests and check code coverage
Project, Class, and Function/Method Naming
Project, class, function/method naming should represent the purpose of the code, do not shy away from long method names if they describe the method appropriately
Project names should end in "-ws" if they are strictly a web service or api
Project names should begin with "pg-", "mongo-", etc if they are used to store SQL scripts for building projects
Avoid creating generic utility or helper classes that end up becoming a dumping ground for miscellaneous code
Follow consistent naming schemes (if a code base's best practice is camelCase or under_scores:
Python: my_method()
Groovy: myMethod()
Indentation, Spacing, Code Grouping, and Variable Naming
Code should be indented and consistent, language indentation preferred styles should be followed, for example, python should follow PEP 8
Use 4 spaces, not 2 spaces
Do not have excessive line breaks in code, but do use line break to make the code more readable, code should not be jammed together
Focus on how the spacing, indentation, and line breaks assist to make the code more readable
Methods that may require several lines of code, should be kept together, but within separate blocks of code with a line break between them
Commenting before blocks of code may help emphasize visual separation
Limit line length to make code easier to read, lines should be not be horizontally long to the extent that you need to scroll very far right to read the code
Capitalize SQL special words (e.g. SELECT id, username FROM user;)
Python variables and functions/methods should be preceded with underscores only if they are considered private or internal (e.g. def _my_private_method())
Do not submit code for code review that has print() statements, these should either be removed if they were used for code testing, or changed over to application logging
Constants should be all UPPERCASE and word delimited by an underscore: MY_CONSTANT, the only exception to this is javascript where const variables are declared for ES6

Python

def exist_middle_init(self, a_str):

 """
 Check if there's a middle name with only 1 character
 :param a_str:
 :return:
 """
 updated_str: str = self.add_space_after_comma(a_str.strip())
    for s in RegexPatterns.white_space().split(updated_str):
        if RegexPatterns.middle_name().match(s):
            return True
 return False


Groovy

def getUniqueAwards(){
    def allAwards = getAllAwards()
    def uniqueAwards = []
    allAwards?.each{
        def awardMap = [awardCode: it?.awardCode, awardCodeAndName: it?.awardCodeAndName]
        if (!uniqueAwards.contains(awardMap)){
            uniqueAwards.add(awardMap)
        }
    }

    return uniqueAwards
}
File and Folder Organization
If there is an existing framework, follow its folder structure
Keep code organized in folders in such a way that it is easy to find code:
All ORM representations in one folder
All Controllers in one folder
etc.
It may be helpful to create subfolders to hold related grouping of code or classes
If a framework uses several programming languages, keep the specific languages in their own folders, for example, with Grails/React: the groovy code is separated from the react javascript code 
Software Complexity
Avoid overly complex code where possible, linting libraries or packages can assist in analyzing code complexity
Avoid deep nesting, this may make code difficult to follow
A class or function/method should do one thing, if there is more happening in the method, it should be separated out to other functions or methods, so that there is only one reason that the code is changing - See the SOLID coding principle