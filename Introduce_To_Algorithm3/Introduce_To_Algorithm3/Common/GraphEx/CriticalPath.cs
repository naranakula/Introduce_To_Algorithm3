using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    /// <summary>
    /// suppose we have n jobs.job i must end before job j start.we can construct a dag to  solve the problem.
    /// for job i,add two vertex ai,bi, ai-->bi weight at the time 
    /// </summary>
    public class CriticalPath
    {
        public static double Cp(List<Job> jobs)
        {
            //the job id start from 0 to n-1
            int n = jobs.Count;

            for (int i = 0; i < n; i++)
            {
                jobs[i].JobID = i;
            }

            //source and sink vertex
            int source = 2*n;
            int sink = 2*n + 1;

            //build graph
            EdgeWeightedDigraph g = new EdgeWeightedDigraph(2*n+2);
            for (int i = 0; i < n; i++)
            {
                //add edge from source to the start of job
                g.AddEdge(new DirectedEdge(source,i,0));
                //add edge from the end of job to sink
                g.AddEdge(new DirectedEdge(i+n,sink,0));
                //add edge from the start of job to the end of job and weight duration
                g.AddEdge(new DirectedEdge(i,i+n,jobs[i].Duration));


                foreach (var job in jobs[i].Constaints)
                {
                    g.AddEdge(new DirectedEdge(job.JobID+n,i,0));
                }
            }


            //compute longest paths from source to sink
            DagLogestPaths paths = new DagLogestPaths(g,source);
            return paths.DistTo(sink);
        }
    }


    public class Job
    {
        public int JobID { get; set; }

        /// <summary>
        /// the time need to finish job
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// the job need to complete before the start of this job
        /// </summary>
        public List<Job> Constaints { get; set; } 
    }

}
