This is a project put together for a job interview with Goto.

The challenge specifications:

-For this challenge you may use any resource you want. You may search the web for information. You may use libraries with which you are familiar. You may ask us clarifying questions, (Cc: Ryan Jane from our Recruiting Team).
-In the programming language of your choice, please write a server that listens on both UDP and TCP. The port number to listen on is given on the command line. The server returns the current date and/or time in ISO 8601 format, followed by a newline.
 
-There are three possible requests:
"date" --> returns the current date (YYYY-MM-DD)
"time" --> returns the current time (hh:mm:ss, with timezone)
"datetime" --> returns combined date and time (YYYY-MM-DDThh:mm:ss, with timezone)
 

-Over UDP, the server reads one request per packet and replies with the response.
-Over TCP, the server reads one request per line and replies with one response per line. Sockets are closed after 5 seconds of inactivity.