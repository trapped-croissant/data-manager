# data-manager

Aspire Application for generating random data into a csv file, importing data into a database, or creating a table in a database.\
This is currently a work in progress and will be adding to it as time permits. 

I am always striving for better so any constructive feedback is welcomed and appreciated. 

## AppHost

This Project is for the Aspire AppHost. This is the main running project that will start :
 - Postgres
 - Redis
 - Carr.Data-Generator
 - data-manager-front-end

## Carr.DataGenerator

This Project is for generating and downloading a csv file with random data. The endpoint can specify the number of columns and rows.\
When generating data, it will randomly pick a data type from the following :
 - String
 - Decimal
 - Integer
 - DateTime
 - Guid
 - Boolean

The endpoint uses multithreaded functionality to equally distributes the workload across all processors on the hosted machine

## data-manager-front-end

This Project is a React App that allows the user to interact with the api through a convenient GUI