/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
namespace Bam.Net.Automation
{
    public interface IWorker
    {
        Job Job { get; set; }
        string Name { get; set; }
        int StepNumber { get; set; }
        bool Busy { get; set; }
        Status Status { get; set; }
        WorkState SetPropertiesFromWorkState(WorkState state);
        
        /// <summary>
        /// Sets properties for the current worker to the values that apply found in the
        /// specified workstate.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        WorkState WorkState(WorkState state);
        WorkState Do(Job job);        
    }
}
