using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// we consider the parallel precedence-constrained job scheduling prolem:
    /// given a set jobs of specified duration to be completed, with precedence constraints that specify that certain jobs have to be completed before certain jobs are begun, how can we schedule jobs such that all jobs completed in the minimum amount of time while still respect the constraints
    /// 
    /// 
    /// The problem can be solved by formulatingit as a longest paths problem in an edge weighted DAG:
    /// Create an edge-weighted DAG with a source s,a sink t, and two vertices for each job(a start vertex and a end vertex). For each job,add an edge from its start vertex to it end vertex with weight equal to its duration. for each precedence constraint v-->w, add a zero wright edge from the end vertex corresponging v to the beginning vertex  corresponging to w.also add zero weight edges from the source to each job's start vertex and from each job's end vertex to the sink
    /// </summary>
    public class CPM
    {

        public double Cpm(List<Job> jobs)
        {
            //the jobs id start from 0 to n-1
            int n = jobs.Count;
            for (int i = 0; i < n; i++)
            {
                jobs[i].JobID = i;
            }
            //source and sink vertex
            int source = 2 * n;
            int sink = 2 * n + 1;

            //build network
            EdgeWeightedDigraph g = new EdgeWeightedDigraph(2 * n + 2);
            for (int i = 0; i < n; i++)
            {
                g.AddEdge(new DirectedEdge(source, i, 0));
                g.AddEdge(new DirectedEdge(i + n, sink, 0));
                g.AddEdge(new DirectedEdge(i, i + n, 0));

                foreach (var job in jobs[i].Constaints)
                {
                    g.AddEdge(new DirectedEdge(i+n,job.JobID,jobs[i].Duration));
                }

            }

            //compute longest paths
            AcyclicLP lp = new AcyclicLP(g,source);
            return lp.DistTo(sink);
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
