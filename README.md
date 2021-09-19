# public-address-book
.net core + angular + signalR

# Database
This project was made with a database-first approach. The table Contact has a unique key combining the 
columns 'Name' and 'Address' meaning we cannot have more than one entry with the same name and address.
The table PhoneNumber has a unique key combining 'Number' and 'ContactID' restricting one contact to have multiple same numbers.
The relation is one (Contact) to many (PhoneNumber).

# API
.NET Core API, uses dependency injection, repositories, custom classes for error handling, etc. 
Provides API for the model classes Contact and PhoneNumber.

# Client
Simple Angular app used for testing a basic functionality of getting the public address book contacts.

SignalR has been integrated and made to work between the API and the client. 
Modifying data in any way will reflect in real-time changes on the front-end.
