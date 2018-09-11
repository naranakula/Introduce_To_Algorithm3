using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace Quartznet3Learn.Jobs
{
    public class SampleJob:IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.MergedJobDataMap;
            
            return Task.Delay(1);
        }
    }
}
