using System;
using NUnit.Framework;
using Detrack.DetrackCore;
using System.Collections.Generic;
using Exceptions;

namespace DetrackTest
{
    public class InstantiationOfJob
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void When_JobClassInstantiated_Expect_InputStringNotEmpty()
        {
            Assert.Throws(Is.TypeOf<DONumberEmptyException>().And.Message.EqualTo("DO Number, Address or Date cannot be empty!"),
                () => new Job("", "", ""));

            Assert.Throws(Is.TypeOf<DONumberEmptyException>().And.Message.EqualTo("DO Number, Address or Date cannot be empty!"),
                () => new Job("", "Singapore", "DO 1"));
        }

        [Test]
        public void When_JobClassInstantiated_Expect_DateValid()
        {
            Exception exception = Assert.Throws<InvalidDateException>(() => new Job("DO 1", "2019-15-20", "Singapore"));
            Assert.AreEqual("Month is not a valid number", exception.Message);

            Exception exception1 = Assert.Throws<InvalidDateException>(() => new Job("DO 1", "2019-05-33", "Singapore"));
            Assert.AreEqual("Day is not a valid number", exception1.Message);

            Exception exception2 = Assert.Throws<InvalidDateException>(() => new Job("DO 1", "2019=05=20", "Singapore"));
            Assert.AreEqual("Invalid date format. Date must be integer and date format must be yyyy-mm-dd", exception2.Message);

            Exception exception3 = Assert.Throws<InvalidDateException>(() => new Job("DO 1", "2019-11.5-20", "Singapore"));
            Assert.AreEqual("Invalid date format. Date must be integer and date format must be yyyy-mm-dd", exception3.Message);

            Exception exception4 = Assert.Throws<InvalidDateException>(() => new Job("DO 1", "25-05-2019", "Singapore"));
            Assert.AreEqual("Invalid date format. Date must be integer and date format must be yyyy-mm-dd", exception3.Message);
        }
        
        [Test]
        public void When_JobFieldsUpdated_Expect_Pass()
        {
            Job myjob = new Job("NewJob", "2019-06-11", "Singapore");
            foreach (var property in typeof(Job).GetProperties())
            {
                if (property.ToString().Contains("String"))
                {
                    property.SetValue(myjob, "2019-11-11");
                }

                if (property.ToString().Contains("Int32"))
                {
                    property.SetValue(myjob, 5);
                }

                if (property.ToString().Contains("Boolean"))
                {
                    property.SetValue(myjob, true);
                }
                
                if (property.ToString().Contains("Single"))
                {
                    property.SetValue(myjob, 1.5f);
                }
                
                if (property.ToString().Contains("DetrackCore.Status"))
                {
                    property.SetValue(myjob, Status.info_recv);
                }
                
                if (property.ToString().Contains("Item"))
                {
                    Item item = new Item();
                    List<Item> itemlist = new List<Item>
                    {
                        item
                    };
                    property.SetValue(myjob, itemlist);
                }
            }
        }
    }

    public class CreatingJob
    {
        [Test]
        public void When_DONumberExist_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Job myjob = new Job("when do number exist throw error", "2019-06-11", "Singapore");
            Exception ex = Assert.Throws<AggregateException>(() => Job.CreateJob(myjob).Wait());
            Assert.AreEqual("One or more errors occurred. (DoNumber is already taken)", ex.Message);
        }

        [Test]
        public void When_DONumberDoesntExist_Expect_Pass()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Job myjob = new Job("created", "2019-06-11", "Singapore");
            Job.CreateJob(myjob).Wait();

            Job.DeleteJob("created", "2019-06-11").Wait();
        }

        [Test]
        public void When_APIKeyEmpty_Expect_ThrowError()
        {
            Job myjob = new Job("my api key is empty", "2019-06-11", "Singapore");
            Exception ex = Assert.Throws<AggregateException>(() => Job.CreateJob(myjob).Wait());
            Assert.AreEqual("One or more errors occurred. (API Key cannot be empty!)", ex.Message);
        }
    }

    public class RetrievingJob
    {
        [Test]
        public void When_DONumberDoesntExist_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Exception ex = Assert.Throws<AggregateException>(() => Job.RetrieveJob("i dont exist", "2019-06-11").Wait());
            Assert.AreEqual("One or more errors occurred. (Could not find job with this DO Number)", ex.Message);
        }

        [Test]
        public void When_DateIsInvalid_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Exception ex = Assert.Throws<AggregateException>(() => Job.RetrieveJob("123456", "2019=05=12").Wait());
            Assert.AreEqual("One or more errors occurred. (Invalid date format. Date must be integer and date format must be yyyy-mm-dd)", ex.Message);
        }

        [Test]
        public void When_DONumberExist_Expect_Pass()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Job.RetrieveJob("when do number exist expect pass (retrieve job)", "2019-06-11").Wait();
        }
    }

    public class DeleteJob
    {
        [Test]
        public void When_JobIsAlreadyCompleted_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Exception ex = Assert.Throws<AggregateException>(() => Job.DeleteJob("when job is completed throw error", "2019-06-11").Wait());
            Assert.AreEqual("One or more errors occurred. (Job is either completed, partially completed or failed.)", ex.Message);
        }

        [Test]
        public void When_JobIsPartiallyCompleted_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Exception ex = Assert.Throws<AggregateException>(() => Job.DeleteJob("when job is partially completed throw error", "2019-06-11").Wait());
            Assert.AreEqual("One or more errors occurred. (Job is either completed, partially completed or failed.)", ex.Message);
        }

        [Test]
        public void When_JobIsFailed_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Exception ex = Assert.Throws<AggregateException>(() => Job.DeleteJob("when job is failed throw error", "2019-06-11").Wait());
            Assert.AreEqual("One or more errors occurred. (Job is either completed, partially completed or failed.)", ex.Message);
        }

        [Test]
        public void When_JobIsOutForDelivery_Expect_Pass()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Job.DeleteJob("when job is out for delivery pass", "2019-06-11").Wait();

            Job myjob = new Job("when job is out for delivery pass", "2019-06-11", "Singapore");
            myjob.AssignTo = "Nina";
            Job.CreateJob(myjob).Wait();
        }

        [Test]
        public void When_DONumberDoesntExist_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Exception ex = Assert.Throws<AggregateException>(() => Job.DeleteJob("i dont exist").Wait());
            Assert.AreEqual("One or more errors occurred. (Could not find job with this DO Number)", ex.Message);
        }

        [Test]
        public void When_DateIsInvalid_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Exception ex = Assert.Throws<AggregateException>(() => Job.DeleteJob("123456", "2019=05=12").Wait());
            Assert.AreEqual("One or more errors occurred. (Invalid date format. Date must be integer and date format must be yyyy-mm-dd)", ex.Message);
        }
    }

    public class UpdateJob
    {
        [Test]
        public void When_DONumberDoesNotExist_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Job myjob = new Job("i dont exist", "2019-06-11", "Singapore");
            Exception ex = Assert.Throws<AggregateException>(() => myjob.UpdateJob().Wait());
            Assert.AreEqual("One or more errors occurred. (Could not find job with this DO Number)", ex.Message);
        }

        [Test]
        public void When_JobStatusUpdatedtoCompletedWithoutPODTime_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Job myjob = new Job("when job status updated to c/pc/f without pod time error", "2019-06-11", "Singapore");
            myjob.Status = Status.completed;
            Exception ex = Assert.Throws<AggregateException>(() => myjob.UpdateJob().Wait());
            Console.WriteLine(ex.Message);
            Assert.AreEqual("One or more errors occurred. (PodTime cannot be blank)", ex.Message);
        }

        [Test]
        public void When_JobStatusUpdatedtoPartiallyCompletedWithoutPODTime_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Job myjob = new Job("when job status updated to c/pc/f without pod time error", "2019-06-11", "Singapore");
            myjob.Status = Status.completed_partial;
            Exception ex = Assert.Throws<AggregateException>(() => myjob.UpdateJob().Wait());
            Assert.AreEqual("One or more errors occurred. (PodTime cannot be blank)", ex.Message);
        }

        [Test]
        public void When_JobStatusUpdatedtoFailedWithoutPODTime_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Job myjob = new Job("when job status updated to c/pc/f without pod time error", "2019-06-11", "Singapore");
            myjob.Status = Status.failed;
            Exception ex = Assert.Throws<AggregateException>(() => myjob.UpdateJob().Wait());
            Assert.AreEqual("One or more errors occurred. (PodTime cannot be blank)", ex.Message);
        }

        [Test]
        public void When_JobAssignedToDriver_Expect_JobStatusChangedToOutForDelivery()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Job myjob = new Job("when job assigned to driver expect job status change", "2019-06-11", "Singapore");
            myjob.AssignTo = "Nina";
            myjob.PODTime = "11:00 AM";
            myjob.UpdateJob("when job assigned to driver expect job status change", "2019-06-11").Wait();

            Job.DeleteJob("when job assigned to driver expect job status change", "2019-06-11").Wait();
            Job.CreateJob(myjob).Wait();
        }
    }

    public class ReattemptJob
    {
        [Test]
        public void When_JobIsAlreadyCompleted_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Exception ex = Assert.Throws<AggregateException>(() => Job.ReattemptJob("when job is completed throw error (reattempt job)", "2019-06-11").Wait());
            Assert.AreEqual("One or more errors occurred. (Job must only be failed for reattempt.)", ex.Message);

        }

        [Test]
        public void When_JobIsPartiallyCompleted_Expect_Pass()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Job.ReattemptJob("when job is partially completed expect pass (reattempt job)", "2019-06-11").Wait();
        }

        [Test]
        public void When_JobFailed_Expect_Pass()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Job.ReattemptJob("when job is failed expect pass (reattempt job)", "2019-06-11").Wait();
            
        }

        [Test]
        public void When_JobIsOutForDelivery_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Exception ex = Assert.Throws<AggregateException>(() => Job.ReattemptJob("when job is out for delivery throw error (reattempt job)", "2019-06-11").Wait());
            Assert.AreEqual("One or more errors occurred. (Job must only be failed for reattempt.)", ex.Message);
        }

        [Test]
        public void When_JobIsInfoReceived_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Exception ex = Assert.Throws<AggregateException>(() => Job.ReattemptJob("when job is info received throw error (reattempt job)", "2019-06-11").Wait());
            Assert.AreEqual("One or more errors occurred. (Job must only be failed for reattempt.)", ex.Message);
        }

        [Test]
        public void When_DONumberNotFound_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Exception ex = Assert.Throws<AggregateException>(() => Job.ReattemptJob("i dont exist", "2019-06-11").Wait());
            Assert.AreEqual("One or more errors occurred. (Could not find job with this DO Number)", ex.Message);
        }
    }

    public class CreatingJobs
    {
        [Test]
        public void When_DONumberExist_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            List<Job> jobs = new List<Job>();

            Job job1 = new Job("ppppp", "2019-06-11", "Singapore");
            Job job2 = new Job("when do number exist throw error (creating jobs)", "2019-06-11", "Singapore");
            Job job3 = new Job("oooooo", "2019-06-11", "Singapore");

            jobs.Add(job1);
            jobs.Add(job2);
            jobs.Add(job3);

            Exception ex = Assert.Throws<AggregateException>(() => Job.CreateJobs(jobs).Wait());
            Assert.AreEqual("One or more errors occurred. (DoNumber: when do number exist throw error (creating jobs) is already taken)", ex.Message);

            jobs.Remove(job2);
            Job.DeleteJobs(jobs).Wait();
        }

        [Test]
        public void When_DataIsEmpty_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            List<Job> jobs = new List<Job>();
            Exception ex = Assert.Throws<AggregateException>(() => Job.CreateJobs(jobs).Wait());
            Assert.AreEqual("One or more errors occurred. (JobClass list is empty.)", ex.Message);
        }
    }

    public class UpdatingJobs
    {
        [Test]
        public void When_DONumberDoesNotExist_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            List<Job> jobs = new List<Job>();

            Job job1 = new Job("i dont exist", "2019-06-11", "Singapore");
            Job job2 = new Job("i dont exist too", "2019-06-11", "Singapore");
            Job job3 = new Job("i dont exist too ver 2", "2019-06-11", "Singapore");

            jobs.Add(job1);
            jobs.Add(job2);
            jobs.Add(job3);

            Exception ex = Assert.Throws<AggregateException>(() => Job.UpdateJobs(jobs).Wait());
            Assert.AreEqual("One or more errors occurred. (DO Number: i dont exist. Could not find job with this DO Number.)", ex.Message);
        }

        [Test]
        public void When_JobStatusUpdatedtoCompletedWithoutPODTime_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            List<Job> jobs = new List<Job>();
            Job job2 = new Job("when job status updated to c/pc/f without pod time throw error (updating jobs)", "2019-06-11", "Singapore");

            job2.AssignTo = "Nina";

            job2.Status = Status.completed;

            jobs.Add(job2);

            Exception ex = Assert.Throws<AggregateException>(() => Job.UpdateJobs(jobs).Wait());
            Assert.AreEqual("One or more errors occurred. (DO Number: when job status updated to c/pc/f without pod time throw error (updating jobs). PodTime cannot be blank)", ex.Message);
        }

        [Test]
        public void When_JobStatusUpdatedtoPartiallyCompletedWithoutPODTime_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            List<Job> jobs = new List<Job>();
            Job job1 = new Job("when job status updated to c/pc/f without pod time throw error (updating jobs)", "2019-06-11", "Singapore");
            
            job1.Status = Status.completed_partial;

            job1.AssignTo = "Nina";

            jobs.Add(job1);

            Exception ex = Assert.Throws<AggregateException>(() => Job.UpdateJobs(jobs).Wait());
            Assert.AreEqual("One or more errors occurred. (DO Number: when job status updated to c/pc/f without pod time throw error (updating jobs). PodTime cannot be blank)", ex.Message);
        }

        [Test]
        public void When_JobStatusUpdatedtoFailedWithoutPODTime_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            List<Job> jobs = new List<Job>();
            Job job1 = new Job("when job status updated to c/pc/f without pod time throw error (updating jobs)", "2019-06-11", "Singapore");

            job1.AssignTo = "Nina";

            job1.Status = Status.failed;

            jobs.Add(job1);

            Exception ex = Assert.Throws<AggregateException>(() => Job.UpdateJobs(jobs).Wait());
            Assert.AreEqual("One or more errors occurred. (DO Number: when job status updated to c/pc/f without pod time throw error (updating jobs). PodTime cannot be blank)", ex.Message);
        }

        [Test]
        public void When_DataIsEmpty_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            List<Job> jobs = new List<Job>();
            Exception ex = Assert.Throws<AggregateException>(() => Job.UpdateJobs(jobs).Wait());
            Assert.AreEqual("One or more errors occurred. (JobClass list is empty.)", ex.Message);
        }

        [Test]
        public void When_ItemsAreAddedIntoJob_Expect_Pass()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Job job1 = new Job("when items are added expect pass", "2019-06-11", "Singapore");
            Item item = new Item();
            item.Description = "Lorem ipsum blabla";
            job1.Items.Add(item);
            job1.PODTime = "11:00 AM";
            
            List<Job> jobs = new List<Job>{job1};

            Job.UpdateJobs(jobs).Wait();
        }

        [Test]
        public void When_JobUpdatedToCompleteButAlreadyCompleted()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Job job = new Job("when job updated to complete but already completed throw error", "2019-06-11", "Singapore");
            job.Status = Status.completed;

            List<Job> jobs = new List<Job> {job};

            Exception ex = Assert.Throws<AggregateException>(() => Job.UpdateJobs(jobs).Wait());
            Assert.AreEqual("One or more errors occurred. (DO Number: when job updated to complete but already completed throw error. Unable to update job status as it is already completed/partially completed/failed)", ex.Message);
        }
    }

    public class DeleteJobs
    {
        [Test]
        public void When_DONumberDoesNotExist_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            List<Job> jobs = new List<Job>();
            Job job1 = new Job("i dont exist", "2019-06-11", "Singapore");
            Job job2 = new Job("dummy delete job 1", "2019-06-11", "Singapore");
            Job job3 = new Job("dummy delete job 2", "2019-06-11", "Singapore");

            jobs.Add(job1);
            jobs.Add(job2);
            jobs.Add(job3);

            Exception ex = Assert.Throws<AggregateException>(() => Job.DeleteJobs(jobs).Wait());
            Assert.AreEqual("One or more errors occurred. (DO Number: i dont exist. Could not find job with this DO Number.)", ex.Message);

            jobs.Remove(job1);
            Job.CreateJobs(jobs).Wait();
        }

        [Test]
        public void When_JobIsCompleted_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            List<Job> jobs = new List<Job>();
            Job job1 = new Job("when job is completed throw error", "2019-06-11", "Singapore");
            Job job2 = new Job("dummy delete job 1", "2019-06-11", "Singapore");
            Job job3 = new Job("dummy delete job 2", "2019-06-11", "Singapore");

            jobs.Add(job1);
            jobs.Add(job2);
            jobs.Add(job3);

            Exception ex = Assert.Throws<AggregateException>(() => Job.DeleteJobs(jobs).Wait());
            Assert.AreEqual("One or more errors occurred. (DO Number: when job is completed throw error is either completed, partially completed or failed so it cannot be deleted.)", ex.Message);

            jobs.Remove(job1);
            Job.CreateJobs(jobs).Wait();

        }

        [Test]
        public void When_JobIsPartiallyCompleted_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            List<Job> jobs = new List<Job>();
            Job job1 = new Job("when job is partially completed throw error", "2019-06-11", "Singapore");
            Job job2 = new Job("dummy delete job 1", "2019-06-11", "Singapore");
            Job job3 = new Job("dummy delete job 2", "2019-06-11", "Singapore");

            jobs.Add(job1);
            jobs.Add(job2);
            jobs.Add(job3);

            Exception ex = Assert.Throws<AggregateException>(() => Job.DeleteJobs(jobs).Wait());
            Assert.AreEqual("One or more errors occurred. (DO Number: when job is partially completed throw error is either completed, partially completed or failed so it cannot be deleted.)", ex.Message);

            jobs.Remove(job1);
            Job.CreateJobs(jobs).Wait();
        }

        [Test]
        public void When_JobIsFailed_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            List<Job> jobs = new List<Job>();
            Job job1 = new Job("when job is failed throw error", "2019-06-11", "Singapore");
            Job job2 = new Job("dummy delete job 1", "2019-06-11", "Singapore");
            Job job3 = new Job("dummy delete job 2", "2019-06-11", "Singapore");

            jobs.Add(job1);
            jobs.Add(job2);
            jobs.Add(job3);

            Exception ex = Assert.Throws<AggregateException>(() => Job.DeleteJobs(jobs).Wait());
            Assert.AreEqual("One or more errors occurred. (DO Number: when job is failed throw error is either completed, partially completed or failed so it cannot be deleted.)", ex.Message);

            jobs.Remove(job1);
            Job.CreateJobs(jobs).Wait();
        }

        [Test]
        public void When_DataIsEmpty_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            List<Job> jobs = new List<Job>();
            Exception ex = Assert.Throws<AggregateException>(() => Job.DeleteJobs(jobs).Wait());
            Assert.AreEqual("One or more errors occurred. (JobClass list is empty.)", ex.Message);
        }

        [Test]
        public void When_JobIsOutForDelivery_Expect_Pass()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            List<Job> jobs = new List<Job>();
            Job job1 = new Job("dummy delete job 1", "2019-06-11", "Singapore");
            Job job2 = new Job("when job is out for delivery expect pass (Delete jobs)", "2019-06-11", "Singapore");
            Job job3 = new Job("dummy delete job 2", "2019-06-11", "Singapore");

            jobs.Add(job1);
            jobs.Add(job2);
            jobs.Add(job3);
            
            Job.DeleteJobs(jobs).Wait();

            List<Job> newjobs = new List<Job>();
            Job newjob1 = new Job("dummy delete job 1", "2019-06-11", "Singapore");
            Job newjob2 = new Job("when job is out for delivery expect pass (Delete jobs)", "2019-06-11", "Singapore");
            Job newjob3 = new Job("dummy delete job 2", "2019-06-11", "Singapore");

            newjobs.Add(newjob1);
            newjobs.Add(newjob2);
            newjobs.Add(newjob3);

            Job.CreateJobs(newjobs).Wait();
        }
    }

    public class ListAllJobs
    {
        [Test]
        public void When_ListAllJobWithParams_Expect_Pass()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Dictionary<string, string> param = new Dictionary<string, string>
            {
                {"page", "2"},
                {"limit", "5"},
                {"date", "2019-06-11"},
                {"type", "delivery"},
                {"assignTo", "Nina"},
                {"status", Status.completed.ToString()},
                {"DONumber", "when job is completed throw error"}
            };

            Job.ListAllJobs(param).Wait();
        }

        [Test]
        public void When_ListAllJobsWithNoParams_Expect_Pass()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Dictionary<string, string> param = new Dictionary<string, string>();
            Job.ListAllJobs(param).Wait();
        }

        /*
        [Test]
        public void When_DONumberNotFound_Expect()
        {
            
        }*/

        [Test]
        public void When_DateFormatIsWrong_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Dictionary<string, string> param = new Dictionary<string, string>
            {
                {"page", "2"},
                {"limit", "5"},
                {"date", "2019/06/11"},
                {"type", "delivery"},
                {"assignTo", "Nina"},
                {"status", Status.completed.ToString()},
                {"DONumber", "when job is completed throw error"}
            };

            Exception ex = Assert.Throws<AggregateException>(() => Job.ListAllJobs(param).Wait());
            Assert.AreEqual("One or more errors occurred. (Invalid date format. Date must be integer and date format must be yyyy-mm-dd)", ex.Message);
        }

        [Test]
        public void When_AssignToIsUnassigned_Expect_Pass()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Dictionary<string, string> param = new Dictionary<string, string>
            {
                {"page", "2"},
                {"limit", "5"},
                {"date", "2019-06-11"},
                {"type", "delivery"},
                {"assignTo", "unassigned"},
                {"status", Status.completed.ToString()},
                {"DONumber", "when job is completed throw error"}
            };
            Job.ListAllJobs(param).Wait();
        }

        [Test]
        public void When_KeyIsInvalid_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Dictionary<string, string> param = new Dictionary<string, string>
            {
                {"list", "empty"}
            };
            Exception ex = Assert.Throws<AggregateException>(() => Job.ListAllJobs(param).Wait());
            Assert.AreEqual("One or more errors occurred. (list is an invalid key)", ex.Message);
        }
    }

    public class DownloadJobExport
    {
        [Test]
        public void When_DocumentTypeIsNotPodOrShippingLabel_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Exception ex = Assert.Throws<AggregateException>(() =>
                Job.DownloadJobExport("DownloadJobExport", "", "delivery", "2019-06-11").Wait());
            
            Assert.AreEqual("One or more errors occurred. (Document must be either pod or shipping-label)", ex.Message);
        }

        /*
        [Test]
        public void When_DONumberNotFound_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Exception ex = Assert.Throws<AggregateException>(() => 
                Job.DownloadJobExport("i dont exist", "", "pod", "2019-06-11").Wait());
            Assert.AreEqual("One or more errors occurred. (Document must be either pod or shipping-label)", ex.Message);
        }*/

        [Test]
        public void When_DateFormatIsWrong_Expect_ThrowError()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Exception ex = Assert.Throws<AggregateException>(() =>
                Job.DownloadJobExport("DownloadJobExport", "", "delivery", "2019/06/11").Wait());
            Assert.AreEqual("One or more errors occurred. (Invalid date format. Date must be integer and date format must be yyyy-mm-dd)", ex.Message);
        }

        [Test]
        public void When_DONumberExist_Expect_Pass()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Job.DownloadJobExport("DownloadJobExport", "", "pod", "2019-06-11").Wait();
        }

        [Test]
        public void When_PathIsNotEmpty_Expect_Pass()
        {
            Job.DefaultApiKey = "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0";
            Job.DownloadJobExport("DownloadJobExport", "..", "pod", "2019-06-11").Wait();
        }
    }
    
}