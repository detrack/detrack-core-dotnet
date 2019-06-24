![Detrack logo](https://www.detrack.com/wp-content/uploads/2016/12/Logo_detrack.png)
# detrack-core-dotnet

Official core library for .NET applications to interact with the [Detrack](https://www.detrack.com) API. :thumbsup:

## Installation
Install with dotnet-cli:
```
dotnet add package ?????
```
Add your default API KEY (this can be retrieved from the dashboard web application):
```csharp
using Detrack.DetrackCore;
Job.DefaultApiKey = "keygoeshere";
```
**Note:** All methods are Http requests which is async and therefore needs to be awaited before running the next lines.

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
Job.CreateJob(job).Wait();
```

#### Update a job:
**Note:**
- UpdateJob is a non-static method.
- Both parameters are optional. If not given, it will take the instance's do number and date
```csharp
// instantiate a job class
Job job = new Job("doNumber", "date", "address");

// instance method
job.UpdateJob("doNumber", "date").Wait();
```

#### Retrieve a job:
```csharp
Job.RetrieveJob("doNumber", "date").Wait();
```

#### Delete a job:
**Note:** Job can only be deleted if its status is "info received" or "out for delivery"
```csharp
Job.DeleteJob("doNumber", "date").Wait();
```

#### Reattempt a job:
**Note:** Only failed jobs can be reattempted
```csharp
Job.ReattemptJob("doNumber", "date").Wait();
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
Job.CreateJobs(joblist).Wait();
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
Job.UpdateJobs(joblist).Wait();
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
Job.DeleteJobs(joblist);
```

## Extra Functions
#### List All Jobs:
**Note:** Parameters available is : `page`, `limit`, `date`, `type`, `assignTo`, `JobStatus`, `doNumber`.

```csharp
// create a dictionary with type string as key and values
Dictionary<string,string> parameters = new Dictionary<string,string>();

// add your parameters
parameters.Add("page", 5);
parameters.Add("date", "2019-05-01");
parameters.Add("JobStatus", JobStatus.completed_partial.ToString());

// list all jobs
Job.ListAllJobs(parameters);
```

#### Download Job POD or shipping label as pdf:
**Note:** pathToSaveFile will start in the folder that contains the dll. If you want to go out of the folder then do `../`
```csharp
Job.DownloadJobExport("doNumber", "pathToSaveFile", "documentType(pod or shipping-label)", "date").Wait();
```

## Adding and Removing Items on a Job
**Note:** Updating an item requires you to create a new `Item` class and push the update
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
Job.UpdateJob("doNumber", "date").Wait();
```

#### Removing Items
```csharp
// if you know the index of the item
job.Items.RemoveAt(index);

//if you know the ID of the items
foreach (Item item in job.Items)
{
    if (item.ID == "job id to delete")
    {
        job.Items.Remove(item);
    }
}
```
