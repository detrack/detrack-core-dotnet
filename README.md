![Detrack logo](https://www.detrack.com/wp-content/uploads/2019/06/Logo_detrack-500px.png)
# detrack-core-dotnet

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Official core library for .NET applications to interact with the [Detrack](https://www.detrack.com) API.

## Installation
Install with dotnet-cli:
```
dotnet add package Detrack.DetrackCore
```
Add your default API KEY (this can be retrieved from the dashboard web application):
```csharp
using Detrack.DetrackCore;
Job.DefaultApiKey = "keygoeshere";
```
**Note:** All methods below are Http request which is async, therefore it needs wrapped in an async Task method during call and then awaited. (More information on this in the examples)

## For single job
#### Instantiate a new `Job` class:
```csharp
Job job = new Job("doNumber", "date", "address");
```
#### Create a new job:
```csharp
// instantiate a job class
Job job = new Job("doNumber", "date", "address");

// static method
await Job.CreateJob(job);
```

#### Update a job:
**Note:**
- UpdateJob is a **non-static** method.
- Both parameters are optional. If not given, it will take the instance's do number and date
```csharp
// instantiate a job class
Job job = new Job("doNumber", "date", "address");

// instance method
await job.UpdateJob("doNumber", "date");
```

#### Retrieve a job:
**Note:** Returns a `Job`.
```csharp
Job job = await Job.RetrieveJob("doNumber", "date");
```

#### Delete a job:
**Note:** Job can only be deleted if its status is "info received" or "out for delivery"
```csharp
await Job.DeleteJob("doNumber", "date");
```

#### Reattempt a job:
**Note:** Only **failed** jobs can be reattempted
```csharp
await Job.ReattemptJob("doNumber", "date");
```

## For multiple jobs
**Note:** All batch jobs uses `List<Job>` as a parameter
#### Create a list of jobs:
```csharp
List<Job> joblist = new List<Job>();

// instantiate several jobs
Job job1 = new Job("doNumber", "date", "address");
Job job2 = new Job("doNumber", "date", "address");
Job job3 = new Job("doNumber", "date", "address");

// add the instantiated job into the joblist
joblist.Add(job1);
joblist.Add(job2);
joblist.Add(job3);
```

#### Batch create jobs:
```csharp
// create a list of jobs
List<Job> joblist = new List<Job>();

// instantiate several jobs
Job job1 = new Job("doNumber", "date", "address");
Job job2 = new Job("doNumber", "date", "address");
Job job3 = new Job("doNumber", "date", "address");

// (optional) you can change fields before adding them into the list
job1.assignTo = "driver";
job2.OpenToMarketplace = true;

// add the instantiated job into the joblist
joblist.Add(job1);
joblist.Add(job2);
joblist.Add(job3);

// batch create jobs
await Job.CreateJobs(joblist);
```

#### Batch update jobs:
Batch update jobs will update job with the specified `doNumber` and `date`.
```csharp
// create a list of jobs
List<Job> joblist = new List<Job>();

// instantiate several jobs
Job job1 = new Job("doNumber", "date", "address");
Job job2 = new Job("doNumber", "date", "address");
Job job3 = new Job("doNumber", "date", "address");

// (optional) you can change fields before adding them into the list
job1.assignTo = "driver";
job2.OpenToMarketplace = true;

// add the instantiated job into the joblist
joblist.Add(job1);
joblist.Add(job2);
joblist.Add(job3);

// batch update jobs
await Job.UpdateJobs(joblist).Wait();
```

#### Batch delete jobs:
Batch delete jobs will update job with the specified `doNumber` and `date`.
```csharp
// create a list of jobs
List<Job> joblist = new List<Job>();

// instantiate several jobs
Job job1 = new Job("doNumber", "date", "address");
Job job2 = new Job("doNumber", "date", "address");
Job job3 = new Job("doNumber", "date", "address");

// add the instantiated job into the joblist
joblist.Add(job1);
joblist.Add(job2);
joblist.Add(job3);

// batch delete jobs
await Job.DeleteJobs(joblist);
```

## Extra Functions
#### List All Jobs:
**Note:** 
- Returns `List<Job>`
- Parameters available is : `page`, `limit`, `date`, `type`, `assignTo`, `status`, `DONumber`. ALl parameters are optional parameters (giving an empty `Dictionary<string, string>` will still work.)
- `assignTo` can be "_name of driver_", "unassigned" or empty.

```csharp
// create a dictionary with type string as key and values
Dictionary<string,string> parameters = new Dictionary<string,string>();

// add your parameters
parameters.Add("page", 5);
parameters.Add("date", "2019-05-01");
parameters.Add("status", Status.completed_partial.ToString());
parameters.Add("assignTo", "unassigned") //or parameters.Add("assignTo", "your driver")

// list all jobs
List<Job> joblist = await Job.ListAllJobs(parameters);
```

#### Download Job POD or shipping label as pdf:
**Note:** pathToSaveFile will start in the folder that contains the dll. If you want to go out of the folder then do `..`
```csharp
await Job.DownloadJobExport("doNumber", "pathToSaveFile", "documentType(pod or shipping-label)", "date");
```

## Adding Items on a Job
**Note:** 
- Updating an item requires you to create a new `Item` class and push the update
- Pushing an item will delete all the existing items so if youwant to have multiple items make sure to update them in 1 request
#### Adding Items
```csharp
// instantiate a job
Job job = new Job("doNumber", "date", "address");

// create a new item class
Item item = new Item();

// (optional) Change some fields here
item.Description = "Insert description here";
item.Quantity = 15;
item.RejectQuantity= 20;

// add the itemlist into the job
job.Items.Add(item);

// update the job
await Job.UpdateJob("doNumber", "date");
```

## Examples
**Note:** As mentioned before, Http request is async so the methods must be wrapped in an async method. `.Wait()` and `.Result()` will cause instant deadlock when called from a UI thread. It will work fine on console but considered an unsafe practice. Below is one way to avoid the deadlock :

```csharp
using System.Threading.Tasks;
using Detrack.DetrackCore;

public static async Task CreatingJob()
{
    Job.DefaultApiKey = "yourapikey";
    Job job = new Job("doNumber", "date", "address");
    await Job.CreateJob(job);
}
```
