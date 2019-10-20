# WordSearcher
This is a sample test of a console application that can process all the text files inside a folder and search words in that files.

This application has some values that can be set up on AppSettings.file to change the behavior of the application:
- "finishWord": this is the text that the user should write to finish the application.
- "punctuationMarks": these are the punctuation marks that are going to be omitted when the application reads all the text of a file.
- "textExtension": this is the file extension of a text file. For this application, it holds only one file extension, but in the future, it can be several splits by commas.
- "numberOfResultsToShow": this is the number of results to display on the console.

# How to run the application
Write the following command on the prompt: "WordSearcher.exe PATH_OF_THE_FOLDER_TO_SEARCH", where PATH_OF_THE_FOLDER_TO_SEARCH is the path of the folder to search.

# Last considerations
This solution has been done with a Visual Studio Community 2019, so it doesn't have a code coverage file that checks the code coverage.
