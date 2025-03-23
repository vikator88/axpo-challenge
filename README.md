# Technical Test for Axpo

This repository contains the solution for the technical test given by Axpo. Below you will find information on how to execute the console command, the structure of the solution, the external libraries used, and how the logging and worker loop have been implemented.

## How to Run the Console Command

To run the application, execute the following command in the terminal:

```bash
dotnet restore
dotnet build
dotnet run --project .\src\AxpoChallenge -- [parameter1] [parameter2] ... [parameterN]
```

### Parameters:

1. **-h, --help**: Shows help for the command
2. **-t <intervalInMinutes>**: (Required) Interval to run the report generation
3. **-o <outputFolder>**: (Optional) Folder where the CSV files will be saved. Default: current directory
4. **-d <executionDate>**: (Opional) Date of report generation. Format: YYYYMMDD. This tool generates reports for day-ahead. Default: current date
5. **-tz <timeZoneString>**: (Optional) Time zone of the server. Keep in mind your OS. Default: Central European Standard Time (Windows) or Europe/Berlin (Unix-like)

Ensure that you provide valid parameters as the application requires them for proper execution.

## Solution Structure

This project follows the **Clean Architecture** pattern, which promotes separation of concerns and scalability. The solution is divided into the following layers:

1. **Domain**: Contains the business logic and domain entities.
2. **Application**: Contains use cases, services and some service interfaces to be implemented on Infrastructure.
3. **Infrastructure**: Contains the implementation of external services. In this challenge, access to API via PowerService.dll or export to CSV file.
4. **Presentation**: The console application that interacts with the user and runs the application.

The core logic is decoupled from the infrastructure, allowing for easier testing and extension of the solution.

### About Domain Layer

In this case, the domain layer includes a small example of Domain-Driven Design (DDD), with two entities and a value object that represent the model of this mini application.

To make it a complete DDD implementation, many other aspects are missing, such as domain events. However, I wanted to showcase a small part of how I prefer to structure applications.

### About the domain itself

The entity PowerTradeEntity and the value object PowerPeriodValueObject represent the same as the PowerTrade class and PowerPeriod structure from the provided PowerService.dll. However, it is important to highlight that, by doing this, we can decouple external entities from internal ones. This way, if the external entity changes in the future, all the logic based on the internal entity will continue to work. The only change required would be to modify the logic in the mappers.

## Design Patterns Used

I also didn't want to miss the opportunity to use some design patterns in the places where I considered it necessary:

### Builder Pattern:

- Description: The Builder pattern is used to construct complex objects step by step. It allows for the creation of objects with a variety of configurations without having to create multiple constructors or setters for each combination.
- Pros: It provides a clear separation between the construction of an object and its representation. This makes the code easier to maintain and extend, especially when dealing with complex objects.
- Usage: I used it to generate the options object that will be used throughout the execution of the program. I also used it to create some of the test objects in the unit tests.

### Repository Pattern:

- Description: The Repository pattern is used to encapsulate the logic of accessing data sources. It provides a central location to query and persist objects, which allows for easier maintenance and testing by isolating data access code from the rest of the application.
- Pros: It helps to centralize the logic for data access, reducing duplication and making it easier to modify or extend without affecting other parts of the application. It also improves testability.
- Usage: I used it to encapsulate the logic for fetching the PowerTrade entities from the external service.

### Factory Pattern:

- Description: The Factory pattern is used to create objects without specifying the exact class of the object that will be created. It delegates the responsibility of object creation to a separate factory class, which allows for easier management of complex object creation processes.
- Pros: It abstracts the instantiation logic and provides flexibility in creating different objects, particularly when you need to handle variations in object creation. It also improves code readability and maintainability.
- Usage: I used it to create the mappers for the CSV export. In this specific case, where there is only one entity to export, it wasn't strictly necessary, but I wanted to incorporate the knowledge into the code.

## External Libraries Used

### CsvHelper

- **Purpose**: This library is used for reading and writing CSV files in a strongly-typed manner.
- **Usage**: It allows us to easily serialize objects into CSV format.

### Polly

- **Purpose**: Polly is used to handle transient fault handling and retries.
- **Usage**: The library is used to implement retry logic in cases where an operation might fail due to temporary issues, such as PowerService.dll failures or access to disk failures.

## Worker Loop Implementation

The worker loop is managed by a background worker that continuously runs in a loop. In each iteration of the loop, a new task is created using `Task.Run()` to execute the use case asynchronously. After the task finishes, the worker sleeps for the specified duration defined in the configuration before running the next iteration.

The code itself contains a commented-out implementation of the loop in a blocking execution, where sleep only occurs when the use case has finished.

Why is the first option (background task) better?

1.  Avoids blocking the loop: In the first option, if the use case takes longer than the interval, the next execution will be delayed.

    - This can cause drift and prevent timely executions.

2.  Ensures periodic execution: The second option always triggers the execution at the correct interval, even if the previous one is still running.

3.  Parallel execution: If needed, multiple executions can overlap without delaying the next scheduled run.

4.  Fault tolerance: If an execution fails, the loop continues running and schedules the next one as expected.

The main risk is that if a task takes too long, multiple executions could overlap, leading to resource contention or performance issues.
For this challenge, it won't be a problem because it only uses two resources: an API to fetch data and a CSV file to write the results with different names, depending on the time it was executed, so there will not be any data overwriting issues.

## Logging System

The solution uses **console logging** to provide feedback on the application's execution.

- **Info**: Used when something important happens, like the successful completion of a task or a significant milestone.
- **Warn**: Used when something fails but is recoverable, such as a minor issue that does not stop the execution of the program.
- **Error**: Used when the task fails completely, causing it to stop.

## Tests

To run the application, execute the following command in the terminal:

```bash
dotnet restore
```

I have implemented both integration tests and unit tests.

For the unit tests, I have used the Moq library to simulate calls to the external service.

In total, I have created 9 test cases, organized as follows:

### Unit Tests for PowerTradeAggregationService

- Test for Aggregating Power Trades: This test ensures that the AggregateTrades method correctly returns aggregated power positions when valid input is provided.
- Test for Handling Different Number of Periods: This test verifies that an exception is thrown when the power trades being aggregated have a different number of periods.
- Test for Handling Different Dates: This test checks that an exception is thrown if the power trades being aggregated have different dates.

### Integration Tests for PowerTradeAggregationService

- Test for Aggregating Power Trades: Similar to the unit test, but this integration test ensures that the AggregateTrades method works as expected when interacting with the actual external service, returning the correct aggregated power positions.
- Test for One-Hour Difference on Winter Daylight Saving Date: This test checks that when two consecutive days are aggregated during the winter daylight saving time change, the difference between them is correctly handled as one hour.
- Test for One-Hour Difference on Summer Daylight Saving Date: This test verifies that when two consecutive days are aggregated during the summer daylight saving time change, the difference between them is correctly handled as one hour.

### Integration Tests for PowerTradeRepository

- Test for Valid Trades from Service: This test ensures that the GetTradesAsync method returns valid PowerTrade objects when the service is called, verifying that the external service is functioning correctly.

### Unit Tests for PowerTradeRepository

- Test for Correct Mapping of Power Trades: This test verifies that the GetTradesAsync method correctly maps the data from the external service to the PowerTrade objects in the application.
- Test for Empty List when No Trades are Found: This test checks that the GetTradesAsync method returns an empty list when no trades are found, ensuring that the method behaves properly in this scenario.
